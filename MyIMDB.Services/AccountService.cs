using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess;
using MyIMDB.DataAccess.Interfaces;
using MyIMDB.Services.Hashing;
using MyIMDB.Services.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyIMDB.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork Uow;
        private readonly IHasher Hasher;
        private readonly IHttpContextAccessor httpAccessor;
        private readonly IMapper mapper;
        private readonly JwtIssuerOptions options;

        public AccountService(IUnitOfWork uow, IHasher hasher, IHttpContextAccessor httpAccessor, IMapper mapper, IOptions<JwtIssuerOptions> options)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
            Hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            this.httpAccessor = httpAccessor ?? throw new ArgumentNullException(nameof(httpAccessor));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await Uow.UserRepository.GetByUsername(username);

            if (user == null)
                return null;

            if (await Hasher.VerifyPasswordHash(password, new PasswordHash(user.PasswordHash, user.PasswordSalt)))
                return user;

            return null;
        }
        public async Task<User> Create(RegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new Exception("Password is required");

            if (await Uow.UserRepository.GetByUsername(model.UserName) != null)
                throw new Exception("Username \"" + model.UserName + "\" is already taken");

            if (await Uow.UserRepository.GetByEmail(model.Email) != null)
                throw new Exception("Email \"" + model.Email + "\" is already taken");

            var user = mapper.Map<RegisterModel, User>(model);

            user.RoleId = Uow.RolesRepository.GetQueryable().FirstOrDefault(x => x.Name == Constants.UserRole).Id;

            PasswordHash ph = await Hasher.CreatePasswordHash(model.Password);

            user.PasswordHash = ph.Hash;
            user.PasswordSalt = ph.Salt;

            await Uow.UserRepository.Add(user);
            await Uow.SaveChangesAsync();

            return user;
        }
        public async Task Update(User userParam, string password = null)
        {
            var user = await Uow.UserRepository.Get(userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            if (userParam.UserName != user.UserName)
            {
                if (await Uow.UserRepository.GetByUsername(userParam.UserName) != null)
                    throw new Exception("Username " + userParam.UserName + " is already taken");
            }
            if (userParam.Email != user.Email)
            {
                if (await Uow.UserRepository.GetByEmail(userParam.Email) != null)
                    throw new Exception("eMail " + userParam.Email + " is already taken");
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                PasswordHash ph = await Hasher.CreatePasswordHash(password);

                userParam.PasswordHash = ph.Hash;
                userParam.PasswordSalt = ph.Salt;
            }
            Uow.UserRepository.Update(userParam);
            await Uow.SaveChangesAsync();
        }
        public async Task<User> Get(long id)
        {
            return await Uow.UserRepository.GetById(id);
        }
        public async Task<RegisterDataModel> GetRegistrationData()
        {
            var genders = mapper.Map<Gender[], GenderModel[]>(await Uow.GenderRepository.GetQueryable().ToArrayAsync());

            var countries = mapper.Map<Country[], CountryModel[]>(await Uow.CountryRepository.GetQueryable().ToArrayAsync());

            return new RegisterDataModel() { Genders = genders, Countries = countries };
        }
        public async Task<UserPageViewModel> GetUserPageModel(long userId)
        {
            var user = await Uow.UserRepository.GetForUserPage(userId);

            return mapper.Map<User, UserPageViewModel>(user);
        }
        public async Task RestorePassword(string newPassword, string hashParam)
        {
            User user = await Uow.UserRepository.GetQueryable()
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

                    User user = await Uow.UserRepository.GetByEmail(email);

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
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.SigningCredentials.Key,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        public async Task SetRefreshToken(User user, RefreshToken token)
        {
            user.Token = token;
            await Uow.SaveChangesAsync();
        }
    }
}
