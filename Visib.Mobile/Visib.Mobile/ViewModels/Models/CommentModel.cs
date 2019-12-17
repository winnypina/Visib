using Visib.Mobile.Services.Requests;

namespace Visib.Mobile.ViewModels.Models
{
    public class CommentModel : CommentRequest
    {
        public string Since { get; set; }
        public string UserPicture { get; set; }
    }
}
