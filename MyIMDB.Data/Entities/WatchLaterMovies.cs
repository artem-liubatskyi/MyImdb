namespace MyIMDB.Data.Entities
{
    public class WatchLaterMovies
    {
        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public long UsereId { get; set; }
        public virtual User User { get; set; }
    }
}
