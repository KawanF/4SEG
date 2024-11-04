using _4SEG.Data;
using _4SEG.Models;

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
            return _context.Usuarios.SingleOrDefault(u => u.Username == username);
        }
    }
}