using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;

namespace Visib.Mobile.Views
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = true, Icon = "newpost.png")]
    public partial class NewPostView
    {
        public NewPostView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
        }
    }
}
