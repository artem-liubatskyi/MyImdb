using MyIMDB.Data.Entities;
using System.Threading.Tasks;

namespace MyIMDB.Services
{
    public interface IMoviePersonService
    {
        Task<MoviePerson> Get(long id);
    }
}
