using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using System.Linq;

namespace MyIMDB.Services.MapperProfiles
{
    public class ReviewToReviewApiConverter : ITypeConverter<Review, ReviewApiModel>
    {
        public ReviewApiModel Convert(Review source, ReviewApiModel destination, ResolutionContext context)
        {
            destination = new ReviewApiModel
            {
                Id = source.Id,
                UserId = source.UserId,
                UserName = source.User.UserName,
                MovieId = source.MovieId,
                Added = source.Added.ToString(),
                LikesCount = source.LikesCount,
                DislikesCount = source.DislikesCount,
                Text = source.Text
            };
            context.Items.TryGetValue("userId", out object userId);

            if (userId == null)
                return destination;

            var like = source.Likes.FirstOrDefault(x => x.UserId == (long)userId);

            destination.LikedByCurrentUser = like.Liked;

            return destination;

        }
    }
}
