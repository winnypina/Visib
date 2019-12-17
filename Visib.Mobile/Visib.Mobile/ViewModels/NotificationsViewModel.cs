using System.Linq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Visib.Mobile.Services.Responses;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.ViewModels
{
    public class NotificationsViewModel : MvxNavigationViewModel
    {
        private readonly IPostService postService;
        private bool isLoading;

        public NotificationsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService) : base(logProvider, navigationService)
        {
            this.postService = postService;
            Notifications = new MvxObservableCollection<NotificationModel>();
        }

        public MvxObservableCollection<NotificationModel> Notifications { get; set; }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading,value); }

        public override async void ViewAppeared()
        {
            IsLoading = true;

            var notifications = await postService.GetNotifications().ConfigureAwait(false);
            if(Notifications.Count >0)
            {
                Notifications.RemoveRange(0, Notifications.Count);
            }
            if(notifications != null)
            {
               Notifications.AddRange(notifications.OrderByDescending(n => n.LastChange));
            }

            IsLoading = false;
        }
    }
}
