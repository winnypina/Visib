using System;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Base;
using Xamarin.Forms;

namespace Visib.Mobile.Services
{
    public class UserInteractionService : IUserInteractionService
    {

        public async Task DisplayMessage(string title, string message)
        {
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            });
        }

        public async Task DisplayConfirmation(string title, string message,string accept, string cancel, Action<bool> resultAction)
        {
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
            {
                resultAction(await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel));
            });
        }
    }
}
