using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;

namespace MyIMDB.Services
{
    public interface IMoviePersonService
    {
        Task<MoviePersonViewModel> Get(long id);
    }
}
