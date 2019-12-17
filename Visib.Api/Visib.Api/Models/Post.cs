using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Visib.Api.Models
{
    public class Post
    {
        [Key] public Guid Id { get; set; }
        public AppUser AppUser { get; set; }
        public int LikeCount { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public int ViewCount { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsPaused { get; set; }
        public string AppUserId { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public int CommentCount { get; set; }
    }


    public class PostLike
    {
        [Key] public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime When { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }

    public class PostComment
    {
        [Key] public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string Description { get; set; }

        public DateTime PublishDate { get; set; }
    }
}