
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Framework;

namespace assignment_4.Models;

public class PostModel
{
    public PostModel(){}
    
    public PostModel(string title, string summary, string content, ApplicationUserModel user)
    {
        Title = title;
        Summary = summary;
        Content = content;
        ApplicationUser = user;
        ApplicationUserId = user.Id;
        Nickname = user.Nickname;
        //BlogModel = blogModel.Id;
        Time = DateTime.Now;

    }
    
    public int Id { get; set; }
    
    [Required]
    // This already exists in ApplicationUser, but not sure how to access it yet
    public string? Nickname { get; set; }
    
    [Required]
    public string ? Title { get; set; } 
    
    [Required]
    public string? Summary { get; set; }
    
    [Required]
    public string? Content { get; set; }

    public DateTime Time { get; set; } = DateTime.Now;
    
    // This already exists in ApplicationUser, but not sure how to access it yet
    public string ApplicationUserId { get; set; }
    public ApplicationUserModel ApplicationUser { get; set; }
    
}