using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class ConfigurationViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;
        private readonly IUserInteractionService userInteractionService;
        private bool isLoading;

        public ConfigurationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService, IUserInteractionService userInteractionService) : base(logProvider, navigationService)
        {
            this.loginService = loginService;
            this.userInteractionService = userInteractionService;
        }

        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public MvxAsyncCommand GoToEditProfileCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<EditProfileViewModel>());
        public MvxAsyncCommand GoToInviteFriendsCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<InviteFriendsViewModel>());
        public MvxAsyncCommand GoToUseTermsCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<UseTermsViewModel>());
        public MvxAsyncCommand GoToPrivacyPolicyCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<PrivacyPolicyViewModel>());
        public MvxAsyncCommand GoToChangePasswordCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<ChangePasswordViewModel>());
        public MvxAsyncCommand LogoutCommand => new MvxAsyncCommand(async () =>
        {
            loginService.Logout();
            await NavigationService.Navigate<WelcomeViewModel>();
        });

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public MvxAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(async () =>
        {
            await userInteractionService.DisplayConfirmation("Remover", "Tem certeja que deseja remover completamente os seus dados?", "Sim", "Não", async result =>
             {
                 if (result)
                 {
                     IsLoading = true;
                     await loginService.Delete();
                     IsLoading = false;
                     await NavigationService.Navigate<WelcomeViewModel>();
                 }
             });

        });
    }
}
