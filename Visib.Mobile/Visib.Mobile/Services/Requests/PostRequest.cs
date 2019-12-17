using System;
namespace Visib.Mobile.Services.Requests
{
    public class PostRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public Guid CategoryId { get; internal set; }
    }
}
