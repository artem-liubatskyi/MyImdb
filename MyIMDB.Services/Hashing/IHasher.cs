using System.Threading.Tasks;

namespace MyIMDB.Services.Hashing
{
    public interface IHasher
    {
        Task<PasswordHash> CreatePasswordHash(string password);
        Task<bool> VerifyPasswordHash(string password, PasswordHash hash);
        string ByteToString(byte[] a);
    }
}
