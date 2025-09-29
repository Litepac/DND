namespace DNDProject.Api.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }

    public ICollection<Container> Containers { get; set; } = new List<Container>();
}
