using MyIMDB.Data.Entities;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetById(long id);
        Task<User> GetByUsername(string username);
        Task<User> GetByEmail(string email);
        Task<User> GetWithMovies(long id);
        Task<User> GetForUserPage(long id);
    }
}
