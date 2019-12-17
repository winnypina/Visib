using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Visib.Mobile.Services.Requests;

namespace Visib.Mobile.ViewModels
{
    public class SignupViewModel : MvxNavigationViewModel
    {
        private string _email;
        private bool _isEmail;
        private bool _isPhone;

        private string _loginLabel;
        private string _name;
        private string _password;
        private string _confirmPassword;
        private string _phone;

        private string _title;
        private bool isLoading;
        private readonly IPopupService popupService;
        private readonly ILoginService loginService;

        public SignupViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPopupService popupService, ILoginService loginService) : base(logProvider, navigationService)
        {
            IsPhone = true;
            IsEmail = false;
            LoginLabel = "Telefone";
            this.popupService = popupService;
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

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }


        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                SetProperty(ref _phone, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public MvxCommand ChangeModeCommand => new MvxCommand(ExecuteChangeModeCommand);

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(ExecuteSendCommand);

        public override void Prepare()
        {
            Title = "Cadastro Telefone";
        }

        private void ExecuteChangeModeCommand()
        {
            IsPhone = !IsPhone;
            IsEmail = !IsEmail;
            LoginLabel = IsPhone ? "Telefone" : "Email";
            Email = string.Empty;
            Phone = string.Empty;
            Title = IsPhone ? "Cadastro Telefone" : "Cadastro Email";

#if DEBUG
            Name = "Ricardo";
            if (IsPhone)
            {
                Phone = "15996703410";
            }
            else
            {
                Email = "ricardozas@gmail.com";
            }

            Password = "teste@123";
            ConfirmPassword = "teste@123";
#endif
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }


        private async Task ExecuteSendCommand()
        {
            IsLoading = true;

            var userInteractionService = Mvx.IoCProvider.Resolve<IUserInteractionService>();

            if (string.IsNullOrEmpty(Name) || (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone)) || string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword))
            {
                await userInteractionService.DisplayMessage("Preenchimento obrigatório", "Por favor preencha todos os campos");
                IsLoading = false;
                return;
            }

            if (IsEmail)
            {
                var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var match = regex.Match(Email);
                if (!match.Success)
                {
                    await userInteractionService.DisplayMessage("Email inválido", "Por favor utilize um email valido");
                    IsLoading = false;
                    return;
                }
            }

            if (IsPhone)
            {
                var regex = new Regex(@"^\(\d{2}\)9\d{4}-\d{4}|\((?:1[2-9]|[2-9]\d)\) [5-9]\d{3}-\d{4}$");
                var match = regex.Match(Phone);
                if (!match.Success)
                {
                    await userInteractionService.DisplayMessage("Telefone inválido", "Por favor utilize um telefone valido");
                    IsLoading = false;
                    return;
                }
            }

            if (Password != ConfirmPassword)
            {
                await userInteractionService.DisplayMessage("Senhas não conferem", "A senha e a confirmação da senha não conferem");
                IsLoading = false;
                return;
            }


            var login = !string.IsNullOrEmpty(Email) ? Email : Phone;
            var emailLogin = login.Contains("@") ? login : "no@email.com";
            var phoneLogin = !login.Contains("@") ? login : null;
            var account = new CreateAccountRequest
            {
                CpfCnpj = null,
                Name = Name,
                Password = Password,
                Email = emailLogin,
                BirthDate = null,
                Address = null,
                MobilePhone = phoneLogin
            };

            if (IsPhone)
            {

                if (await loginService.Signup(Name, login, Password, null)
                    .ConfigureAwait(false))
                {
                    await popupService.ShowWithOptions("Digite o código recebido por SMS", async (obj) =>
                    {
                        IsLoading = true;
                        await VerifyCode(obj);
                    }, "CONFIRMAR", "CANCELAR");
                }
                else
                {
                    await userInteractionService.DisplayMessage("Erro",
                        "Ocorreu um problema ao efetuar seu registro. Por favor tente novamente mais tarde");
                }
            }
            else
            {

                if (await loginService.Signup(Name, login, Password, null)
                    .ConfigureAwait(false))
                {

                    await popupService.ShowWithOptions("Digite o código recebido por email", async (obj) =>
                    {
                        IsLoading = true;
                        await VerifyCode(obj);
                    }, "CONFIRMAR", "CANCELAR");
                }
                else
                {
                    await userInteractionService.DisplayMessage("Erro",
                       "Ocorreu um problema ao efetuar seu registro. Por favor tente novamente mais tarde");
                }

            }

            IsLoading = false;
        }

        private async Task VerifyCode(string code)
        {
            var login = !string.IsNullOrEmpty(Email) ? Email : Phone;

            var userInteractionService = Mvx.IoCProvider.Resolve<IUserInteractionService>();

            if (await loginService.VerifyCode(login, Convert.ToInt32(code)))
            {
                if (await loginService.Login(login, Password))
                {
                    await NavigationService.Navigate<TabsMasterViewModel>();
                }
                else
                {
                    await userInteractionService.DisplayMessage("Erro", "Ocorreu um erro na tentiva de verificação do código. Tente novamente mais tarde!");
                }


            }
            else
            {
                await userInteractionService.DisplayMessage("Erro", "Código inválido, por favor verifique o código enviado!");
                await popupService.ShowWithOptions($"Digite o código recebido por {(IsPhone ? "SMS" : "Email")}", async (obj) =>
                {
                    IsLoading = true;
                    await VerifyCode(obj);
                }, "CONFIRMAR", "CANCELAR");
            }
        }
    }

}
