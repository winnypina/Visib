using System;

namespace Visib.Mobile.Services.Responses
{
    public class LikeResponse
    {
        public string AppUserId { get; set; }
        public string Username { get; set; }
        public DateTime When { get; set; }
        public Guid PostId { get; set; }
        public string PostTitle { get; set; }
    }
}
