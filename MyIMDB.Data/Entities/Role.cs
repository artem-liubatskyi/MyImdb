using Microsoft.AspNetCore.Identity;
using MyIMDB.Data.Abstraction;

namespace MyIMDB.Data.Entities
{
    public class Role : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
