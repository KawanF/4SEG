namespace _4SEG.Services;

using _4SEG.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IEmailService emailService, IConfiguration config)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _config = config;
    }

    public string Login(string username, string senha, string ipAtual)
    {
        var user = _userRepository.GetByUsername(username);
        if (user == null || user.IPAutorizado != ipAtual)
        {
            return null; // IP não autorizado ou usuário não encontrado
        }

        var hashedPassword = HashSenha(senha);
        if (user.Senha != hashedPassword)
        {
            // Log em TXT
            LogErro("Senha incorreta.");
            return null;
        }

        // Geração do JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["JwtConfig:Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Perfil)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string HashSenha(string senha)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private void LogErro(string mensagem)
    {
        System.IO.File.AppendAllText("log.txt", $"{DateTime.Now}: {mensagem}\n");
    }

   
}
