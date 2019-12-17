using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class SplashViewModel : MvxNavigationViewModel
    {
        private readonly ILoginService loginService;

        public SplashViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService) : base(logProvider, navigationService)
        {
            this.loginService = loginService;
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            if(loginService.LoadCachedUser())
            {
                await NavigationService.Navigate<TabsMasterViewModel>();
            } else
            {
                await NavigationService.Navigate<WelcomeViewModel>();
            }
        }
    }
}
