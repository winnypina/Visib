using System;

namespace Visib.Mobile.Services.Requests
{
    public class CommentRequest
    {
        public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public string AppUsername { get; set; }
        public Guid PostId { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
    }
}