using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class ForgotPasswordViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;
        private readonly IUserInteractionService userInteractionService;
        private string username;
        private bool isLoading;

        public ForgotPasswordViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService, IUserInteractionService userInteractionService) : base(
logProvider, navigationService)
        {
            this.loginService = loginService;
            this.userInteractionService = userInteractionService;
        }

        public string Username { get => username; set => SetProperty(ref username, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading,value); }

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            if (!string.IsNullOrEmpty(Username))
            {
                if (await loginService.ResetPassword(Username))
                {
                    await userInteractionService.DisplayMessage("Nova senha", "Uma nova senha foi enviada");
                    await NavigationService.Navigate<WelcomeViewModel>();
                }
                else
                {
                    await userInteractionService.DisplayMessage("Usuário não encontrado", "Não encontramos um usuário com essas credenciais, por favor tente novamente");
                }
            }
            IsLoading = false;
        });
    }
}