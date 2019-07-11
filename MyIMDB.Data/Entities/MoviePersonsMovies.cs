namespace MyIMDB.Data.Entities
{
    public class MoviePersonsMovies
    {
        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public long MoviePersonId { get; set; }
        public virtual MoviePerson Person { get; set; }

        public long MoviePersonTypeId { get; set; }
        public virtual MoviePersonType MoviePersonType { get; set; }

        public string Character { get; set; }
    }
}