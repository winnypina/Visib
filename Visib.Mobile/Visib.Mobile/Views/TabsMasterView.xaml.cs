using MvvmCross;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Plugin.Messenger;
using Visib.Mobile.Messages;

namespace Visib.Mobile.Views
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, NoHistory = true, Position = TabbedPosition.Root)]
    public partial class TabsMasterView 
    {
        public TabsMasterView()
        {
            InitializeComponent();
            CurrentPageChanged+= (sender, e) =>
            {
                Mvx.IoCProvider.Resolve<IMvxMessenger>().Publish(new TabChangedMessage(this));
            };
        }


    }
}
