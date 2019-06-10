namespace MyIMDB.ApiModels.Models
{
    public class MovieListViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double AverageRate { get; set; }
        public string ImageUrl { get; set; }
        public int UsersRate { get; set; }
        public bool isInWatchlist { get; set; }
    }
}
