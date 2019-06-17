using System.Collections.Generic;

namespace MyIMDB.ApiModels.Models
{
    public class UserPageViewModel
    {
        public string FullName { get; set; }
        public IEnumerable<MovieListViewModel> Rates { get; set; }
        public IEnumerable<MovieListViewModel>  WatchLaterMovies { get; set; }

    }
}
