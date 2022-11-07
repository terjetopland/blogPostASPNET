using assignment_4.Models;
using Microsoft.AspNetCore.Identity;

namespace assignment_4.Data;

public class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUserModel> um, RoleManager<IdentityRole> rm)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var toProduction = false;
        
        // Create roles
        var adminRole = new IdentityRole("Admin");
        rm.CreateAsync(adminRole).Wait();
        var blogUserRole = new IdentityRole("BlogUser");
        rm.CreateAsync(blogUserRole).Wait();

        if (!toProduction)
        {
            // object is stored in RAM
            var admin = new ApplicationUserModel
            {
                UserName = "Admin@yahoo.no", Email = "admin@yahoo.no", EmailConfirmed = true, Nickname = "Admin user"
            };
            var blogUser = new ApplicationUserModel
            {
                UserName = "blogUser@yahoo.no", Email = "blogUser@yahoo.no", EmailConfirmed = true,
                Nickname = "Blog user"
            };


            // Use the functionality of 'UserManager' to add it
            um.CreateAsync(admin, "Password1.").Wait();
            // Use 'UserManager' to add a role
            um.AddToRoleAsync(admin, "Admin").Wait();

            um.CreateAsync(blogUser, "Password1.").Wait();
            um.AddToRoleAsync(blogUser, "BlogUser").Wait();



            //object adminPost1 = new PostModel("Title1","This is adminPost1", "Still on adminPost1", admin, blogmodel);

            var newPosts = new[]
            {
                new PostModel("Title2", "This is adminPost2", "Still on adminPost2", admin),
                new PostModel("Title3", "This is adminPost3", "Still on adminPost3", admin)
            };

            //db.AddRange(adminPost1);


            db.PostModels.AddRange(newPosts);

            db.SaveChanges();
        }
    }
}