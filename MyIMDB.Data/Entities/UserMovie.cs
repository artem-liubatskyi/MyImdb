namespace MyIMDB.Data.Entities
{
    public class UserMovie
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long MovieId { get; set; }
        public Movie Movie { get; set; }
        public int? Rate { get; set; }
        public bool IsInWatchlist { get; set; }
    }
}
