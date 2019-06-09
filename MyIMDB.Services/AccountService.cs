﻿using System;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyIMDB.Services.Hashing;
using MyIMDB.Services.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;


namespace MyIMDB.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork Uow;
        private readonly IHasher Hasher;
        private readonly IHttpContextAccessor httpAccessor;

        public AccountService(IUnitOfWork uow, IHasher hasher, IHttpContextAccessor httpContextAccessor)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
            Hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            httpAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await Uow.Repository<User>().GetQueryable().FirstOrDefaultAsync(x => x.Login == username);
            
            if (user == null)
                return null;

            if (await Hasher.VerifyPasswordHash(password, new PasswordHash(user.PasswordHash, user.PasswordSalt)))
                return user;

            return  null;
        }
        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (await Uow.Repository<User>().GetQueryable().AnyAsync(x => x.Login == user.Login))
                throw new Exception("Username \"" + user.Login + "\" is already taken");

            if (await Uow.Repository<User>().GetQueryable().AnyAsync(x => x.EMail == user.EMail))
                throw new Exception("Email \"" + user.EMail + "\" is already taken");

            PasswordHash ph = await Hasher.CreatePasswordHash(password);

            user.PasswordHash = ph.Hash;
            user.PasswordSalt = ph.Salt;

            await Uow.Repository<User>().Add(user);
            await Uow.SaveChangesAsync();

            return user;
        }
        public async Task Update(User userParam, string password = null)
        {
            var user = await Uow.Repository<User>().GetQueryable().FirstOrDefaultAsync(x=>x.Id==userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            if (userParam.Login != user.Login)
            {
                if (Uow.Repository<User>().GetQueryable().Any(x => x.Login == userParam.Login))
                    throw new Exception("Username " + userParam.Login + " is already taken");
            }
            if (userParam.EMail != user.EMail)
            {
                if (Uow.Repository<User>().GetQueryable().Any(x => x.Login == userParam.Login))
                    throw new Exception("eMail " + userParam.EMail + " is already taken");
            }
            
            if (!string.IsNullOrWhiteSpace(password))
            {
                PasswordHash ph = await Hasher.CreatePasswordHash(password);

                userParam.PasswordHash = ph.Hash;
                userParam.PasswordSalt = ph.Salt;
            }
            Uow.Repository<User>().Update(userParam);
            await Uow.SaveChangesAsync();
        }
        public async Task<User> Get(long id)
        {
            return await Uow.Repository<User>().Get(id);
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
            var user = await Uow.Repository<User>().GetQueryable().Where(x=>x.Id==userId)
                .Include(u => u.Rates)
                .ThenInclude(rate => rate.Movie)
                .ThenInclude(movie=>movie.Rates)
                .Include(u=>u.WatchLaterList)
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
            var watchList = user.WatchLaterList.Select(x => new MovieListViewModel()
            {
                Id = x.MovieId,
                Title = x.Movie.Title,
                Year = x.Movie.Year,
                AverageRate = x.Movie.Rates.Any() ? (x.Movie.Rates.Sum(m => m.Value) / x.Movie.Rates.Count()) : 0,
                UsersRate = (user.Rates.Any()) ? (user.Rates.FirstOrDefault(rate => rate.ProfileId == user.Id).Value) : 0,
                ImageUrl = x.Movie.ImageUrl
            });
            return new UserPageViewModel() { FullName = user.FullName, Rates = rates, WatchLaterMovies= watchList };

        }
        public async Task RestorePassword(string newPassword, string hashParam)
        { 
            User user = await Uow.Repository<User>().GetQueryable()
                .FirstOrDefaultAsync(x => Hasher.ByteToString(x.PasswordHash) == hashParam);

            PasswordHash ph = await Hasher.CreatePasswordHash(newPassword);

            user.PasswordHash = ph.Hash;
            user.PasswordSalt = ph.Salt;

            await Uow.SaveChangesAsync();
        }
        public async Task ForgotPassword(string email, NotificationServiceType type)
        {
            switch (type)
            {
                case NotificationServiceType.Email:

                    User user = await Uow.Repository<User>().GetQueryable().FirstAsync(x => x.EMail == email);

                    if (user == null)
                    {
                        throw new Exception($"User with {email} email not found");
                    }

                    string hostUrl = "http://localhost:4200";//httpAccessor.HttpContext.Request.Host.ToString();
                    string url =  
                        $"{hostUrl}/restoring-password/{Hasher.ByteToString(user.PasswordHash)}";

                    var message = new NotificationMessage
                    {
                        Title = "Restoring password",
                        To = email,
                        Body = $"Click a link to restore password: {url}",
                    };
                    NotificationServiceFactory factory = new NotificationServiceFactory();
                    await factory.CreateService(type).Send(message);
                        
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
