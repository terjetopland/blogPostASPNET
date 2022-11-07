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
    private UserManager<ApplicationUserModel> _um;
    private RoleManager<IdentityRole> _rm;
    private readonly SignInManager<ApplicationUserModel> _signInManager;

    public BlogController(ApplicationDbContext db, UserManager<ApplicationUserModel> um, RoleManager<IdentityRole> rm, ILogger<BlogController> logger, SignInManager<ApplicationUserModel> signInManager)
    {
        _db = db;
        _um = um;
        _rm = rm;
        _logger = logger;
        _signInManager = signInManager;
    }
// https://uia.instructure.com/courses/11691/external_tools/170
    
    // GET
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        // making temporary models to send to the view
        var posts = await _db.PostModels
            .Include(p => p.ApplicationUser)
            .Select( p => new PostModel
        {
            Id = p.Id,
            Title = p.Title,
            Summary = p.Summary,
            Content = p.Content,
            ApplicationUserId = p.ApplicationUserId,
            ApplicationUser = p.ApplicationUser,
            Nickname = p.Nickname,
            //BlogModel = blogModel.Id;
            Time = p.Time
        } )
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
        var applicationUserModel = await _um.GetUserAsync(User);
        
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
        var applicationUserModel = await _um.GetUserAsync(User);


        if (_signInManager.IsSignedIn(User) && _um.GetUserAsync(User) != null)
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
        var applicationUserModel = await _um.GetUserAsync(User);
        var post = await _db.PostModels.FindAsync(id);
        

        if (_signInManager.IsSignedIn(User) && applicationUserModel != null && applicationUserModel.Id == post.ApplicationUserId) 
        {
            
            return View(post);
        }

        if (_signInManager.IsSignedIn(User) && applicationUserModel.Id != post.ApplicationUserId)
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
        var applicationUserModel = await _um.GetUserAsync(User);
        
      
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