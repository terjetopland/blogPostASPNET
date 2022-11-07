using assignment_4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace assignment_4.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUserModel>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    
    // Making a Db Set of the model -> uses reflection 
    // public DbSet<ModelName> ModelNames => Set<ModelName>();

    //public DbSet<BlogModel> BlogModels => Set<BlogModel>();
    public DbSet<PostModel> PostModels => Set<PostModel>();
    

    // Not sure if this is needed here since we inherit from ApplicationUserModel
    public DbSet<ApplicationUserModel> ApplicationUserModels => Set<ApplicationUserModel>();
}