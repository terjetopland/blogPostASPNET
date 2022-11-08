using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using assignment_4.Data;
using Microsoft.AspNetCore.Mvc;
using assignment_4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace assignment_4.Controllers;

public class BlogController : Controller
{
    
    private readonly ILogger<BlogController> _logger;
    private ApplicationDbContext _db;
    private readonly UserManager<ApplicationUserModel> _userManager;
    private RoleManager<IdentityRole> _rm;
    private readonly SignInManager<ApplicationUserModel> _signInManager;

    public BlogController(ApplicationDbContext db, UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> rm, ILogger<BlogController> logger, SignInManager<ApplicationUserModel> signInManager)
    {
        _db = db;
        _userManager = userManager;
        _rm = rm;
        _logger = logger;
        _signInManager = signInManager;
    }
// https://uia.instructure.com/courses/11691/external_tools/170
    
    // GET
    [HttpGet]
    //[Authorize(Roles = "BlogUser")] // only users with the role BlogUser can access this page
    [Authorize]
    public async Task<IActionResult> Index()
    {
        // Get the user who are asking for data
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        
        var posts = await _db.PostModels
                // hvor post sin ApplictationUserId er samme som currentUser.Id
                // select * 
                // from PostModels
                // where ApplicationUserId = {{currentUser.Id}}
                // order by PostModels.PostDate desc
            //.Where(post => post.ApplicationUserId == currentUser.Id)
            .OrderByDescending(p => p.Time)
            .ToListAsync();

        return View(posts);
    }
    
    [HttpGet]
    // Trying out roles. Now only admin can access!
    //[Authorize(Roles = "Admin")]
    [Authorize]
    public async Task<IActionResult> Add()
    {
        var applicationUserModel = await _userManager.GetUserAsync(User);
        
        if (_signInManager.IsSignedIn(User) && applicationUserModel != null)
        {
            return View(new PostModel());
        }

        return Redirect("/Identity/Account/Login");

    }
    
    //Post
    [HttpPost]
    [Authorize]
    public async Task<RedirectResult> Add(PostModel postModel)
    {
        var applicationUserModel = await _userManager.GetUserAsync(User);


        if (_signInManager.IsSignedIn(User) && _userManager.GetUserAsync(User) != null)
        {
            //postModel.Nickname = applicationUserModel.Nickname;
            postModel.ApplicationUser = applicationUserModel;
            postModel.Nickname = applicationUserModel.Nickname;
            postModel.Time = DateTime.Now;

            
            _db.AddRange(postModel);
            _db.SaveChanges();
            

        }
        return Redirect("/Blog/Index");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _db.PostModels.FindAsync(id);
        // Get the user who are asking for data
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        if (_signInManager.IsSignedIn(User) && currentUser != null && currentUser.Id == post.ApplicationUserId) 
        {
            
            return View(post);
        }

        if (_signInManager.IsSignedIn(User) && currentUser != null && currentUser.Id != post.ApplicationUserId)
        {
            return Redirect("/Blog/EditErrror");
        }
        return Redirect("/Identity/Account/Login");

    }
    /*
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var applicationUserModel = await _um.GetUserAsync(User);
        var post = await _db.PostModels.FindAsync(id);
        
        if (_signInManager.IsSignedIn(User) && applicationUserModel != null && applicationUserModel.Nickname == post.Nickname)
        {
            
            return View(post);
        }
        return Redirect("/Identity/Account/Login");

    }
   */
    //Post
    [HttpPost]
    [Authorize]
    public async Task<RedirectResult> Edit(PostModel postModel)
    {
        var applicationUserModel = await _userManager.GetUserAsync(User);
        
      
            //postModel.Nickname = applicationUserModel.Nickname;
            postModel.Time = DateTime.Now;
            postModel.Nickname = applicationUserModel.Nickname;
            postModel.ApplicationUser = applicationUserModel;
            
                _db.PostModels.Update(postModel);
                _db.SaveChanges();

                return Redirect("/Blog/Index");
        

    }
    
    //Get
    [HttpGet]
    public IActionResult EditErrror()
    {
        return View();
    }
    
}