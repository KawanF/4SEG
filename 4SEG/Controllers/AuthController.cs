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
        var token = _authService.Login(loginRequest.Username, loginRequest.Password, HttpContext.Connection.RemoteIpAddress.ToString());

        if (token == null)
            return Unauthorized("Credenciais inválidas");

        return Ok(new { Token = token });
    }
}


public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

