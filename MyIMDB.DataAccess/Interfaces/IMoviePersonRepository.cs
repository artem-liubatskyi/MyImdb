using MyIMDB.Data.Entities;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IMoviePersonRepository : IRepository<MoviePerson>
    {
        Task<MoviePerson> GetFull(long id);
    }
}
