using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Xamarin.Auth;

namespace Visib.Mobile.ViewModels
{
    public class WelcomeViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;
        private bool isLoading;

        public WelcomeViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService) : base(logProvider, navigationService)
        {
            this.loginService = loginService;
        }

        public MvxAsyncCommand LoginWithFacebookCommand => new MvxAsyncCommand(ExecuteLoginWithFacebookCommand);

        public MvxAsyncCommand LoginWithInstagramCommand => new MvxAsyncCommand(ExecuteLoginWithInstagramCommand);

        public MvxAsyncCommand SignupCommand => new MvxAsyncCommand(ExecuteSignupCommand);

        public MvxAsyncCommand SigninCommand => new MvxAsyncCommand(ExecuteSigninCommand);

        private async Task ExecuteSigninCommand()
        {
            await NavigationService.Navigate<SigninViewModel>();
        }

        public MvxAsyncCommand ForgotPasswordCommand => new MvxAsyncCommand(ExecuteForgotPasswordCommand);

        private async Task ExecuteForgotPasswordCommand()
        {
            await NavigationService.Navigate<ForgotPasswordViewModel>();
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        private async Task ExecuteSignupCommand()
        {
            await NavigationService.Navigate<SignupViewModel>();
        }

        public override async Task Initialize()
        {
            if(loginService.LoadCachedUser())
            {
                await NavigationService.Navigate<TabsMasterViewModel>();
            }
        }

        private Task ExecuteLoginWithInstagramCommand()
        {
            IsLoading = true;
            var auth = new OAuth2Authenticator(
                "24a92b732f1a4b2a92fd750b5c83d78d",
                "basic public_content likes",
                new Uri("https://api.instagram.com/oauth/authorize"),
                new Uri("http://www.visib.com"));

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Completed += async (sending, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    var token = eventArgs.Account.Properties["access_token"];
                }
                else
                {
                    IsLoading = false;
                }
            };
            presenter.Login(auth);
            return Task.CompletedTask;
        }

        private Task ExecuteLoginWithFacebookCommand()
        {
            IsLoading = true;
            var auth = new OAuth2Authenticator(
                "235193597371570",
                "email",
                new Uri("https://m.facebook.com/dialog/oauth/"),
                new Uri("https://www.facebook.com/connect/login_success.html"));

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Completed += async (sending, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {

                    if(await loginService.LoginWithFacebook(eventArgs.Account.Properties["access_token"])
                        .ConfigureAwait(false))
                    {
                        await NavigationService.Navigate<TabsMasterViewModel>();
                    }
                    IsLoading = false;
                }
                else
                {
                    IsLoading = false;
                }
            };
            presenter.Login(auth);
            return Task.CompletedTask;
        }
    }
}
