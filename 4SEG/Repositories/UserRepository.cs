using _4SEG.Data;
using _4SEG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace _4SEG.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Usuario GetByUsername(string username)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Username == username);
        }

        // Buscar usuário por email
        public Usuario GetByEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
        }

        // Criar um novo usuário
        public void Create(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        // Atualizar informações do usuário
        public void Update(Usuario usuario)
        {
            var existingUser = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (existingUser != null)
            {
                existingUser.Username = usuario.Username;
                existingUser.Email = usuario.Email;
                existingUser.Senha = usuario.Senha;
                existingUser.Perfil = usuario.Perfil;
                existingUser.IPAutorizado = usuario.IPAutorizado;

                _context.SaveChanges();
            }
        }

        // **Nova Implementação para Recuperação de Senha**
        public void GeneratePasswordRecoveryToken(string email, string token, DateTime expiration)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.PasswordRecoveryToken = token;
                user.PasswordRecoveryTokenExpiration = expiration;
                _context.SaveChanges();
            }
        }

        public void ResetPassword(string token, string email, string newPassword)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.PasswordRecoveryToken == token && u.PasswordRecoveryTokenExpiration > DateTime.UtcNow);
            if (user != null)
            {
                user.Senha = newPassword;
                user.PasswordRecoveryToken = "NULL";
                user.PasswordRecoveryTokenExpiration = null;
                _context.SaveChanges();
            }
        }

        // **Implementação para 2FA (OTP)**
        public void SetOtpCode(string email, string otpCode, DateTime expiration)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.OtpCode = otpCode;
                user.OtpExpiration = expiration;
                _context.SaveChanges();
            }
        }

        public void InvalidateOtpCode(string email)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.OtpCode = "NULL";
                user.OtpExpiration = null;
                _context.SaveChanges();
            }
        }

        public bool ValidateOtpCode(string email, string otpCode)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.OtpCode == otpCode && u.OtpExpiration > DateTime.UtcNow);
            if (user != null)
            {
                InvalidateOtpCode(email); // Invalidar após validação
                return true;
            }
            return false;
        }

        public Usuario GetByResetToken(string token)
        {
            return _context.Usuarios.FirstOrDefault(u => u.PasswordRecoveryToken == token && u.PasswordRecoveryTokenExpiration > DateTime.UtcNow);
        }

    }
}