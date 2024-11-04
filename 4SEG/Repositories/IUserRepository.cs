using _4SEG.Models;

namespace _4SEG.Repositories
{
    public interface IUserRepository
    {
        Usuario GetByUsername(string username);
    }

}
