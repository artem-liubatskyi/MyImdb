using System.Collections.Generic;

namespace MyIMDB.ApiModels.Models
{
    public class MovieViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double AverageRate { get; set; }
        public double UsersRate { get; set; }
        public IEnumerable<string> Genres { get; set; }
        public IEnumerable<MoviePersonListViewModel> Directors { get; set; }
        public IEnumerable<MoviePersonListViewModel> Stars { get; set; }
        public virtual IEnumerable<string> Countries { get; set; }
        public bool isInWatchlist { get; set; }
    }
}
