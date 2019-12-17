using System;
using System.ComponentModel.DataAnnotations;

namespace Visib.Api.ViewModels
{
    public class PostCommentViewModel
    {
        [Key] public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public string AppUsername { get; set; }

        public Guid PostId { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
    }
}