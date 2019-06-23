using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;
using System;
using System.Threading.Tasks;

namespace MyIMDB.Services
{
    public class MoviePersonService : IMoviePersonService
    {
        private readonly IUnitOfWork Uow;

        public MoviePersonService(IUnitOfWork uow)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }
        public async Task<MoviePerson> Get(long id)
        {
            return await Uow.MoviePersonRepository.GetFull(id);
        }
    }
}
