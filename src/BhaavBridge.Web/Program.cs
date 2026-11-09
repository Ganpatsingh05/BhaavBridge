
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
    .AddIdentity<ApplicationUser, IdentityRole>() // it register the identity authentication System
    /////// IndentityRole-->Enables role-base authorization
    .AddEntityFrameworkStores<ApplicationDbContext>()/////// it stores the information in SQL Server
    .AddDefaultTokenProviders(); // enables toekn generation for operations such as password reset

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the default Identity roles.
using (var scope = app.Services.CreateScope())
{
    await IdentityRoleSeeder.SeedAsync(scope.ServiceProvider);

    // Create the administrator account only in Development.
    if (app.Environment.IsDevelopment())
    {
        await AdminBootstrapSeeder.SeedAsync(
            scope.ServiceProvider,
            app.Configuration);
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
