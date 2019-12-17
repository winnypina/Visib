using System;
using CoreGraphics;
using UIKit;
using Visib.Mobile.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace Visib.Mobile.iOS.Renderers
{

    public class TabbedPageRenderer : TabbedRenderer
    {

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            var appeareance = UITabBar.Appearance;
            appeareance.BarTintColor = UIColor.Clear;
            appeareance.BackgroundImage = new UIImage();
            appeareance.ShadowImage = new UIImage();

            if(e.NewElement != null)
            {
                var lineView = new UIView(frame: new CGRect(x: 0, y: 0, width: UIScreen.MainScreen.Bounds.Width, height: 1));
                lineView.BackgroundColor = UIColor.White;
                TabBar.AddSubview(lineView);
            }
           

            TabBar.BackgroundColor = UIColor.Clear;
            TabBar.Translucent = true;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (TabBar == null) return;
            if (TabBar.Items == null) return;


            var tabs = Element as TabbedPage;
            if (tabs != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateItem(TabBar.Items[i], tabs.Children[i].Icon);
                }
            }
        }

        private void UpdateItem(UITabBarItem item, string icon)
        {
            if (item == null)
                return;
            try
            {
                string newIcon = icon.Replace(".png", "Selected.png");
                if (item == null) return;
                if (item.SelectedImage == null) return;
                if (item.SelectedImage.AccessibilityIdentifier == icon) return;

                item.Image = UIImage.FromBundle(icon);
                item.Image = item.Image.ImageWithRenderingMode(UIKit.UIImageRenderingMode.AlwaysOriginal);

                item.SelectedImage = UIImage.FromBundle(newIcon);
                item.SelectedImage = item.SelectedImage.ImageWithRenderingMode(UIKit.UIImageRenderingMode.AlwaysOriginal);

                item.SelectedImage.AccessibilityIdentifier = icon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to set selected icon: " + ex);
            }
        }
    }
}
