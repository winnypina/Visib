using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visib.Mobile.Services.Requests;
using Visib.Mobile.Services.Responses;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.Services
{
    public enum FilterType 
    {
        Recent = 1,
        Viewed = 2,
        Applauded = 3
    }

    public interface IPostService
    {
        Task<IEnumerable<PostModel>> GetPostsForUser(string userId);
        Task<bool> View(Guid postId);
        Task<bool> Like(Guid postId);
        Task<bool> Share(Guid postId);
        Task<IEnumerable<string>> GetTags();
        Task<IEnumerable<NotificationModel>> GetNotifications();
        Task<IEnumerable<CategoryModel>> GetCategories();
        Task<IEnumerable<PostModel>> GetPosts(FilterType filter, int page);
        Task<IEnumerable<PostModel>> Search(string searchTerm, int page);
        Task<bool> Publish(PostModel post, byte[] video);
        Task<IEnumerable<CommentRequest>> GetComments(Guid id);
        Task<bool> CreateComment(CommentRequest request);
        Task<bool> DeleteComment(Guid id);
    }
}
