using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class SigninViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;
        private bool _isPhone;
        private bool _isEmail;
        private string _loginLabel;
        private bool isLoading;
        private string username;
        private string password;

        public SigninViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService) : base(logProvider,
            navigationService)
        {
            IsPhone = true;
            IsEmail = false;
            LoginLabel = "Telefone";
            this.loginService = loginService;


        }

        public bool IsPhone
        {
            get => _isPhone;
            set => SetProperty(ref _isPhone, value);
        }

        public bool IsEmail
        {
            get => _isEmail;
            set => SetProperty(ref _isEmail, value);
        }

        public string LoginLabel
        {
            get => _loginLabel;
            set => SetProperty(ref _loginLabel, value);
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public string Username { get => username; set => SetProperty(ref username, value); }

        public string Password { get => password; set => SetProperty(ref password,value); }

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            if (await loginService.Login(Username, Password).ConfigureAwait(false))
            {

                await NavigationService.Navigate<TabsMasterViewModel>();
            }
            else
            {
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayMessage("Erro","Login e/ou senha inválidos");
            }
            IsLoading = false;
        });

        public MvxCommand ChangeModeCommand => new MvxCommand(ExecuteChangeModeCommand);

        private void ExecuteChangeModeCommand()
        {
            IsPhone = !IsPhone;
            IsEmail = !IsEmail;
            LoginLabel = IsPhone ? "Telefone" : "Email";
            Username = string.Empty;
            Password = string.Empty;
#if DEBUG

            if(IsPhone)
            {
                Username = "15996703410";
            }
            if(IsEmail)
            {
                Username = "ricardozas@gmail.com";
            }
            Password = "teste@123";
#endif
        }
    }
}