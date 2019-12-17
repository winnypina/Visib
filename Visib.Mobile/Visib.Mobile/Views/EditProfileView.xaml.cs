using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;

namespace Visib.Mobile.Views
{
    [MvxModalPresentation]
    public partial class EditProfileView
    {
        public EditProfileView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
