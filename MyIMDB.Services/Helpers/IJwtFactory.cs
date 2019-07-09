using MyIMDB.Data.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyIMDB.Services.Helpers
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(User user);
        RefreshToken GenerateRefreshToken(long userId, string remoteIpAddress, int size = 32, double daysToExpire = 5);
    }
}
