using System;
using MvvmCross.Forms.Presenters.Attributes;
using Visib.Mobile.ViewModels;
using Xamarin.Forms;

namespace Visib.Mobile.Views
{
    [MvxModalPresentation]
    public partial class CommentsView
    {
        public CommentsView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Entry_OnCompleted(object sender, EventArgs e)
        {
            var viewModel = ViewModel as CommentsViewModel;
            if (viewModel.CreateCommentCommand.CanExecute())
            {
                await viewModel.CreateCommentCommand.ExecuteAsync();
            }
        }
    }
}
