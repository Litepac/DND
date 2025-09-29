using System.ComponentModel.DataAnnotations;

namespace DNDProject.Api.Models;

public class PickupEvent
{
    public int Id { get; set; }

    [Required]
    public int ContainerId { get; set; }
    public Container? Container { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }  // afhentningsdato/-tid

    public double? FillPct { get; set; }     // fyldningsgrad målt
    public double? WeightKg { get; set; }    // vægt ved afhentning
    public double? DistanceKm { get; set; }  // kørte km (hvis leveres i dump)
    public double? Co2Kg { get; set; }       // beregnet/leveret CO2
}
