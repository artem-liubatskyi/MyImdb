using System.Collections.Generic;

namespace MyIMDB.ApiModels.Models
{
    public class MoviePersonViewModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }

        public virtual IEnumerable<MovieListViewModel> Movies { get; set; }

        public string Biography { get; set; }
        
        public string Gender { get; set; }
        
        public string Country { get; set; }

        public string ImageUrl { get; set; }
    }
}
