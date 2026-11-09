
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BhaavBridge.Web.Controllers;

public class DashboardController : Controller
{
    // Only Farmers can access this dashboard.
    [Authorize(Roles = "Farmer")]
    public IActionResult Farmer()
    {
        return View();
    }

    // Only Buyers can access this dashboard.
    [Authorize(Roles = "Buyer")]
    public IActionResult Buyer()
    {
        return View();
    }

    // Only Administrators can access this dashboard.
    [Authorize(Roles = "Administrator")]
    public IActionResult Admin()
    {
        return View();
    }
}
