using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Visib.Mobile.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RectangleF = CoreGraphics.CGRect;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace Visib.Mobile.iOS.Renderers
{

    public class CustomEditorRenderer : EditorRendererBase<UITextView>
    {
        // Using same placeholder color as for the Entry
        readonly UIColor _defaultPlaceholderColor = UIColor.White;

        UILabel _placeholderLabel;

        public CustomEditorRenderer()
        {
            Frame = new RectangleF(0, 20, 320, 40);
        }

        protected override UITextView CreateNativeControl()
        {
            return new UITextView(RectangleF.Empty);
        }

        protected override UITextView TextView => Control;

        protected override void UpdateText()
        {
            base.UpdateText();
            _placeholderLabel.Hidden = !string.IsNullOrEmpty(TextView.Text);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            bool initializing = false;
            if (e.NewElement != null && _placeholderLabel == null)
            {
                initializing = true;
                // create label so it can get updated during the initial setup loop
                _placeholderLabel = new UILabel
                {
                    BackgroundColor = UIColor.Clear,
                    LineBreakMode = UILineBreakMode.WordWrap,
                    AdjustsFontSizeToFitWidth = false,
                    PreferredMaxLayoutWidth = 350,
                    Lines = 0
                };
            }

            base.OnElementChanged(e);

            if (e.NewElement != null && initializing)
            {
                CreatePlaceholderLabel();
            }
        }

        protected override void UpdateFont()
        {
            base.UpdateFont();
            _placeholderLabel.Font = Control.Font;
        }

        protected override void UpdatePlaceholderText()
        {
            _placeholderLabel.Text = Element.Placeholder;
        }

        protected override void UpdatePlaceholderColor()
        {
            Xamarin.Forms.Color placeholderColor = Element.PlaceholderColor;
            if (placeholderColor.IsDefault)
                _placeholderLabel.TextColor = _defaultPlaceholderColor;
            else
                _placeholderLabel.TextColor = placeholderColor.ToUIColor();
        }

        void CreatePlaceholderLabel()
        {
            if (Control == null)
            {
                return;
            }

            Control.AddSubview(_placeholderLabel);

            var edgeInsets = TextView.TextContainerInset;
            var lineFragmentPadding = TextView.TextContainer.LineFragmentPadding;

            var vConstraints = NSLayoutConstraint.FromVisualFormat(
                "V:|-" + edgeInsets.Top + "-[_placeholderLabel]-" + edgeInsets.Bottom + "-|", 0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { _placeholderLabel }, new NSObject[] { new NSString("_placeholderLabel") })
            );

            var hConstraints = NSLayoutConstraint.FromVisualFormat(
                "H:|-" + lineFragmentPadding + "-[_placeholderLabel]-" + lineFragmentPadding + "-|",
                0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { _placeholderLabel }, new NSObject[] { new NSString("_placeholderLabel") })
            );

            _placeholderLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            Control.AddConstraints(hConstraints);
            Control.AddConstraints(vConstraints);
        }
    }
}
