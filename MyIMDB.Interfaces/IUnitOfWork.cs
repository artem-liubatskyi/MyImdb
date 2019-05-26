namespace MyIMDB.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void SaveChanges();
    }
}
