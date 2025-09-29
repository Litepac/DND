using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DNDProject.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DNDProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _cfg;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration cfg)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _cfg = cfg;
    }

    // ---------- DTO'er ----------
    public record LoginRequest(string Email, string Password);

    public record LoginResponse(
        string Token,
        DateTime Expires,
        string Email,
        IEnumerable<string> Roles,
        int? CustomerId);

    // ---------- POST /api/auth/login ----------
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest req)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);
        if (user is null) return Unauthorized();

        var ok = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: false);
        if (!ok.Succeeded) return Unauthorized();

        var (token, expires, roles, custId) = await CreateJwtAsync(user);
        return new LoginResponse(token, expires, user.Email!, roles, custId);
    }

    // ---------- GET /api/auth/ping-protected ----------
    // Lille test-endpoint så du kan se at token virker i Swagger
    [HttpGet("ping-protected")]
    [Authorize]
    public IActionResult PingProtected()
        {
        var email = User.Identity?.Name ?? "(ukendt)";
        var roles = User.Claims
                    .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                    .Select(c => c.Value);

    return Ok(new { message = "OK", email, roles });
}


    // ---------- Helper: byg JWT ----------
    private async Task<(string token, DateTime expires, IEnumerable<string> roles, int? customerId)>
        CreateJwtAsync(ApplicationUser user)
    {
        var jwtSection = _cfg.GetSection("Jwt");

        var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("Missing Jwt:Issuer");
        var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("Missing Jwt:Audience");
        var keyStr = jwtSection["Key"] ?? throw new InvalidOperationException("Missing Jwt:Key");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Email ?? user.UserName ?? user.Id),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(ClaimTypes.NameIdentifier, user.Id),
        new(ClaimTypes.Name, user.Email ?? user.UserName ?? "")
    };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        int? customerId = user.CustomerId;
        if (customerId.HasValue)
            claims.Add(new Claim("customerId", customerId.Value.ToString()));

        var expires = DateTime.UtcNow.AddHours(8);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = creds
        };

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(descriptor);
        string tokenString = handler.WriteToken(securityToken);

        return (tokenString, expires, roles, customerId);
    }
}