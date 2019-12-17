using MvvmCross.Forms.Presenters.Attributes;

namespace Visib.Mobile.Views
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, Icon = "search.png")]
    public partial class SearchView
    {
        public SearchView()
        {
            InitializeComponent();
        }
    }
}
