using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;

namespace Visib.Mobile.Views
{
    [MvxModalPresentation]
    public partial class PrivacyPolicyView 
    {
        public PrivacyPolicyView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
