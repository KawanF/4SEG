using _4SEG.Models;
using _4SEG.Repositories;
using _4SEG.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace _4SEG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UsuarioController(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            // Sanitiza entradas do cadastro
            usuario.Username = SanitizeInput(usuario.Username);
            usuario.Email = SanitizeInput(usuario.Email);
            usuario.Nome = SanitizeInput(usuario.Nome);
            usuario.Perfil = SanitizeInput(usuario.Perfil);
            usuario.IPAutorizado = SanitizeInput(usuario.IPAutorizado);

            if (string.IsNullOrWhiteSpace(usuario.Username) || string.IsNullOrWhiteSpace(usuario.Senha))
            {
                return BadRequest("Username e senha são obrigatórios.");
            }

            // Validação do formato do email
            if (!Regex.IsMatch(usuario.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Email inválido.");
            }

            // Verifica se o usuário já existe
            var existingUser = _userRepository.GetByUsername(usuario.Username);
            if (existingUser != null)
            {
                return Conflict("Usuário já existe.");
            }

            // Criptografar a senha usando SHA256
            usuario.Senha = HashSenha(usuario.Senha);

            // Salvar o usuário no banco de dados
            _userRepository.Create(usuario);

            var otpCode = GenerateOtp();
            _userRepository.SetOtpCode(usuario.Email, otpCode, DateTime.UtcNow.AddMinutes(65));
            var verificationLink = $"{Request.Scheme}://{Request.Host}/api/usuario/2fa/verify?username={usuario.Username}&otpCode={otpCode}";
            _emailService.SendEmail(usuario.Email, "Verifique seu cadastro", $"Clique aqui para verificar: {verificationLink}");


            return Ok("Usuário cadastrado com sucesso.");
        }

        [HttpPost("2fa/verify")]
        public IActionResult Verify2FA([FromBody] TwoFactorRequest request)
        {
            var isValid = _userRepository.ValidateOtpCode(request.Email, request.OtpCode);
            if (isValid)
            {
                return Ok("2FA válido.");
            }
            else
            {
                return BadRequest("2FA inválido.");
            }
        }

        [HttpPost("password/forgot")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Gera token de redefinição
            var resetToken = Guid.NewGuid().ToString();
            user.PasswordRecoveryToken = resetToken;
            user.PasswordRecoveryTokenExpiration = DateTime.UtcNow.AddHours(1);
            _userRepository.Update(user);

            // Envia e-mail com o link de redefinição de senha
            var resetLink = $"{Request.Scheme}://{Request.Host}/reset-password?token={resetToken}";
            var emailBody = $"<p>Para redefinir sua senha, clique no link abaixo:</p><a href='{resetLink}'>Redefinir Senha</a>";
            _emailService.SendEmail(user.Email, "Redefinição de Senha", emailBody);

            return Ok("E-mail de redefinição de senha enviado.");
        }

        [HttpPost("password/reset")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = _userRepository.GetByResetToken(request.Token);
            if (user == null || user.PasswordRecoveryTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest("Token inválido ou expirado.");
            }

            _userRepository.ResetPassword(request.Token, user.Email, HashSenha(request.NewPassword));

            return Ok("Senha redefinida com sucesso.");
        }

        // Função de hash para criptografar a senha
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

        // Função de sanitização de entradas
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Remove tags HTML e outros caracteres potencialmente perigosos
            input = Regex.Replace(input, @"<.*?>", string.Empty);
            input = Regex.Replace(input, @"[^\w\s@.-]", string.Empty); // Permite caracteres alfanuméricos, @, . e -

            return input;
        }

        // Função para gerar código OTP
        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // Gera um código de 6 dígitos
        }
    }
}


public class TwoFactorRequest
{
    public string Email { get; set; }
    public string OtpCode { get; set; } 
}

public class ForgotPasswordRequest
{
    public string Email { get; set; }
}

public class ResetPasswordRequest
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; } 
}