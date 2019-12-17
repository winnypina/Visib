using System;
namespace Visib.Mobile.Services.Responses
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public bool IsLikedByUser { get; set; }
        public int LikeCount { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
    }
}
