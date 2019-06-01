using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyIMDB.Data;
using MyIMDB.Interfaces;

namespace MyIMDB.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> repositories;
        private IDbContextTransaction transaction;
        private readonly object createdRepositoryLock;
        private bool transactionClosed;
        private readonly ImdbContext dbContext;

        public UnitOfWork(ImdbContext dbContext)
        {
            this.dbContext = dbContext;
            repositories = new Dictionary<Type, object>();
            createdRepositoryLock = new object();
            transactionClosed = true;
            transaction = null;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            if (!repositories.ContainsKey(typeof(TEntity)))
            {
                lock (createdRepositoryLock)
                {
                    if (!repositories.ContainsKey(typeof(TEntity)))
                    {
                        repositories.Add(typeof(TEntity), new Repository<TEntity>(dbContext));
                    }
                }
            }

            return repositories[typeof(TEntity)] as IRepository<TEntity>;
        }

        public void BeginTransaction()
        {
            if (transactionClosed || transaction == null)
            {
                transaction = dbContext.Database.BeginTransaction();
                transactionClosed = false;
            }
        }

        public void CommitTransaction()
        {
            if (!transactionClosed)
            {
                transaction?.Commit();
                transactionClosed = true;
            }
        }

        public void RollbackTransaction()
        {
            if (!transactionClosed)
            {
                transaction?.Rollback();
                transactionClosed = true;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
