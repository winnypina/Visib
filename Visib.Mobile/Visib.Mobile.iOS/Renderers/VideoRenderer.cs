using System;
using System.ComponentModel;
using System.IO;
using AVFoundation;
using AVKit;
using Foundation;
using MediaPlayer;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using UIKit;
using Visib.Mobile.Controls;
using Visib.Mobile.iOS.Renderers;
using Visib.Mobile.Messages;
using Visib.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Video), typeof(VideoRenderer))]
namespace Visib.Mobile.iOS.Renderers
{

    public class VideoRenderer : ViewRenderer<Video, UIView>
    {
        AVPlayer player;
        AVPlayerItem playerItem;
        AVPlayerViewController _playerViewController;       // solely for ViewController property
        MvxSubscriptionToken token;
        MvxSubscriptionToken playToken;
        MvxSubscriptionToken pauseToken;
        MvxSubscriptionToken unmuteToken;
        string currentPath;

        public VideoRenderer()
        {
            token = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<TabChangedMessage>(ReceiveTabChanged);
            playToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<PlayVideoMessage>(ReceivePlayVideo);
            pauseToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<PauseVideoMessage>(ReceivePauseVideo);
            unmuteToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<UnmuteVideoMessage>(ReceiveUnmuteVideo);
        }

        private void HandleNotification(NSNotification notification)
        {
            var currentPlayerItem = (AVPlayerItem)notification.Object;
            if (currentPlayerItem == player.CurrentItem)
            {
                player?.Seek(CoreMedia.CMTime.Zero);
                player?.Play();
            }
        }

        private void ReceiveUnmuteVideo(UnmuteVideoMessage obj)
        {
            if (player != null)
            {
                if (obj.Path == currentPath)
                {
                    player.Muted = false;
                }
                else
                {
                    player.Muted = true;
                }
            }
        }

        private void ReceivePauseVideo(PauseVideoMessage obj)
        {
            if (obj.Path == currentPath)
            {
                if (player != null)
                {
                    player.Pause();
                    player.Seek(CoreMedia.CMTime.Zero);
                    player.Muted = true;
                }
            }
        }

        private void ReceivePlayVideo(PlayVideoMessage obj)
        {
            if (obj.Path == currentPath)
            {
                player?.Play();
            }
            else
            {
                player?.Pause();
            }
        }

        private void ReceiveTabChanged(TabChangedMessage obj)
        {
            player?.Pause();
        }


        public override UIViewController ViewController => _playerViewController;

        protected override void OnElementChanged(ElementChangedEventArgs<Video> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    // Create AVPlayerViewController
                    _playerViewController = new AVPlayerViewController();

                    // Set Player property to AVPlayer
                    player = new AVPlayer();
                    _playerViewController.Player = player;
                    _playerViewController.ShowsPlaybackControls = false;
                    _playerViewController.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
                    NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, HandleNotification);
                    // Use the View from the controller as the native control
                    SetNativeControl(_playerViewController.View);
                }

                //SetAreTransportControlsEnabled();
                SetSource();

