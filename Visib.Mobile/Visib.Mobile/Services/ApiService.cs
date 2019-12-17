using System;
using System.Net.Http;
using Fusillade;
using Refit;

namespace Visib.Mobile.Services
{
    public class ApiService : IApiService
    {

        //public const string ApiBaseAddress = "http://192.168.0.101:5000/api";
        //public const string ApiBaseAddress = "http://10.125.133.74:5000/api";
        public const string ApiBaseAddress = "https://admin.visib.com/api";
        public const string MediaBaseAddress = "https://s3.us-east-2.amazonaws.com/visib/";

        private readonly Lazy<IVisibService> _background;
        private readonly Lazy<IVisibService> _speculative;
        private readonly Lazy<IVisibService> _userInitiated;
        
        public ApiService(string apiBaseAddress = null)
        {
            IVisibService CreateClient(HttpMessageHandler messageHandler)
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress),
                    Timeout = TimeSpan.FromMinutes(10)
                };


                return RestService.For<IVisibService>(client);
            }

            _background = new Lazy<IVisibService>(() => CreateClient(
                new AuthenticatedHttpClientHandler(new HttpClientHandler(), Priority.Background)));

            _userInitiated = new Lazy<IVisibService>(() => CreateClient(
                new AuthenticatedHttpClientHandler(new HttpClientHandler(), Priority.UserInitiated)));

            _speculative = new Lazy<IVisibService>(() => CreateClient(
                new AuthenticatedHttpClientHandler(new HttpClientHandler(), Priority.Speculative)));
        }

        public IVisibService Background => _background.Value;

        public IVisibService UserInitiated => _userInitiated.Value;

        public IVisibService Speculative => _speculative.Value;
        
    }
}