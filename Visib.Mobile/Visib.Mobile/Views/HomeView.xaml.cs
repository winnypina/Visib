using System;
using MvvmCross;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Plugin.Messenger;
using Visib.Mobile.Messages;
using Visib.Mobile.ViewModels;
using Visib.Mobile.ViewModels.Models;
using Xamarin.Forms;

namespace Visib.Mobile.Views
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, Icon = "home.png")]
    public partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //PostsListView.ItemAppearing+= PostsListView_ItemAppearing;
            //PostsListView.ItemDisappearing+= PostsListView_ItemDisappearing;
            //PostsListView.ItemTapped += PostsListView_ItemTapped;
            //PostsListView.RowHeight = (int)Application.Current.MainPage.Height;
        }

        void Handle_Error(object sender, FFImageLoading.Forms.CachedImageEvents.ErrorEventArgs e)
        {

        }

        private void PostsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IMvxMessenger>().Publish(new UnmuteVideoMessage(this) { Path = (e.Item as PostModel).VideoSource });
        }

        private void PostsListView_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IMvxMessenger>().Publish(new PauseVideoMessage(this) { Path = (e.Item as PostModel).VideoSource });
        }

        private async void PostsListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IMvxMessenger>().Publish(new PlayVideoMessage(this) { Path = (e.Item as PostModel).VideoSource });
            await (ViewModel as HomeViewModel).ViewCommand.ExecuteAsync(e.Item as PostModel).ConfigureAwait(false);
        }

    }
}
