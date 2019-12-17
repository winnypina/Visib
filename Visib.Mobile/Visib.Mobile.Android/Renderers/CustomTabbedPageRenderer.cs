using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Views;
using Visib.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using TabbedPage = Xamarin.Forms.TabbedPage;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace Visib.Mobile.Droid.Renderers
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        private bool _isShiftModeSet;

        public CustomTabbedPageRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            try
            {
                if (!_isShiftModeSet)
                {
                    var children = GetAllChildViews(ViewGroup);
                    ViewGroup.SetBackground(new ColorDrawable(Android.Graphics.Color.Transparent));
                    if (children.SingleOrDefault(x => x is BottomNavigationView) is BottomNavigationView bottomNav)
                    {
                        bottomNav.SetShiftMode(false, false);
                        bottomNav.ItemIconTintList = null;
                        bottomNav.SetBackground(new ColorDrawable(Android.Graphics.Color.ParseColor("#1d193a")));
                        bottomNav.ItemBackgroundResource = Resource.Drawable.transparent;
                        _isShiftModeSet = true;
                       
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error setting ShiftMode: {e}");
            }

        }

       

        private List<View> GetAllChildViews(View view)
        {
            if (!(view is ViewGroup group)) return new List<View> { view };

            var result = new List<View>();

            for (var i = 0; i < group.ChildCount; i++)
            {
                var child = group.GetChildAt(i);

                var childList = new List<View> { child };
                childList.AddRange(GetAllChildViews(child));

                result.AddRange(childList);
            }

            return result.Distinct().ToList();
        }
    }
}
