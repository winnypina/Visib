using System.Collections.Generic;
using System.Threading.Tasks;
using Visib.Mobile.Services.Responses;

namespace Visib.Mobile.Services
{
    public interface ILoginService
    {

        Task<AccountResponse> GetProfile(string id);
        string AccessToken { get; set; }
        AccountResponse Account { get; set; }
        Task<bool> Login(string username, string password);

        Task<bool> Follow(string id);

        Task<IEnumerable<AccountResponse>> GetFollows(string userId);
        Task<IEnumerable<AccountResponse>> GetFollowers(string userId);

        Task<bool> VerifyCode(string login, int code);

        Task<bool> ResendCode(string login);

        Task<bool> ChangePassword(string currentPassword, string newPassword);

        Task<bool> Signup(string name, string login, string password, string picture);
        Task<string> GetUsageTerms();
        Task<bool> LoginWithFacebook(string accessToken);
        Task<bool> UpdateAccount(AccountResponse account);
        bool LoadCachedUser();
        void Logout();
        Task<bool> Delete();
        Task<bool> ResetPassword(string username);
    }
}