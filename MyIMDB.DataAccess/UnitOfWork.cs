using Microsoft.EntityFrameworkCore.Storage;
using MyIMDB.Data;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;
using System;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDbContextTransaction transaction;
        private bool transactionClosed;
        private readonly ImdbContext dbContext;
        private bool disposed = false;

        private IUserMovieRepository userMoviesRepository;
        private IMovieRepository movieRepository;
        private IUserRepository userRepository;
        private IMoviePersonRepository moviePersonRepository;
        private IRepository<Gender> genderRepository;
        private IRepository<Country> countryRepository;
        private IRepository<MoviePersonType> moviePersonTypeRepository;
        private IRepository<Role> rolesRepository;


        public IUserMovieRepository UserMoviesRepository
        {
            get
            {
                if (userMoviesRepository == null)
                    userMoviesRepository = new UserMovieRepository(dbContext);
                return userMoviesRepository;
            }
        }
        public IMovieRepository MovieRepository
        {
            get
            {
                if (movieRepository == null)
                    movieRepository = new MovieRepository(dbContext);
                return movieRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(dbContext);
                return userRepository;
            }
        }
        public IMoviePersonRepository MoviePersonRepository
        {
            get
            {
                if (moviePersonRepository == null)
                    moviePersonRepository = new MoviePersonRepository(dbContext);
                return moviePersonRepository;
            }
        }
        public IRepository<Gender> GenderRepository
        {
            get
            {
                if (genderRepository == null)
                    genderRepository = new Repository<Gender>(dbContext);
                return genderRepository;
            }
        }
        public IRepository<Country> CountryRepository
        {
            get
            {
                if (countryRepository == null)
                    countryRepository = new Repository<Country>(dbContext);
                return countryRepository;
            }
        }
        public IRepository<MoviePersonType> MoviePersonTypeRepository
        {
            get
            {
                if (moviePersonTypeRepository == null)
                    moviePersonTypeRepository = new Repository<MoviePersonType>(dbContext);
                return moviePersonTypeRepository;
            }
        }
        public IRepository<Role> RolesRepository
        {
            get
            {
                if (rolesRepository == null)
                    rolesRepository = new Repository<Role>(dbContext);
                return rolesRepository;
            }
        }
        public UnitOfWork(ImdbContext dbContext)
        {
            this.dbContext = dbContext;
            transactionClosed = true;
            transaction = null;
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    dbContext.Dispose();
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
    }
}
