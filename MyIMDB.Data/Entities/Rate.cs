using MyIMDB.Interfaces;

namespace MyIMDB.Data.Entities
{
    public class Rate
    {
        public int Value { get; set; }

        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public long ProfileId { get; set; }
        public User Profile { get; set; }
    }
}
