using System;
using System.ComponentModel;
using Android.Content;
using Android.Media;
using Android.Runtime;
using Android.Widget;
using MvvmCross;
using MvvmCross.Navigation;
using Visib.Mobile.Controls;
using Visib.Mobile.Droid.Renderers;
using Visib.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ARelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportRenderer(typeof(Video), typeof(VideoPlayerRenderer))]
namespace Visib.Mobile.Droid.Renderers
{

    public class PreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        private readonly VideoView view;
        private readonly Video video;

        public PreparedListener(VideoView view, Video video)
        {
            this.view = view;
            this.video = video;
        }

        public void OnCompletion(MediaPlayer mp)
        {
            view.Resume();
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mp.Looping = true;
            if(video.AutoPlay)
            {
                view.Start();
            }
        }
    }

    public class ErrorListener : Java.Lang.Object, MediaPlayer.IOnErrorListener
    {
        private readonly VideoView view;
        private readonly Video video;

        public ErrorListener(VideoView view, Video video)
        {
            this.view = view;
            this.video = video;
        }

        public bool OnError(MediaPlayer mp, [GeneratedEnum] MediaError what, int extra)
        {
            video.Hide();
            return true;
        }
    }

    public class CompletionListener : Java.Lang.Object, MediaPlayer.IOnCompletionListener
    {
        private readonly VideoView view;

        public CompletionListener(VideoView view)
        {
            this.view = view;
        }

        public void OnCompletion(MediaPlayer mp)
        {
            view.Start();
        }
    }

    public class VideoPlayerRenderer : ViewRenderer<Video, ARelativeLayout>
    {
        VideoView videoView;
        MediaController mediaController;    // Used to display transport controls
        PreparedListener preparedListener;
        CompletionListener completionListener;
        ErrorListener errorListener;

        public VideoPlayerRenderer(Context context) : base(context)
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().BeforeChangePresentation += VideoPlayerRenderer_BeforeChangePresentation;
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().BeforeNavigate += VideoPlayerRenderer_BeforeNavigate;
        }

        private void VideoPlayerRenderer_BeforeNavigate(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            if(videoView != null)
            {
                videoView.StopPlayback();
            }
        }

        private void VideoPlayerRenderer_BeforeChangePresentation(object sender, MvvmCross.Navigation.EventArguments.ChangePresentationEventArgs e)
        {
            if (videoView != null)
            {
                videoView.StopPlayback();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Video> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    // Save the VideoView for future reference
                    videoView = new VideoView(Context);
                    preparedListener = new PreparedListener(videoView, Element);
                    completionListener = new CompletionListener(videoView);
                    errorListener = new ErrorListener(videoView, Element);
                    videoView.SetOnPreparedListener(preparedListener);
                    videoView.SetOnCompletionListener(completionListener);
                    videoView.SetOnErrorListener(errorListener);
                    // Put the VideoView in a RelativeLayout
                    ARelativeLayout relativeLayout = new ARelativeLayout(Context);
                    relativeLayout.AddView(videoView);

                    // Center the VideoView in the RelativeLayout
                    ARelativeLayout.LayoutParams layoutParams =
                        new ARelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                    layoutParams.AddRule(LayoutRules.AlignParentTop);
                    layoutParams.AddRule(LayoutRules.AlignParentBottom);
                    layoutParams.AddRule(LayoutRules.AlignParentLeft);
                    layoutParams.AddRule(LayoutRules.AlignParentRight);
                    videoView.LayoutParameters = layoutParams;
                    // Handle a VideoView event


                    SetNativeControl(relativeLayout);
                }

                SetAreTransportControlsEnabled();
                SetSource();
            }

            if (args.OldElement != null)
            {

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null && videoView != null)
            {

            }
            if (Element != null)
            {

            }

            base.Dispose(disposing);
        }



        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == Video.SourceProperty.PropertyName)
            {
                SetSource();
            }
        }

        void SetAreTransportControlsEnabled()
        {

            videoView.SetMediaController(null);

            if (mediaController != null)
            {
                mediaController.SetMediaPlayer(null);
                mediaController = null;
            }

        }

        void SetSource()
        {
            string filename = Element.Source;



            if (!string.IsNullOrEmpty(filename))
            {
                Element.IsVisible = true;
                if (Element.VideoId != Guid.Empty)
                {
                    Mvx.IoCProvider.Resolve<IPostService>().View(Element.VideoId);
                }
                switch (Element.SourceType)
                {
                    case VideoSourceType.Embedded:
                        string package = Context.PackageName;

                        filename = System.IO.Path.GetFileNameWithoutExtension(filename).ToLowerInvariant();
                        string uri = "android.resource://" + package + "/raw/" + filename;

                        videoView.SetVideoURI(Android.Net.Uri.Parse(uri));
                        break;
                    case VideoSourceType.TempFile:
                    case VideoSourceType.Http:
                        videoView.SetVideoURI(Android.Net.Uri.Parse(filename));
                        break;
                }


                if (Element.AutoPlay)
                {
                    videoView.Start();
                }
            }
            else
            {
                Element.IsVisible = false;
            }


        }

        // Event handler to update status
        void OnUpdateStatus(object sender, EventArgs args)
        {

        }

        // Event handlers to implement methods
        void OnPlayRequested(object sender, EventArgs args)
        {
            videoView.Start();
        }

        void OnPauseRequested(object sender, EventArgs args)
        {
            videoView.Pause();
        }

        void OnStopRequested(object sender, EventArgs args)
        {
            videoView.StopPlayback();
        }
    }
}
