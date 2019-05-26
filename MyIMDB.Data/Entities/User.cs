using System;
using System.Collections.Generic;
using MyIMDB.Interfaces;

namespace MyIMDB.Data.Entities
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string EMail { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }

        public long? GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        public long? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual IEnumerable<WatchLaterMovies> WatchLaterList { get; set; }
        public virtual IEnumerable<Rate> Rates { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
