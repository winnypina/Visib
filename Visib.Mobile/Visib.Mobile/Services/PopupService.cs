using System;
using System.Threading.Tasks;
using Visib.Mobile.Popups;
using MvvmCross;
using MvvmCross.Base;
using Rg.Plugins.Popup.Services;

namespace Visib.Mobile.Services
{
    public class PopupService : IPopupService
    {
        public async Task ShowWithOptions(string message, Action<string> OkAction, string okText, string cancelText)
        {
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new PopupWithOptions(message, OkAction, okText, cancelText));
            });
            
        }

        public async Task Dismiss()
        {
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
            {
                await PopupNavigation.Instance.PopAllAsync();
            });
        }

    }
}