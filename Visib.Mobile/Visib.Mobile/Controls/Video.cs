using System;
using Xamarin.Forms;

namespace Visib.Mobile.Controls
{

    public enum VideoSourceType
    {
        Embedded,
        TempFile,
        Http
    }

    public class Video : View
    {
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(
                nameof(Source),
                typeof(string),
                typeof(Video),
                string.Empty,
                BindingMode.TwoWay);

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly BindableProperty SourceTypeProperty =
            BindableProperty.Create(
                nameof(SourceType),
                typeof(VideoSourceType),
                typeof(Video),
                VideoSourceType.Embedded,
                BindingMode.TwoWay);

        public VideoSourceType SourceType
        {
            get { return (VideoSourceType)GetValue(SourceTypeProperty); }
            set { SetValue(SourceTypeProperty, value); }
        }

        public static readonly BindableProperty VideoIdProperty =
            BindableProperty.Create(
                nameof(VideoId),
                typeof(Guid),
                typeof(Video),
                Guid.Empty,
                BindingMode.TwoWay);

        public Guid VideoId
        {
            get { return (Guid)GetValue(VideoIdProperty); }
            set { SetValue(VideoIdProperty, value); }
        }

        public static readonly BindableProperty LoopProperty =
            BindableProperty.Create(
                nameof(Loop),
                typeof(bool),
                typeof(Video),
                true,
                BindingMode.TwoWay);

        public bool Loop
        {
            get { return (bool)GetValue(LoopProperty); }
            set { SetValue(LoopProperty, value); }
        }


        public void Hide()
        {
            IsVisible = false;
        }

        public static readonly BindableProperty AutoPlayProperty =
            BindableProperty.Create(
                nameof(AutoPlay),
                typeof(bool),
                typeof(Video),
                true,
                BindingMode.TwoWay);

        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }


        public Action OnFinishedPlaying { get; set; }
    }
}
