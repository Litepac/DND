using Microsoft.AspNetCore.Identity;

namespace DNDProject.Api.Models;

// Udvides senere hvis I vil gemme mere pr. bruger
public class ApplicationUser : IdentityUser
{
    // Eksempel: kobling til en kunde (bruges til “Mine containere”)
    public int? CustomerId { get; set; }
}
