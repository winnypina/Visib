using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Media;
using Com.Github.Hiteshsondhi88.Libffmpeg;
using Com.Github.Hiteshsondhi88.Libffmpeg.Exceptions;
using Plugin.CurrentActivity;
using Visib.Mobile.Services;

namespace Visib.Mobile.Droid.Services
{

    public class CompressionHandler : ExecuteBinaryResponseHandler
    {
        private readonly Action onCompleted;

        public CompressionHandler(Action onCompleted)
        {
            this.onCompleted = onCompleted;
        }

        public override void OnSuccess(string message)
        {
            onCompleted();
        }

        public override void OnFailure(string message)
        {

        }

        public override void OnProgress(string message)
        {

        }


    }

    public class LoadBinaryHandler : LoadBinaryResponseHandler
    {
        private readonly string command;
        private readonly Activity activity;
        private readonly Action onCompleted;

        public LoadBinaryHandler(string command, Activity activity, Action onCompleted)
        {
            this.command = command;
            this.activity = activity;
            this.onCompleted = onCompleted;
        }

        public override void OnFailure()
        {

        }

        public override void OnSuccess()
        {
            try
            {
                var cmd = command.Split(' ');
                if (command.Length != 0)
                {
                    var ffmpeg = FFmpeg.GetInstance(activity);
                    ffmpeg.Execute(cmd, new CompressionHandler(onCompleted));
                }
            }
            catch (FFmpegCommandAlreadyRunningException e)
            {
                //There is a command already running
            }
        }
    }

    public class CompressionService : ICompressionService
    {
        public Task AddMusic(string videoSource, string musicPath, Action onCompleted)
        {
            var destination = Path.Combine(Path.GetDirectoryName(videoSource), "videowithmusic.mp4");
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            var retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(CrossCurrentActivity.Current.Activity, Android.Net.Uri.FromFile(new Java.IO.File(videoSource)));
            var time = retriever.ExtractMetadata(MetadataKey.Duration);
            int timeInSeconds = int.Parse(time) / 1000;

            var ffmpeg = FFmpeg.GetInstance(CrossCurrentActivity.Current.Activity);
            ffmpeg.Execute($"-hide_banner -i {videoSource} -i {musicPath} -map 0:v:0 -map 1:a:0 -t {timeInSeconds} -preset veryfast {destination}".Split(' '), new CompressionHandler(onCompleted));
            return Task.CompletedTask;
        }

        public Task Compress(string source, string destination, Action onCompleted)
        {
            if(File.Exists(destination))
            {
                File.Delete(destination);
            }
            var ffmpeg = FFmpeg.GetInstance(CrossCurrentActivity.Current.Activity);
            ffmpeg.LoadBinary(new LoadBinaryHandler($"-hide_banner -i {source} -crf 20 -preset veryfast -c:a copy -s 540x960 {destination}", CrossCurrentActivity.Current.Activity, onCompleted));
            return Task.CompletedTask;
        }

        public Task GenerateCovers(string source, Action onCompleted)
        {
            var retriever = new MediaMetadataRetriever();

            foreach(var file in Directory.GetFiles(Path.GetDirectoryName(source)))
            {
                if(file.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
                {
                    File.Delete(file);
                }
            }

            var filesPath = Path.Combine(Path.GetDirectoryName(source), "thumb%04d.jpg");
            //use one of overloaded setDataSource() functions to set your data source
            retriever.SetDataSource(CrossCurrentActivity.Current.Activity, Android.Net.Uri.FromFile(new Java.IO.File(source)));
            var time = retriever.ExtractMetadata(MetadataKey.Duration);

            long timeInSeconds = long.Parse(time) / 1000;
            retriever.Release();
            var totalFrames = 5;
            var period = Convert.ToInt32(Math.Floor((double)timeInSeconds / totalFrames));
            if (period < 1) period = 1;
            var ffmpeg = FFmpeg.GetInstance(CrossCurrentActivity.Current.Activity);
            ffmpeg.Execute($"-hide_banner -i {source} -vf fps=1/{period} {filesPath}".Split(' '), new CompressionHandler(onCompleted));
            return Task.CompletedTask;
        }
    }
}
