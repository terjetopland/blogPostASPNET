using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace assignment_4.Controllers;

public class BlogAdminController : Controller
{
    // GET
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        return View();
    }
}