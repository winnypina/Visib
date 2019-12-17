using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;

namespace Visib.Mobile.Views
{
    [MvxModalPresentation]
    public partial class FollowersView 
    {
        public FollowersView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
