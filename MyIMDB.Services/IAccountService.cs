using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services
{
    public interface IAccountService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        void Update(User userParam, string password = null);
        void Delete(int id);
        Task<User> Get(long id);
        Task<RegisterDataModel> GetRegistrationData();
        Task<UserPageViewModel> GetUserPageModel(long userId);
    }
}
