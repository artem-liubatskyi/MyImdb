using System;
using System.Collections.Generic;
using System.Text;

namespace MyIMDB.ApiModels.Models
{
    public class ReviewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime Added { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
    }
}
