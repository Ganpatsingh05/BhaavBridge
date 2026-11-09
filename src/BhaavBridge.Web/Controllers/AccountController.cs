
using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;
public AccountController(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context,
    SignInManager<ApplicationUser> signInManager)
{
    _userManager = userManager;
    _context = context;
    _signInManager = signInManager;
}

    // GET: /Account/FarmerRegister
    [HttpGet]
    public IActionResult FarmerRegister()
    {
        return View();
    }

    // POST: /Account/FarmerRegister
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FarmerRegister(
        FarmerRegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        var user = new ApplicationUser
        {
            FullName = model.FullName.Trim(),
            UserName = model.Email.Trim(),
            Email = model.Email.Trim()
        };

        var createResult = await _userManager.CreateAsync(
            user, model.Password);

        if (!createResult.Succeeded)
        {
            await transaction.RollbackAsync();

            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        var roleResult = await _userManager.AddToRoleAsync(
            user, "Farmer");

        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync();

            foreach (var error in roleResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        var farmerProfile = new FarmerProfile
        {
            UserId = user.Id,
            Village = model.Village.Trim(),
            District = model.District.Trim(),
            State = model.State.Trim(),
            FarmName = string.IsNullOrWhiteSpace(model.FarmName)
                ? null
                : model.FarmName.Trim()
        };

        _context.FarmerProfiles.Add(farmerProfile);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["SuccessMessage"] =
            "Farmer registration completed successfully. You can now log in.";

        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/BuyerRegister
    [HttpGet]
    public IActionResult BuyerRegister()
    {
        return View();
    }

    // POST: /Account/BuyerRegister
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BuyerRegister(
        BuyerRegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        var user = new ApplicationUser
        {
            FullName = model.FullName.Trim(),
            UserName = model.Email.Trim(),
            Email = model.Email.Trim()
        };

        var createResult = await _userManager.CreateAsync(
            user, model.Password);

        if (!createResult.Succeeded)
        {
            await transaction.RollbackAsync();

            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        var roleResult = await _userManager.AddToRoleAsync(
            user, "Buyer");

        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync();

            foreach (var error in roleResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        var buyerProfile = new BuyerProfile
        {
            UserId = user.Id,
            BusinessName = model.BusinessName.Trim(),
            ContactPerson = model.ContactPerson.Trim(),
            Address = model.Address.Trim(),
            District = model.District.Trim(),
            State = model.State.Trim()
        };

        _context.BuyerProfiles.Add(buyerProfile);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["SuccessMessage"] =
            "Buyer registration completed successfully. You can now log in.";

        return RedirectToAction("Index", "Home");
	}
    [HttpGet] // GET: /Account/Login
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email.Trim(),
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);
if (result.Succeeded)
{
    // If the user was trying to access a specific local page,
    // send them there after successful login.
    if (!string.IsNullOrEmpty(returnUrl) &&
        Url.IsLocalUrl(returnUrl))
    {
        return Redirect(returnUrl);
    }

    // Find the logged-in user.
    var user = await _userManager.FindByEmailAsync(
        model.Email.Trim());

    if (user != null)
    {
        // Get all roles assigned to this user.
        var roles = await _userManager.GetRolesAsync(user);

        // Redirect according to the user's role.
        if (roles.Contains("Farmer"))
        {
            return RedirectToAction("Farmer", "Dashboard");
        }

        if (roles.Contains("Buyer"))
        {
            return RedirectToAction("Buyer", "Dashboard");
        }

        if (roles.Contains("Administrator"))
        {
            return RedirectToAction("Admin", "Dashboard");
        }
    }

    // Fallback if no recognized role is assigned.
    return RedirectToAction("Index", "Home");
}


        ModelState.AddModelError(
            string.Empty,
            "Invalid email or password.");

        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
    // GET: /Account/AccessDenied
[HttpGet]
public IActionResult AccessDenied()
{
    return View();
}

}
