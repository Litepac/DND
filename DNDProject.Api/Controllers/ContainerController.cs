using DNDProject.Api.Data;
using DNDProject.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DNDProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContainersController : ControllerBase
{
    private readonly AppDbContext _db;
    public ContainersController(AppDbContext db) => _db = db;

    // GET: api/containers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Container>>> GetAll()
        => await _db.Containers.AsNoTracking().ToListAsync();

    // GET: api/containers/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Container>> GetById(int id)
    {
        var c = await _db.Containers.FindAsync(id);
        return c is null ? NotFound() : c;
    }
}
