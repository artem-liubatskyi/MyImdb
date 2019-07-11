using MyIMDB.Data.Entities;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> AddAsync(Review review);
    }
}