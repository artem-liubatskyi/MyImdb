using System.Threading.Tasks;
using MyIMDB.Data.Abstraction;
using MyIMDB.Data.Entities;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IUserMovieRepository UserMoviesRepository { get; }
        IMovieRepository MovieRepository { get; }
        IUserRepository UserRepository { get; }
        IMoviePersonRepository MoviePersonRepository { get; }
        IRepository<Gender> GenderRepository { get; }
        IRepository<Country> CountryRepository { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task<int> SaveChangesAsync();
    }
}
