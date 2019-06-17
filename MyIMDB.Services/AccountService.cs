using System;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyIMDB.Services.Hashing;
using MyIMDB.Services.Helpers;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using MyIMDB.DataAccess.Interfaces;

namespace MyIMDB.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork Uow;
        private readonly IHasher Hasher;
        private readonly IHttpContextAccessor httpAccessor;
        private readonly IMapper mapper;

        public AccountService(IUnitOfWork uow, IHasher hasher, IHttpContextAccessor httpAccessor, IMapper mapper)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
            Hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            this.httpAccessor = httpAccessor ?? throw new ArgumentNullException(nameof(httpAccessor));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        public async Task<User> Create(RegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new Exception("Password is required");

            if (await Uow.Repository<User>().GetQueryable().AnyAsync(x => x.Login == model.Login))
                throw new Exception("Username \"" + model.Login + "\" is already taken");

            if (await Uow.Repository<User>().GetQueryable().AnyAsync(x => x.EMail == model.Email))
                throw new Exception("Email \"" + model.Email + "\" is already taken");

            var user = mapper.Map<RegisterModel, User>(model);

            PasswordHash ph = await Hasher.CreatePasswordHash(model.Password);

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
            var genders = mapper.Map<Gender[],GenderModel[]>(await Uow.Repository<Gender>().GetQueryable().ToArrayAsync());

            var countries = mapper.Map<Country[],CountryModel[]>(await Uow.Repository<Country>().GetQueryable().ToArrayAsync());

            return new RegisterDataModel() { Genders = genders, Countries = countries };
        }
        public async Task<UserPageViewModel> GetUserPageModel(long userId)
        {
            var user = await Uow.Repository<User>().GetQueryable().Where(x=>x.Id==userId)
                .Include(u => u.Movies)
                    .ThenInclude(rate => rate.Movie)
                        .ThenInclude(movie=>movie.UserMovies)
                .FirstOrDefaultAsync();

           return mapper.Map<User, UserPageViewModel>(user);
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
