namespace _4SEG.Controllers;
using _4SEG.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var token = _authService.Login(loginRequest.Username, loginRequest.Senha, loginRequest.IpAutorizado);

        if (token == null)
    {
        return Unauthorized("Credenciais inválidas.");
    }

    // Salva o token JWT em um cookie
    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
    {
        HttpOnly = true,
        Secure = true, // Use true em produção (HTTPS)
        Expires = DateTime.UtcNow.AddHours(1)
    });

    return Ok(new { Message = "Login bem-sucedido." });
    }
}


public class LoginRequest
{
    public string Username { get; set; }
    public string Senha { get; set; }
    public string IpAutorizado { get; set; }

}

