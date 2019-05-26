using System;
using System.Text;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyIMDB.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork Uow;

        public AccountService(IUnitOfWork uow)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await Uow.Repository<User>().GetQueryable().FirstOrDefaultAsync(x => x.Login == username);
            
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (Uow.Repository<User>().GetQueryable().Any(x => x.Login == user.Login))
                throw new Exception("Username \"" + user.Login + "\" is already taken");

            if (Uow.Repository<User>().GetQueryable().Any(x => x.EMail == user.EMail))
                throw new Exception("Email \"" + user.EMail + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            Uow.Repository<User>().Update(user);
            Uow.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = Uow.Repository<User>().GetQueryable().FirstOrDefault(x=>x.Id==userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            if (userParam.Login != user.Login)
            {
                if (Uow.Repository<User>().GetQueryable().Any(x => x.Login == userParam.Login))
                    throw new Exception("Username " + userParam.Login + " is already taken");
            }
            
            user.FullName = userParam.FullName;
            user.Login = userParam.Login;
            
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            Uow.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = Uow.Repository<User>().GetQueryable().FirstOrDefault(x=>x.Id==id);
            if (user != null)
            {
                Uow.Repository<User>().Delete(user);
                Uow.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        public User GetById(int id)
        {
            return Uow.Repository<User>().GetQueryable().FirstOrDefault(x=>x.Id==id);
        }
        public async Task<RegisterDataModel> GetRegistrationData()
        {
            var genders = await Uow.Repository<Gender>().GetQueryable().Select(x=>new GenderModel
            {
                Id=x.Id,
                Title=x.Title
            }).ToArrayAsync();

            var countries = await Uow.Repository<Country>().GetQueryable().Select(x=> new CountryModel
            {
                Id =x.Id,
                Name=x.Name
            }).ToArrayAsync();
            return new RegisterDataModel() { Genders = genders, Countries = countries };
        }
        public async Task<UserPageViewModel> GetUserPageModel(long userId)
        {
            var user = await Uow.Repository<User>().Get(userId)
                .Include(u => u.Rates)
                .ThenInclude(rate => rate.Movie)
                .ThenInclude(movie=>movie.Rates)
                .FirstOrDefaultAsync();

            var rates = user.Rates.Select(x => new MovieListViewModel()
            {
                Id = x.MovieId,
                Title = x.Movie.Title,
                Year = x.Movie.Year,
                AverageRate = x.Movie.Rates.Any() ? (x.Movie.Rates.Sum(m=>m.Value)/x.Movie.Rates.Count()) : 0,
                UsersRate = x.Value,
                ImageUrl = x.Movie.ImageUrl
            });
            return new UserPageViewModel() { FullName = user.FullName, Rates = rates };

        }
    }
}
