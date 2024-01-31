namespace Entities.Models;

public class Rocket
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } //tipi
    public string Details { get; set; } //özellikleri
    public DateTime LaunchDate { get; set; } //fırlatılma tarihi
    
    public ICollection<News> News { get; set; }
}