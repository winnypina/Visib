using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;

namespace Visib.Mobile.Droid
{
    [Activity(
        Label = "Visib", Icon = "@drawable/icon",
        Theme = "@style/AppTheme.Splash",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxFormsSplashScreenActivity<Setup, VisibApp, App>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;

            // Create your application here
            StartActivity(typeof(MainActivity));
        }
    }

}
