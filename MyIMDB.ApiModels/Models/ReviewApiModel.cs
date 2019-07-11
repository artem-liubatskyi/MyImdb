using System;
using System.Collections.Generic;
using System.Text;

namespace MyIMDB.ApiModels.Models
{
    public class ReviewApiModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long MovieId { get; set; }
        public string Text { get; set; }
        public string Added { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public bool? LikedByCurrentUser { get; set; }
    }
}
