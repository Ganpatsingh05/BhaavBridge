
using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(//// Getting  the database connection string.
        "Connection string 'DefaultConnection' was not found. Configure it with .NET User Secrets.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));  //  Registering the database context

// 3. Configure ASP.NET Core Identity.
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
    /////// IndentityRole-->Enables role-base authorization
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cookie configuration is registered on IServiceCollection, not IdentityBuilder.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    // API clients receive HTTP status codes instead of HTML login redirects.
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));

var app = builder.Build();

// Seed the default Identity roles.
using (var scope = app.Services.CreateScope())
{
    await IdentityRoleSeeder.SeedAsync(scope.ServiceProvider);

    // A Development administrator is created only when both local secrets exist.
    // The web application can still run locally without an administrator bootstrap.
    if (app.Environment.IsDevelopment())
    {
        var adminEmail = app.Configuration["AdminBootstrap:Email"];
        var adminPassword = app.Configuration["AdminBootstrap:Password"];

        var hasAdminEmail = !string.IsNullOrWhiteSpace(adminEmail);
        var hasAdminPassword = !string.IsNullOrWhiteSpace(adminPassword);

        if (hasAdminEmail != hasAdminPassword)
        {
            throw new InvalidOperationException(
                "Configure both AdminBootstrap:Email and AdminBootstrap:Password, or leave both unset.");
        }

        if (hasAdminEmail && hasAdminPassword)
        {
            await AdminBootstrapSeeder.SeedAsync(
                scope.ServiceProvider,
                app.Configuration);
        }
        else
        {
            app.Logger.LogInformation(
                "Development administrator bootstrap skipped because AdminBootstrap secrets are not configured.");
        }
    }
}
// 5. Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Authentication must come before authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Map attribute-routed REST API controllers.
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
