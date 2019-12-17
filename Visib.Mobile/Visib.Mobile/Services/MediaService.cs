using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Polly;
using Refit;

namespace Visib.Mobile.Services
{
    public class MediaService : IMediaService
    {

        private readonly IApiService _apiService;

        public MediaService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task Upload(byte[] data, string fileName)
        {
           if (!CrossConnectivity.Current.IsConnected) return;
           var signupTask = _apiService.UserInitiated.UploadMedia(new List<StreamPart> { new StreamPart(new MemoryStream(data),fileName)}).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
