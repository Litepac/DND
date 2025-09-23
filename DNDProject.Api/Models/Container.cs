namespace DNDProject.Api.Models;

public class Container
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // fx Pap/Plast/Rest
    public int SizeLiters { get; set; }              // Størrelse i liter
    public double WeeklyAmountKg { get; set; }       // Mængde pr. uge (kg)
}
