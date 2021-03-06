﻿using MyIMDB.Data.Abstraction;
using System;
using System.Collections.Generic;

namespace MyIMDB.Data.Entities
{
    public class Review : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long MovieId { get; set; }
        public Movie Movie { get; set; }
        public string Text { get; set; }
        public DateTime Added { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public virtual IEnumerable<Like> Likes { get; set; }
    }
}
