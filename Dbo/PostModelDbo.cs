using assignment_4.Models;

namespace assignment_4.Dbo;

public class PostModelDbo
{
    public int Id { get; set; }
    public string? Nickname { get; set; }
    public string ? Title { get; set; } 
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    
    public bool CanEdit { get; set; }
}