using MyIMDB.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IMoviePersonRepository : IRepository<MoviePerson>
    {
        Task<MoviePerson> GetFull(long id);
        Task<IEnumerable<MoviePerson>> GetBySearchQuery(string searchQuery);
    }
}
