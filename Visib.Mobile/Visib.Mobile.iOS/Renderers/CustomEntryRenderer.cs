using Visib.Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]

namespace Visib.Mobile.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> args)
        {
            base.OnElementChanged(args);

            var textField = Control;

            if(textField != null)
            {
                textField.BorderStyle = UITextBorderStyle.None;

                // Use 'Done' on keyboard
                textField.ReturnKeyType = UIReturnKeyType.Done;
                textField.EnablesReturnKeyAutomatically = true;
            }
            
        }
    }
}