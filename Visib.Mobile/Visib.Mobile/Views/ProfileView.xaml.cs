using MvvmCross.Forms.Presenters.Attributes;

namespace Visib.Mobile.Views
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, Icon = "profile.png")]
    public partial class ProfileView
    {
        public ProfileView()
        {
            InitializeComponent();
        }
    }
}
