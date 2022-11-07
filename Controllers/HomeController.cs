using System.Diagnostics;

using assignment_4.Data;
using Microsoft.AspNetCore.Mvc;
using assignment_4.Models;
using Microsoft.AspNetCore.Identity;

namespace assignment_4.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _db;
    private UserManager<ApplicationUserModel> _um;
    private RoleManager<IdentityRole> _rm;

    public HomeController(ApplicationDbContext db, UserManager<ApplicationUserModel> um, RoleManager<IdentityRole> rm, ILogger<HomeController> logger)
    {
        _db = db;
        _um = um;
        _rm = rm;
        _logger = logger;
    }
    
    

    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        return View(user);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}