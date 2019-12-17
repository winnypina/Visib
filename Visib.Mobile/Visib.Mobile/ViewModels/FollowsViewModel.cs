using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Visib.Mobile.Services.Responses;

namespace Visib.Mobile.ViewModels
{
    public class FollowsViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;
        private bool isLoading;

        public FollowsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService) : base(logProvider, navigationService)
        {
            this.loginService = loginService;
            Follows = new MvxObservableCollection<AccountResponse>();
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public MvxObservableCollection<AccountResponse> Follows { get; }

        public async override Task Initialize()
        {
            var follows = await loginService.GetFollows(loginService.Account.Id);
            if (Follows.Count > 0)
            {
                Follows.RemoveRange(0, Follows.Count);
            }
            if (follows != null)
            {
                Follows.AddRange(follows);
            }
        }

        public MvxAsyncCommand<AccountResponse> UnfollowCommand => new MvxAsyncCommand<AccountResponse>(async account =>
        {
            loginService.Account.Follows--;
            account.IsFollowing = false;
            Follows.Remove(account);
            await loginService.Follow(account.Id).ConfigureAwait(false);
        });

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

    }
}
