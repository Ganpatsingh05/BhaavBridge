
using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Profile/Farmer
    [Authorize(Roles = "Farmer")]
    [HttpGet]
    public async Task<IActionResult> Farmer()
    {
        var userId = _userManager.GetUserId(User);

        var user = await _userManager.FindByIdAsync(userId!);

        var profile = await _context.FarmerProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (user == null || profile == null)
            return NotFound("Farmer profile not found.");

        var model = new FarmerProfileViewModel
        {
            FullName = user.FullName,
            Village = profile.Village,
            District = profile.District,
            State = profile.State,
            FarmName = profile.FarmName
        };

        return View(model);
    }

    // POST: /Profile/Farmer
    [Authorize(Roles = "Farmer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Farmer(FarmerProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = _userManager.GetUserId(User);

        var user = await _userManager.FindByIdAsync(userId!);

        var profile = await _context.FarmerProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (user == null || profile == null)
            return NotFound("Farmer profile not found.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        user.FullName = model.FullName.Trim();

        var userResult = await _userManager.UpdateAsync(user);

        if (!userResult.Succeeded)
        {
            await transaction.RollbackAsync();

            foreach (var error in userResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        profile.Village = model.Village.Trim();
        profile.District = model.District.Trim();
        profile.State = model.State.Trim();
        profile.FarmName = string.IsNullOrWhiteSpace(model.FarmName)
            ? null
            : model.FarmName.Trim();

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["SuccessMessage"] = "Farmer profile updated successfully.";

        return RedirectToAction(nameof(Farmer));
    }

    // GET: /Profile/Buyer
    [Authorize(Roles = "Buyer")]
    [HttpGet]
    public async Task<IActionResult> Buyer()
    {
        var userId = _userManager.GetUserId(User);

        var user = await _userManager.FindByIdAsync(userId!);

        var profile = await _context.BuyerProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (user == null || profile == null)
            return NotFound("Buyer profile not found.");

        var model = new BuyerProfileViewModel
        {
            FullName = user.FullName,
            BusinessName = profile.BusinessName,
            ContactPerson = profile.ContactPerson,
            Address = profile.Address,
            District = profile.District,
            State = profile.State
        };

        return View(model);
    }

    // POST: /Profile/Buyer
    [Authorize(Roles = "Buyer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Buyer(BuyerProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = _userManager.GetUserId(User);  ///They retrieve the ID of the currently logged-in user.

        var user = await _userManager.FindByIdAsync(userId!);

        var profile = await _context.BuyerProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId); ///This ensures that the controller retrieves only the profile belonging to that user.

        if (user == null || profile == null)
            return NotFound("Buyer profile not found.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        user.FullName = model.FullName.Trim();

        var userResult = await _userManager.UpdateAsync(user);

        if (!userResult.Succeeded)
        {
            await transaction.RollbackAsync();

            foreach (var error in userResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        profile.BusinessName = model.BusinessName.Trim();
        profile.ContactPerson = model.ContactPerson.Trim();
        profile.Address = model.Address.Trim();
        profile.District = model.District.Trim();
        profile.State = model.State.Trim();

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["SuccessMessage"] = "Buyer profile updated successfully.";

        return RedirectToAction(nameof(Buyer));
    }
}
