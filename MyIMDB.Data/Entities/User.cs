using MyIMDB.Data.Abstraction;
using System;
using System.Collections.Generic;

namespace MyIMDB.Data.Entities
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }

        public long RoleId { get; set; }
        public Role Role { get; set; }

        public long TokenId { get; set; }
        public RefreshToken Token { get; set; }

        public long? GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        public long? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual IEnumerable<UserMovie> Movies { get; set; }
        public virtual IEnumerable<Review> Reviews { get; set; }
        public virtual IEnumerable<Like> Likes { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
