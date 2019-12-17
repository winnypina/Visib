using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Fusillade;
using MvvmCross;
using MvvmCross.Navigation;
using Punchclock;
using Visib.Mobile.Services;
using Visib.Mobile.ViewModels;

namespace Visib.Mobile
{
    internal class AuthenticatedHttpClientHandler : RateLimitedHttpMessageHandler
    {


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth == null)
            {
                var token = Mvx.IoCProvider.Resolve<ILoginService>().AccessToken;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Mvx.IoCProvider.Resolve<ILoginService>().Logout();
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<SplashViewModel>();
            }
            return response;
        }

        public AuthenticatedHttpClientHandler(HttpMessageHandler handler, Priority basePriority, int priority = 0, long? maxBytesToRead = null, OperationQueue opQueue = null, Func<HttpRequestMessage, HttpResponseMessage, string, CancellationToken, Task> cacheResultFunc = null) : base(handler, basePriority, priority, maxBytesToRead, opQueue, cacheResultFunc)
        {
        }
    }
}
