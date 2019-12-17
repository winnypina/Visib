using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Visib.Mobile.ViewModels
{
    public class TabsMasterViewModel : MvxNavigationViewModel
    {
        public TabsMasterViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        private bool _hasRun;

        public override async void ViewAppearing()
        {
            await ShowInitialViewModels();
            base.ViewAppearing();
        }

        private async Task ShowInitialViewModels()
        {

            if (!_hasRun)
            {
                _hasRun = true;
                var tasks = new List<Task>
                {
                    NavigationService.Navigate<HomeViewModel>(),
                    NavigationService.Navigate<SearchViewModel>(),
                    NavigationService.Navigate<NewPostViewModel>(),
                    NavigationService.Navigate<NotificationsViewModel>(),
                    NavigationService.Navigate<ProfileViewModel>()
                };
                await Task.WhenAll(tasks);

            }

        }
    }
}
