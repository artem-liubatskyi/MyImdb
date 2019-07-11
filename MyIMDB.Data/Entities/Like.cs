namespace MyIMDB.Data.Entities
{
    public class Like
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }

        public long ReviewId { get; set; }
        public virtual Review Review { get; set; }

        public bool Liked { get; set; }
    }
}
