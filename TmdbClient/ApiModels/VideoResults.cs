using System.Collections.Generic;

namespace TmdbClient.ApiModels
{
    public class VideoResults
    {
        public int id { get; set; }
        public List<Video> results { get; set; }
    }
}
