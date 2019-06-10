using System;

namespace MyIMDB.Services.Hashing
{
    public class PasswordHash
    {
        public PasswordHash()
        { }
        public PasswordHash(byte[] hash, byte[] salt)
        {
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            Salt = salt ?? throw new ArgumentNullException(nameof(salt));
        }

        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
