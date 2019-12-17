
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using Plugin.Permissions;
using Visib.Mobile.ViewModels;
using Xamarin.Auth.Presenters.XamarinAndroid;

namespace Visib.Mobile.Droid
{

    public class GlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly Window window;

        public GlobalLayoutListener(Window window)
        {
            this.window = window;
        }

        public void OnGlobalLayout()
        {
            var r = new Rect();
            window.DecorView.GetWindowVisibleDisplayFrame(r);
            int screenHeight = window.DecorView.RootView.Height;

            int keypadHeight = screenHeight - r.Bottom;

            //Log.d(TAG, "keypadHeight = " + keypadHeight);

            if (keypadHeight > screenHeight * 0.15)
            {
                //Keyboard is opened
            }
            else
            {
                int uiOptions = (int)window.DecorView.SystemUiVisibility;

                uiOptions |= (int)SystemUiFlags.LowProfile;
                uiOptions |= (int)SystemUiFlags.Fullscreen;
                uiOptions |= (int)SystemUiFlags.HideNavigation;
                uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

                window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            }
        }
    }

    [Activity(Label = "Visib", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MvxFormsAppCompatActivity<Setup, VisibApp, App>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Window.DecorView.ViewTreeObserver.AddOnGlobalLayoutListener(new GlobalLayoutListener(Window));

            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;

            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            SetStatusBarColor(Android.Graphics.Color.Transparent);

            AppCenter.Start("e90300e9-eb6d-41d5-ba60-6df32d551f45",
                   typeof(Analytics), typeof(Crashes));
        }

        public override void InitializeForms(Bundle bundle)
        {
            base.InitializeForms(bundle);
            AuthenticationConfiguration.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
#pragma warning disable XA0001 // Find issues with Android API usage
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
#pragma warning restore XA0001 // Find issues with Android API usage
        }



        public override void OnWindowFocusChanged(bool hasFocus)
        {
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }

        protected override async void OnResume()
        {
            base.OnResume();
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;

            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            
        }
    }
}