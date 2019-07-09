using MyIMDB.Data.Abstraction;
using System;

namespace MyIMDB.Data.Entities
{
    public class RefreshToken : IEntity
    {
        public long Id { get; set; }
        public string Token { get; private set; }
        public DateTime Expires { get; private set; }
        public long UserId { get; private set; }
        public virtual User User { get; set; }
        public bool Active => DateTime.UtcNow <= Expires;
        public string RemoteIpAddress { get; private set; }

        public RefreshToken(string token, DateTime expires, long userId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
