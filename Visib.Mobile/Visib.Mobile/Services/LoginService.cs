using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MonkeyCache.SQLite;
using Plugin.Connectivity;
using Polly;
using Refit;
using Visib.Mobile.Services.Requests;
using Visib.Mobile.Services.Responses;

namespace Visib.Mobile.Services
{
    public class LoginService : ILoginService
    {
        private readonly IApiService _apiService;

        public LoginService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public string AccessToken { get; set; }

        public AccountResponse Account { get; set; }

        public async Task<bool> Login(string username, string password)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var loginTask = _apiService.UserInitiated.Login(new AuthTokenRequest { Password = password, UserName = username}).ConfigureAwait(false);
            try
            {
                var loginResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await loginTask);
                if (loginResponse.AccessToken == null) return false;
                AccessToken = loginResponse.AccessToken;
                var cache = Barrel.Current;
                cache.Add("token", AccessToken, TimeSpan.FromDays(30));
                var accountTask = _apiService.UserInitiated.GetAccount().ConfigureAwait(false);
                var accountResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await accountTask);
                Account = accountResponse;
                
                cache.Add("account", Account, TimeSpan.FromDays(30));
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;

        }

        public async Task<bool> Follow(string id)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.Follow(id).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

      

        public async Task<IEnumerable<AccountResponse>> GetFollows(string userId)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetFollows(userId).ConfigureAwait(false);
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

        public async Task<IEnumerable<AccountResponse>> GetFollowers(string userId)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetFollowers(userId).ConfigureAwait(false);
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

        public async Task<AccountResponse> GetProfile(string id)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            try
            {
                var accountTask = _apiService.UserInitiated.GetProfile(id).ConfigureAwait(false);
                var accountResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await accountTask);
                if(Account.Id == id)
                {
                    Account = accountResponse;
                    var cache = Barrel.Current;
                    cache.Add("account", Account, TimeSpan.FromDays(30));
                }
                return accountResponse;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;

        }

        public async Task<bool> LoginWithFacebook(string accessToken)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var loginTask = _apiService.UserInitiated.LoginWithFacebook(new FacebookAuthTokenRequest { AccessToken = accessToken}).ConfigureAwait(false);
            try
            {
                var loginResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await loginTask);
                if (loginResponse.AccessToken == null) return false;
                AccessToken = loginResponse.AccessToken;
                var cache = Barrel.Current;
                cache.Add("token", AccessToken, TimeSpan.FromDays(30));
                var accountTask = _apiService.UserInitiated.GetAccount().ConfigureAwait(false);
                var accountResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await accountTask);
                Account = accountResponse;
                
                cache.Add("account", Account, TimeSpan.FromDays(30));
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;

        }

        public async Task<bool> UpdateAccount(AccountResponse updatedAccount)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            
            var signupTask = _apiService.UserInitiated.UpdateAccount(updatedAccount).ConfigureAwait(false);
            try
            {
                var account = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                Account = account;
                var cache = Barrel.Current;
                cache.Add("account", Account, TimeSpan.FromDays(30));
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public bool LoadCachedUser()
        {
            var cache = Barrel.Current;

            var keys = cache.GetKeys().ToList();

            if (keys.Contains("account") && keys.Contains("token"))
            {
                AccessToken = cache.Get<string>("token");
                Account = cache.Get<AccountResponse>("account");
                return true;
            }

            return false;
        }

        public void Logout()
        {
            Account = null;
            AccessToken = null;
            var cache = Barrel.Current;
            cache.EmptyAll();
        }

        public async Task<bool> Signup(string name, string login, string password, string picture)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var emailLogin = login.Contains("@") ? login : "no@email.com";
            var phoneLogin = !login.Contains("@") ? login : null;

            var signupTask = _apiService.UserInitiated.CreateAccount(new CreateAccountRequest {CpfCnpj = null, Picture = picture, Name = name, Password = password, Email = emailLogin, BirthDate = null,Address = null,MobilePhone = phoneLogin}).ConfigureAwait(false);
            try
            {
                var account = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                Account = account;
                var cache = Barrel.Current;
                cache.Add("account", Account, TimeSpan.FromDays(30));
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> VerifyCode(string login,int code)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.VerifyCode(new CodeVerificationRequest() { UserName = login, Code = code}).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> ResendCode(string login)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.ResendCode(new CodeVerificationRequest() { UserName = login }).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> ResetPassword(string login)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.ResetPassword(new ResetPasswordRequest { Login = login }).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> ChangePassword(string currentPassword, string newPassword)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.ChangePassword(new ChangePasswordRequest() { OldPassword = currentPassword, NewPassword = newPassword }).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }


        public async Task<string> GetUsageTerms()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var usTermTask = _apiService.UserInitiated.GetUseTerms().ConfigureAwait(false);
            try
            {
                var useTermResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await usTermTask);
                return useTermResponse.Text;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> Delete()
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.DeleteAccount().ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}