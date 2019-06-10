using System;
using System.Text;
using System.Threading.Tasks;

namespace MyIMDB.Services.Hashing
{
    public class Hasher : IHasher
    {
        public  async Task<PasswordHash>CreatePasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            PasswordHash ph = new PasswordHash();

            await Task.Run(() =>
             {
                 using (var hmac = new System.Security.Cryptography.HMACSHA512())
                 {
                     ph.Salt = hmac.Key;
                     ph.Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                 }
             });

            return ph;
        }

        public async Task<bool> VerifyPasswordHash(string password, PasswordHash hash)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            bool verify = true;
            await Task.Run(() =>
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512(hash.Salt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != hash.Hash[i])
                        {
                            verify = false;
                            return;
                        }
                    }
                }
            });
            return verify;
        }
        public string ByteToString(byte[] a)
        {
            StringBuilder builder = new StringBuilder();
           
            for (int i = 0; i < a.Length; i++)
                builder.Append(a[i].ToString("X2"));
            return builder.ToString();
        }
    }
}
