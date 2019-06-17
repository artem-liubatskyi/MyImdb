using MyIMDB.Data.Abstraction;

namespace MyIMDB.Data.Entities
{
    public class Gender : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
}
