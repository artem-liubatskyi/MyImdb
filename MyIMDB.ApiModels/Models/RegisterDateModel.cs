using System.Collections.Generic;

namespace MyIMDB.ApiModels.Models
{
    public class RegisterDataModel
    {
        public IEnumerable<GenderModel> Genders {get; set; }
        public IEnumerable<CountryModel> Countries { get; set; }

    }
}
