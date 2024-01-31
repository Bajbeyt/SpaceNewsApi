namespace Entities.Models;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime DateOfUpdate { get; set; }
    public string AuthorName { get; set; }
}