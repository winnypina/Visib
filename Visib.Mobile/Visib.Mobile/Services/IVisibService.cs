using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Visib.Mobile.Services.Requests;
using Visib.Mobile.Services.Responses;

namespace Visib.Mobile.Services
{
    public interface IVisibService
    {
        [Post("/auth/login")]
        Task<LoginResponse> Login([Body] AuthTokenRequest request);

        [Post("/externalauth/facebook")]
        Task<LoginResponse> LoginWithFacebook([Body] FacebookAuthTokenRequest request);

        [Get("/accounts")]
        Task<AccountResponse> GetAccount();

        [Get("/accounts/{id}")]
        Task<AccountResponse> GetProfile(string id);

        [Get("/category")]
        Task<IEnumerable<CategoryResponse>> GetCategories();

        [Get("/post/tags")]
        Task<IEnumerable<string>> GetTags();

        [Get("/UseTerms")]
        Task<UseTermsResponse> GetUseTerms();

        [Get("/post/paged/{page}/{filterType}")]
        Task<IEnumerable<PostResponse>> GetPosts(FilterType filterType,int page);


        [Get("/PrivacyPolicy")]
        Task<UseTermsResponse> GetPrivacyPolicy();

        [Post("/accounts")]
        Task<AccountResponse> CreateAccount([Body] CreateAccountRequest request);

        [Post("/accounts/verify")]
        Task VerifyCode([Body] CodeVerificationRequest request);

        [Post("/post")]
        Task PublishPost([Body] PostRequest request);

        [Post("/post/share/{postId}")]
        Task SharePost(Guid postId);

        [Post("/accounts/resend")]
        Task ResendCode([Body] CodeVerificationRequest request);

        [Post("/accounts/reset")]
        Task ResetPassword([Body] ResetPasswordRequest request);

        [Post("/accounts/password")]
        Task ChangePassword([Body] ChangePasswordRequest request);

        [Put("/accounts")]
        Task<AccountResponse> UpdateAccount(AccountResponse request);
        
        [Multipart]
        [Post("/media")]
        Task UploadMedia([AliasAs("files")] IEnumerable<StreamPart> streams);

        [Post("/post/like/{id}")]
        Task LikePost(Guid id);

        [Post("/post/view/{id}")]
        Task View(Guid id);

        [Get("/TextTranslation")]
        Task<IEnumerable<TranslationResponse>> GetTranslation();

        [Get("/post/comment/{id}")]
        Task<IEnumerable<CommentRequest>> GetComments(Guid id);

        [Post("/post/comment")]
        Task CreateComment(CommentRequest request);

        [Post("/post/comment/{id}")]
        Task DeleteComment(Guid id);

        [Get("/accounts/followers/{userId}")]
        Task<IEnumerable<AccountResponse>> GetFollowers(string userid);

        [Get("/accounts/follows/{userId}")]
        Task<IEnumerable<AccountResponse>> GetFollows(string userId);

        [Post("/accounts/follow/{id}")]
        Task Follow(string id);

        [Get("/post/user/{userId}")]
        Task<IEnumerable<PostResponse>> GetPostsForUser(string userId);

        [Delete("/accounts")]
        Task DeleteAccount();

        [Post("/post/search/paged/{page}")]
        Task<IEnumerable<PostResponse>> SearchPosts([Body] SearchRequest searchTerm, int page);

        [Get("/post/like")]
        Task<IEnumerable<LikeResponse>> GetLikes();
    }
}