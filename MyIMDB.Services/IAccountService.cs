using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Services.Helpers;

namespace MyIMDB.Services
{
    public interface IAccountService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        Task Update(User userParam, string password = null);
        Task<User> Get(long id);
        Task<RegisterDataModel> GetRegistrationData();
        Task<UserPageViewModel> GetUserPageModel(long userId);
        Task ForgotPassword(string email, NotificationServiceType type);
        Task RestorePassword(string newPassword, string hash);
    }
}
