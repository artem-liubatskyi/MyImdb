using System.Threading.Tasks;
using MyIMDB.Data.Entities;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IUserMovieRepository
    {
        Task<UserMovie> Add(UserMovie movie);
        UserMovie Update(UserMovie entity);
    }
}
