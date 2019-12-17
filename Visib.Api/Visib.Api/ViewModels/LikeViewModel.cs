using System;

namespace Visib.Api.ViewModels
{
    public class LikeViewModel
    {
        public string AppUserId { get; set; }
        public string Username { get; set; }
        public DateTime When { get; set; }
        public Guid PostId { get; set; }
        public string PostTitle { get; set; }
    }
}
