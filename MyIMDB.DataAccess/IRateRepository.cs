using MyIMDB.Data.Entities;

namespace MyIMDB.DataAccess
{
    public interface IRateRepository
    {
        Rate Add(Rate entity);
        Rate Update(Rate entity);
    }
}
