using Microsoft.AspNetCore.Identity;

namespace assignment_4.Models;

public class ApplicationUserModel : IdentityUser
{
    public string Nickname { get; set; } = string.Empty;
    public int? Age { get; set; }
    
    

}