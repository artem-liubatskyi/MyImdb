using System;
using System.Collections.Generic;
using System.Text;

namespace TmdbClient.ApiModels
{
    public class Person
    {
        public string Birthday { get; set; }
        public string Known_for_department { get; set; }
        public object Deathday { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<string> Also_known_as { get; set; }
        public int Gender { get; set; }
        public string Biography { get; set; }
        public double Popularity { get; set; }
        public string Place_of_birth { get; set; }
        public string Profile_path { get; set; }
        public bool Adult { get; set; }
        public string Imdb_id { get; set; }
        public object Homepage { get; set; }
    }
}
