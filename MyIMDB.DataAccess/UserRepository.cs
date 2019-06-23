using Microsoft.EntityFrameworkCore;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            return await DbContext.Set<User>().FirstOrDefaultAsync(x => x.EMail == email);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await DbContext.Set<User>().FirstOrDefaultAsync(x => x.Login == username);
        }

        public async Task<User> GetForUserPage(long id)
        {
            return await DbContext.Set<User>().Where(x => x.Id == id)
                .Include(u => u.Movies)
                    .ThenInclude(rate => rate.Movie)
                        .ThenInclude(movie => movie.UserMovies)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetWithMovies(long id)
        {
            return await DbContext.Set<User>().Where(x => x.Id == id)
                .Include(x => x.Movies)
                .FirstAsync();
        }
    }
}
