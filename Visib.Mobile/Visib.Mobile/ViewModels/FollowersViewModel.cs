using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Visib.Mobile.Services.Responses;

namespace Visib.Mobile.ViewModels
{
    public class FollowersViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;
        private bool isLoading;

        public FollowersViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService) : base(logProvider, navigationService)
        {
            this.loginService = loginService;
            Followers = new MvxObservableCollection<AccountResponse>();
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading,value); }

        public MvxObservableCollection<AccountResponse> Followers { get; }

        public async override Task Initialize()
        {
            var followers = await loginService.GetFollowers(loginService.Account.Id);
            if (Followers.Count > 0)
            {
                Followers.RemoveRange(0, Followers.Count);
            }
            if (followers != null)
            {
                Followers.AddRange(followers);
            }
        }

        public MvxAsyncCommand<AccountResponse> FollowCommand => new MvxAsyncCommand<AccountResponse>(async account =>
        {
            loginService.Account.Follows++;
            account.IsFollowing = true;
            await loginService.Follow(account.Id).ConfigureAwait(false);
        });

        public MvxAsyncCommand<AccountResponse> UnfollowCommand => new MvxAsyncCommand<AccountResponse>(async account =>
        {
            loginService.Account.Follows--;
            account.IsFollowing = false;
            await loginService.Follow(account.Id).ConfigureAwait(false);
        });

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

    }
}