                //args.NewElement.UpdateStatus += OnUpdateStatus;
                //args.NewElement.PlayRequested += OnPlayRequested;
                //args.NewElement.PauseRequested += OnPauseRequested;
                //args.NewElement.StopRequested += OnStopRequested;
            }

            if (e.OldElement != null)
            {
                //args.OldElement.UpdateStatus -= OnUpdateStatus;
                //args.OldElement.PlayRequested -= OnPlayRequested;
                //args.OldElement.PauseRequested -= OnPauseRequested;
                //args.OldElement.StopRequested -= OnStopRequested;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (player != null)
            {
                player.ReplaceCurrentItemWithPlayerItem(null);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == Video.SourceProperty.PropertyName)
            {
                SetSource();
            }
        }




        void SetSource()
        {

            AVAsset asset = null;

            if (string.IsNullOrEmpty(Element.Source))
            {
                Element.Hide();
                return;
            }

            Element.IsVisible = true;

            if (Element.VideoId != Guid.Empty)
            {
                Mvx.IoCProvider.Resolve<IPostService>().View(Element.VideoId);
            }

            switch (Element.SourceType)
            {
                
                case VideoSourceType.TempFile:
                    asset = AVAsset.FromUrl(NSUrl.CreateFileUrl(Element.Source,null));
                    break;
                case VideoSourceType.Embedded:
                case VideoSourceType.Http:
                    asset = AVAsset.FromUrl(new NSUrl(Element.Source));
                    break;
            }
            currentPath = Element.Source;

            if (asset != null)
            {
                playerItem = new AVPlayerItem(asset);
            }
            else
            {
                playerItem = null;
            }

            player.ReplaceCurrentItemWithPlayerItem(playerItem);

            if (playerItem != null && Element.AutoPlay)
            {
                player.Play();
            }
        }

        // Event handler to update status
        //void OnUpdateStatus(object sender, EventArgs args)
        //{
        //    VideoStatus videoStatus = VideoStatus.NotReady;

        //    switch (player.Status)
        //    {
        //        case AVPlayerStatus.ReadyToPlay:
        //            switch (player.TimeControlStatus)
        //            {
        //                case AVPlayerTimeControlStatus.Playing:
        //                    videoStatus = VideoStatus.Playing;
        //                    break;

        //                case AVPlayerTimeControlStatus.Paused:
        //                    videoStatus = VideoStatus.Paused;
        //                    break;
        //            }
        //            break;
        //    }
        //    ((IVideoPlayerController)Element).Status = videoStatus;

        //    if (playerItem != null)
        //    {
        //        ((IVideoPlayerController)Element).Duration = ConvertTime(playerItem.Duration);
        //        ((IElementController)Element).SetValueFromRenderer(VideoPlayer.PositionProperty, ConvertTime(playerItem.CurrentTime));
        //    }
        //}

        //TimeSpan ConvertTime(CMTime cmTime)
        //{
        //    return TimeSpan.FromSeconds(Double.IsNaN(cmTime.Seconds) ? 0 : cmTime.Seconds);

        //}

        //// Event handlers to implement methods
        //void OnPlayRequested(object sender, EventArgs args)
        //{
        //    player.Play();
        //}

        //void OnPauseRequested(object sender, EventArgs args)
        //{
        //    player.Pause();
        //}

        //void OnStopRequested(object sender, EventArgs args)
        //{
        //    player.Pause();
        //    player.Seek(new CMTime(0, 1));
        //}
    }

    public class VideoRendererOld : ViewRenderer<Video, UIView>
    {
        AVPlayerViewController videoPlayer;
        NSObject notification = null;
        AVPlayer player;
        MvxSubscriptionToken token;
        MvxSubscriptionToken playToken;
        MvxSubscriptionToken pauseToken;
        MvxSubscriptionToken unmuteToken;
        private string currentPath;

        public VideoRendererOld()
        {
            token = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<TabChangedMessage>(ReceiveTabChanged);
            playToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<PlayVideoMessage>(ReceivePlayVideo);
            pauseToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<PauseVideoMessage>(ReceivePauseVideo);
            unmuteToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<UnmuteVideoMessage>(ReceiveUnmuteVideo);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void ReceiveUnmuteVideo(UnmuteVideoMessage obj)
        {
            if (player != null)
            {
                if (obj.Path == currentPath)
                {
                    player.Muted = false;
                }
                else
                {
                    player.Muted = true;
                }
            }
        }

        private void ReceivePauseVideo(PauseVideoMessage obj)
        {
            if (obj.Path == currentPath)
            {
                if (player != null)
                {
                    player.Pause();
                    player.Seek(CoreMedia.CMTime.Zero);
                    player.Muted = true;
                }
            }
        }

        private void ReceivePlayVideo(PlayVideoMessage obj)
        {
            if(obj.Path == currentPath)
            {
                player?.Play();
            } else
            {
                player?.Pause();
            }
        }

        private void ReceiveTabChanged(TabChangedMessage obj)
        {
            player?.Pause();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Video> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                InitVideoPlayer();

            }
            if (e.OldElement != null)
            {
                // Unsubscribe
                notification?.Dispose();
            }
            if (e.NewElement != null)
            {
                // Subscribe
                notification = MPMoviePlayerController.Notifications.ObservePlaybackDidFinish((sender, args) =>
                {
                    /* Access strongly typed args */
                    Console.WriteLine("Notification: {0}", args.Notification);
                    Console.WriteLine("FinishReason: {0}", args.FinishReason);

                    Element?.OnFinishedPlaying?.Invoke();
                });
            }
        }

        void InitVideoPlayer()
        {
            string path = null;

            if(string.IsNullOrEmpty(Element.Source))
            {
                Console.WriteLine("Video not exist");
                videoPlayer = new AVPlayerViewController();
                videoPlayer.View.BackgroundColor = UIColor.Clear;
                SetNativeControl(videoPlayer.View);
                return;
            }

            switch(Element.SourceType)
            {
                case VideoSourceType.Embedded:
                    path = Path.Combine(NSBundle.MainBundle.BundlePath, Element.Source);
                    if (!NSFileManager.DefaultManager.FileExists(path))
                    {
                        Console.WriteLine("Video not exist");
                        videoPlayer = new AVPlayerViewController();
                        videoPlayer.View.BackgroundColor = UIColor.Clear;
                        SetNativeControl(videoPlayer.View);
                        return;
                    }
                    break;
                case VideoSourceType.TempFile:
                case VideoSourceType.Http:
                    path = Element.Source;
                    break;
            }

            // Load the video from the app bundle.
            NSUrl videoURL = new NSUrl(path, false);
            currentPath = path;

            // Create and configure the movie player.
            player = new AVPlayer(videoURL);

            player.Muted = true;
            player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
            NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, HandleNotification);
            player.AddObserver(this, "error", NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.New, IntPtr.Zero);
            videoPlayer = new AVPlayerViewController();
            videoPlayer.Player = player;
            videoPlayer.ShowsPlaybackControls = false;
            videoPlayer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;

            videoPlayer.View.BackgroundColor = UIColor.Clear;
            foreach (UIView subView in videoPlayer.View.Subviews)
            {
                subView.BackgroundColor = UIColor.Clear;
            }

            //player.Play();
            SetNativeControl(videoPlayer.View);
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (keyPath == "error" && player.Error != null)
            {
                Element.Hide();
            }

        }

        private void HandleNotification(NSNotification notification)
        {
            var playerItem = (AVPlayerItem)notification.Object;
            if(playerItem == player.CurrentItem)
            {
                player?.Seek(CoreMedia.CMTime.Zero);
                player?.Play();
            }

        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Element == null || Control == null)
                return;

            if (e.PropertyName == Video.SourceProperty.PropertyName)
            {
                InitVideoPlayer();
            }
            else if (e.PropertyName == Video.LoopProperty.PropertyName)
            {
                var liveImage = Element as Video;
                //if (videoPlayer != null)
                //videoPlayer.RepeatMode = Element.Loop ? MPMovieRepeatMode.One : MPMovieRepeatMode.None;
            }
        }
    }
}
