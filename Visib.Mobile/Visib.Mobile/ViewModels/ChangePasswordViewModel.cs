using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class ChangePasswordViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;
        private string newPasswordConfimation;
        private string newPassword;
        private string oldPassword;
        private bool hasPassword;
        private bool isLoading;

        public ChangePasswordViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService) : base(logProvider, navigationService)
        {
            this.loginService = loginService;
        }

        public string OldPassword { get => oldPassword; set => SetProperty(ref oldPassword, value); }
        public string NewPassword { get => newPassword; set => SetProperty(ref newPassword, value); }
        public string NewPasswordConfirmation { get => newPasswordConfimation; set => SetProperty(ref newPasswordConfimation, value); }

        public bool HasPassword { get => hasPassword; set => SetProperty(ref hasPassword, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public override void Prepare()
        {
            HasPassword = loginService.Account.HasSetPassword;
        }

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(async () =>
        {
            if (NewPassword != NewPasswordConfirmation)
            {
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayMessage("Confira os dados", "Senhas digitadas não conferem.");
                return;
            }

            IsLoading = true;

            if (await loginService.ChangePassword(OldPassword, NewPassword))
            {
                loginService.Account.HasSetPassword = true;
                await NavigationService.Close(this);
            }
            else
            {
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayMessage("Confira os dados", "Senha atual inválida.");
            }
            IsLoading = false;
        });
        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));
    }
}
