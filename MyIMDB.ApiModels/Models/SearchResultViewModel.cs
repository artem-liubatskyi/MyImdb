using System.Collections.Generic;

namespace MyIMDB.ApiModels.Models
{
    public class SearchResultViewModel
    {
        public string searchQuery { get; set; }
        public IEnumerable<MovieListViewModel> Movies { get; set; }
        public IEnumerable<MoviePersonListViewModel> Persons { get; set; }
    }
}
