
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Forms.Platforms.Ios.Core;
using Xamarin.Forms;

namespace Visib.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, VisibApp, App>
    {

        protected override void LoadFormsApplication()
        {
            Rg.Plugins.Popup.Popup.Init();
            base.LoadFormsApplication();
            Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            AppCenter.Start("d767f440-1cc6-4ca0-b251-730ab73b9d68",
                   typeof(Analytics), typeof(Crashes));
        }

    }
}
