using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Visib.Mobile.Services.Responses;
using Visib.Mobile.ViewModels.Models;
using Plugin.Connectivity;
using Polly;

namespace Visib.Mobile.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IApiService _apiService;
        public Languages CurrentLanguage { get; private set; }

        public Dictionary<string,List<TranslationModel>> TranslatedTexts { get; set; }

        private IEnumerable<TranslationResponse> _allTranslations;

        public TranslationService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task GetTranslations()
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            var signupTask = _apiService.UserInitiated.GetTranslation().ConfigureAwait(false);
            try
            {
                var translations = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                _allTranslations = translations.ToList();
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }
        }


        public async Task<string> GetUseTerms()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var signupTask = _apiService.UserInitiated.GetUseTerms().ConfigureAwait(false);
            try
            {
                var terms = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                switch (CurrentLanguage)
                {
                    case Languages.PtBr:
                        return terms.Text;
                    case Languages.EnUs:
                        return terms.TextEnUs;
                    case Languages.Es:
                        return terms.TextEs;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<string> GetPrivacyPolicy()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var signupTask = _apiService.UserInitiated.GetPrivacyPolicy().ConfigureAwait(false);
            try
            {
                var terms = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                switch (CurrentLanguage)
                {
                    case Languages.PtBr:
                        return terms.Text;
                    case Languages.EnUs:
                        return terms.TextEnUs;
                    case Languages.Es:
                        return terms.TextEs;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public void SetCurrentLanguage(Languages language)
        {
            CurrentLanguage = language;
            var list = _allTranslations.ToList();
            var allScreens = list.Select(n => n.Screen).Distinct();
            TranslatedTexts = new Dictionary<string, List<TranslationModel>>();

            switch (language)
            {
                case Languages.PtBr:

                    foreach (var screen in allScreens)
                    {
                        TranslatedTexts.Add(screen, _allTranslations.Where(n=>n.Screen == screen).Select(n=> new TranslationModel
                        {
                            Value = n.Value,
                            Key = n.Key
                        }).ToList());
                    }

                    break;
                case Languages.EnUs:
                    foreach (var screen in allScreens)
                    {
                        TranslatedTexts.Add(screen, _allTranslations.Where(n => n.Screen == screen).Select(n => new TranslationModel
                        {
                            Value = n.ValueEnUs,
                            Key = n.Key
                        }).ToList());
                    }
                    break;
                case Languages.Es:
                    foreach (var screen in allScreens)
                    {
                        TranslatedTexts.Add(screen, _allTranslations.Where(n => n.Screen == screen).Select(n => new TranslationModel
                        {
                            Value = n.ValueEs,
                            Key = n.Key
                        }).ToList());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}