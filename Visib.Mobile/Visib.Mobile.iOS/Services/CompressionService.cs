using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AVFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using UIKit;
using Visib.Mobile.Services;

namespace Visib.Mobile.iOS.Services
{
    public class CompressionService : ICompressionService
    {
        public async Task AddMusic(string videoSource, string musicPath, Action onCompleted)
        {
            var output = Path.Combine(Path.GetDirectoryName(videoSource), "videowithmusic.mp4");
            if (File.Exists(output))
            {
                File.Delete(output);
            }


            var mixComposition = new AVMutableComposition();
            var mutableCompositionVideoTrack = new List<AVMutableCompositionTrack>();
            var mutableCompositionAudioTrack = new List<AVMutableCompositionTrack>();
            var totalVideoCompositionInstruction = new AVMutableVideoCompositionInstruction();


            //start merge
            var aVideoAsset = new AVUrlAsset(NSUrl.CreateFileUrl(videoSource, null));
            var aAudioAsset = new AVUrlAsset(NSUrl.CreateFileUrl(musicPath, null));

            await aAudioAsset.LoadValuesTaskAsync(new[] { "tracks" });
            var audiostatus = aAudioAsset.StatusOfValue("tracks", out var audiotrackError);
            switch (audiostatus)
            {
                case AVKeyValueStatus.Unknown:
                    break;
                case AVKeyValueStatus.Loading:
                    break;
                case AVKeyValueStatus.Loaded:
                    break;
                case AVKeyValueStatus.Failed:
                    break;
                case AVKeyValueStatus.Cancelled:
                    break;
                default:
                    break;
            }

            await aVideoAsset.LoadValuesTaskAsync(new[] { "tracks" });
            var videoStatus = aVideoAsset.StatusOfValue("tracks", out var videotrackError);
            switch (videoStatus)
            {
                case AVKeyValueStatus.Unknown:
                    break;
                case AVKeyValueStatus.Loading:
                    break;
                case AVKeyValueStatus.Loaded:
                    break;
                case AVKeyValueStatus.Failed:
                    break;
                case AVKeyValueStatus.Cancelled:
                    break;
                default:
                    break;
            }


            mutableCompositionVideoTrack.Add(mixComposition.AddMutableTrack(AVMediaType.Video, 0));
            mutableCompositionAudioTrack.Add(mixComposition.AddMutableTrack(AVMediaType.Audio, 0));

            var videoTracks = aVideoAsset.TracksWithMediaType(AVMediaType.Video);
            var aVideoAssetTrack = videoTracks[0];
            var aAudioAssetTrack = aAudioAsset.TracksWithMediaType(AVMediaType.Audio)[0];

            var range = new CMTimeRange();
            range.Start = CMTime.Zero;
            range.Duration = aVideoAssetTrack.TimeRange.Duration;

            mutableCompositionVideoTrack[0].InsertTimeRange(range, aVideoAssetTrack, CMTime.Zero, out var error);



            //In my case my audio file is longer then video file so i took videoAsset duration
            //instead of audioAsset duration

            mutableCompositionAudioTrack[0].InsertTimeRange(new CMTimeRange { Start = CMTime.Zero, Duration = aVideoAsset.Duration }, aAudioAssetTrack, CMTime.Zero, out var errorAudio);

            //Use this instead above line if your audiofile and video file's playing durations are same

            //            try mutableCompositionAudioTrack[0].insertTimeRange(CMTimeRangeMake(kCMTimeZero, aVideoAssetTrack.timeRange.duration), ofTrack: aAudioAssetTrack, atTime: kCMTimeZero)

            mutableCompositionVideoTrack[0].PreferredTransform = aVideoAssetTrack.PreferredTransform;
          

            //find your video on this URl
            
            


            var assetExport = new AVAssetExportSession(mixComposition, AVAssetExportSessionPreset.HighestQuality);
            assetExport.OutputFileType = AVFileType.Mpeg4;
            assetExport.OutputUrl = NSUrl.CreateFileUrl(output,null);
            assetExport.ShouldOptimizeForNetworkUse = true;

            assetExport.ExportAsynchronously(() =>
            {
                switch (assetExport.Status)
                {
                    case AVAssetExportSessionStatus.Completed:
                        onCompleted();
                        break;
                    case AVAssetExportSessionStatus.Failed:
                        break;
                    case AVAssetExportSessionStatus.Cancelled:
                        break;
                    default:
                        break;
                }
            });



        }

        public Task Compress(string source, string destination, Action onCompleted)
        {

            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            var asset = AVAsset.FromUrl(NSUrl.FromFilename(source));

            AVAssetExportSession export = new AVAssetExportSession(asset, AVAssetExportSessionPreset.Preset960x540);

            export.OutputUrl = NSUrl.FromFilename(destination);
            export.OutputFileType = AVFileType.Mpeg4;
            export.ShouldOptimizeForNetworkUse = true;

            export.ExportAsynchronously(() =>
            {
                if (export.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(export.Error.LocalizedDescription);
                }
                onCompleted();
            });

            return Task.CompletedTask;
        }

        public Task GenerateCovers(string source, Action onCompleted)
        {
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(source)))
            {
                if (file.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
                {
                    File.Delete(file);
                }
            }

            var asset = AVAsset.FromUrl(NSUrl.FromFilename(source));

            var totalFrames = 5;
            var period = Convert.ToInt32(Math.Floor((double)asset.Duration.Seconds / totalFrames));
            if (period < 1) period = 1;

            var imageGenerator = new AVAssetImageGenerator(asset);
            imageGenerator.AppliesPreferredTrackTransform = true;

            int count = 1;
            for (int x = 0; x <= asset.Duration.Seconds; x += period)
            {
                var image = imageGenerator.CopyCGImageAtTime(new CoreMedia.CMTime(x, 1), out var actualTime, out var error);
                var imageData = new UIImage(image).AsPNG();
                var filename = $"thumb{count.ToString().PadLeft(4, '0')}.jpg";
                var fullFileName = Path.Combine(Path.GetDirectoryName(source), filename);
                imageData.Save(fullFileName, false);
                count++;
            }
            Task.Run(() =>
            {
                onCompleted();
            });
            return Task.CompletedTask;
        }
    }
}
