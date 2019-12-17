using System;
using Rg.Plugins.Popup.Services;

namespace Visib.Mobile.Popups
{
    public partial class PopupWithOptions
    {
        private readonly Action<string> _okAction;

        public PopupWithOptions(string message, Action<string> OkAction, string okText, string cancelText)
        {
            _okAction = OkAction;
            InitializeComponent();
            MessageLabel.Text = message;
            ConfirmLabel.Text = okText;
            CancelLabel.Text = cancelText;
        }

        private async void CancelTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void OkTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            _okAction(InputEntry.Text);
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}