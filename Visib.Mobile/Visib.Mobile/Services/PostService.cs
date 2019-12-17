using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Polly;
using Refit;
using Visib.Mobile.Services.Requests;
using Visib.Mobile.Services.Responses;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.Services
{
    public class PostService : IPostService
    {
        private readonly IApiService apiService;
        private readonly IMediaService mediaService;

        public PostService(IApiService apiService, IMediaService mediaService)
        {
            this.apiService = apiService;
            this.mediaService = mediaService;
        }

        public async Task<bool> CreateComment(CommentRequest request)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = apiService.UserInitiated.CreateComment(request).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return true;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> DeleteComment(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = apiService.UserInitiated.DeleteComment(postId).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return true;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }


        public async Task<IEnumerable<CommentRequest>> GetComments(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = apiService.UserInitiated.GetComments(postId).ConfigureAwait(false);
            try
            {
                var posts = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return posts;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<IEnumerable<PostModel>> GetPosts(FilterType filter, int page)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = apiService.UserInitiated.GetPosts(filter, page).ConfigureAwait(false);
            try
            {
                var posts = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                var response = posts.Select(n => new PostModel
                {
                    Title = n.Title,
                    Description = n.Description,
                    Id = n.Id,
                    UserId = n.UserId,
                    Username = n.Username,
                    CommentCount = n.CommentCount,
                    UserPicture = $"https://s3.us-east-2.amazonaws.com/visib/{n.UserId}.png",
                    HasUserApplause = n.IsLikedByUser,
                    ApplauseCount = n.LikeCount,
                    ViewCount = n.ViewCount,
                    Category = n.CategoryName,
                    CategoryId = n.CategoryId,
                    VideoSource = $"https://s3.us-east-2.amazonaws.com/visib/{n.Id}.mp4",
                    VideoCover = $"https://s3.us-east-2.amazonaws.com/visib/{n.Id}.jpg"
                });
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<IEnumerable<PostModel>> GetPostsForUser(string userId)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = apiService.UserInitiated.GetPostsForUser(userId).ConfigureAwait(false);
            try
            {
                var posts = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                var response = posts.Select(n => new PostModel
                {
                    Title = n.Title,
                    Description = n.Description,
                    Id = n.Id,
                    UserId = n.UserId,
                    CommentCount = n.CommentCount,
                    Username = n.Username,
                    UserPicture = $"https://s3.us-east-2.amazonaws.com/visib/{n.UserId}.png",
                    HasUserApplause = n.IsLikedByUser,
                    ApplauseCount = n.LikeCount,
                    ViewCount = n.ViewCount,
                    Category = n.CategoryName,
                    CategoryId = n.CategoryId,
                    VideoSource = $"https://s3.us-east-2.amazonaws.com/visib/{n.Id}.mp4",
                    VideoCover = $"https://s3.us-east-2.amazonaws.com/visib/{n.Id}.jpg"
                });
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> Like(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = apiService.UserInitiated.LikePost(postId).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return true;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> Share(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = apiService.UserInitiated.SharePost(postId).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return true;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<IEnumerable<PostModel>> Search(string searchTerm, int page)
        {

            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = apiService.UserInitiated.SearchPosts(new SearchRequest { Term = searchTerm }, page).ConfigureAwait(false);
            try
            {
                var posts = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                var response = posts.Select(n => new PostModel
                {
                    Title = n.Title,
                    Description = n.Description,
                    CommentCount = n.CommentCount,
                    Id = n.Id,
                    UserId = n.UserId,
                    Username = n.Username,
                    UserPicture = $"https://s3.us-east-2.amazonaws.com/visib/{n.UserId}.png",
                    HasUserApplause = n.IsLikedByUser,
                    ApplauseCount = n.LikeCount,
                    ViewCount = n.ViewCount,
                    Category = n.CategoryName,
                    CategoryId = n.CategoryId,
                    VideoSource = $"https://s3.us-east-2.amazonaws.com/visib/{n.Id}.mp4",
                    VideoCover = $"https://s3.us-east-2.amazonaws.com/visib/{n.Id}.jpg"
                });
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> Publish(PostModel post, byte[] video)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            await mediaService.Upload(video, $"{post.Id}.mp4");
            var postTask = apiService.UserInitiated.PublishPost(new PostRequest
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Tags = string.Join(" ", post.Tags),
                CategoryId = post.CategoryId
            }).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await postTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<CategoryModel>> GetCategories()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = apiService.UserInitiated.GetCategories().ConfigureAwait(false);
            try
            {
                var categories = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                var response = categories.Select(n => new CategoryModel
                {
                    Id = n.Id,
                    Name = n.Name
                });
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<IEnumerable<string>> GetTags()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = apiService.UserInitiated.GetTags().ConfigureAwait(false);
            try
            {
                var tags = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return tags;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> View(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = apiService.UserInitiated.View(postId).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return true;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<IEnumerable<NotificationModel>> GetNotifications()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var likesTask = apiService.UserInitiated.GetLikes().ConfigureAwait(false);
            try
            {
                var likes = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await likesTask);
                var notifications = new List<NotificationModel>();

                var posts = likes.OrderByDescending(n=>n.When).Select(n => n.PostId).Distinct();
                foreach (var post in posts)
                {
                    var notification = new NotificationModel();
                    var likesPerPost = likes.Where(n => n.PostId == post).OrderByDescending(n=>n.When);
                    if(likesPerPost.Count() == 1)
                    {
                        var like = likesPerPost.Single();
                        notification.LastChange = like.When;
                        notification.User1Id = like.AppUserId;
                        notification.Text = $"{like.Username} aplaudiu o seu post {like.PostTitle}";
                    } else if (likesPerPost.Count() == 2)
                    {
                        var like1 = likesPerPost.First();
                        var like2 = likesPerPost.Last();
                        notification.LastChange = like1.When;
                        notification.User1Id = like1.AppUserId;
                        notification.User2Id = like2.AppUserId;
                        notification.Text = $"{like1.Username}, {like2.Username} aplaudiram o seu post {like1.PostTitle}";
                    } else
                    {
                        var like1 = likesPerPost.ElementAt(0);
                        var like2 = likesPerPost.ElementAt(1);
                        notification.LastChange = like1.When;
                        notification.User1Id = like1.AppUserId;
                        notification.User2Id = like2.AppUserId;
                        notification.Text = $"{like1.Username}, {like2.Username} e outras {likesPerPost.Count() - 2} pessoa(s) aplaudiram o seu post {like1.PostTitle}";
                    }
                    notifications.Add(notification);
                }
                return notifications;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
