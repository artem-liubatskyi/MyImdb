using System;
using System.Collections.Generic;
using System.Text;

namespace MyIMDB.ApiModels.Models
{
    public class UserPageViewModel
    {
        public string FullName { get; set; }
        public IEnumerable<MovieListViewModel> Rates { get; set; }
    }
}
