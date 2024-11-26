using _4SEG.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace _4SEG.Repositories
{
    public interface IUserRepository
    {
        Usuario GetByUsername(string username);
        Usuario GetByEmail(string email);
        Usuario GetByResetToken(string token);
        void Create(Usuario usuario);
        void Update(Usuario usuario);
        void GeneratePasswordRecoveryToken(string email, string token, DateTime expiration);
        void ResetPassword(string token, string email, string newPassword);
        void SetOtpCode(string email, string otpCode, DateTime expiration);
        void InvalidateOtpCode(string email);
        bool ValidateOtpCode(string email, string otpCode);

        
    }


}
