using MvvmCross.Forms.Presenters.Attributes;

namespace Visib.Mobile.Views
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, Icon = "notifications.png")]
    public partial class NotificationsView 
    {
        public NotificationsView()
        {
            InitializeComponent();
        }
    }
}
