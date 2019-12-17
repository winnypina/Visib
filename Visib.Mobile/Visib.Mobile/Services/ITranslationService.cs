using System.Collections.Generic;
using System.Threading.Tasks;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.Services
{

    public enum Languages
    {
        PtBr,
        EnUs,
        Es
    }

    public interface ITranslationService
    {
        Languages CurrentLanguage { get; }

        Dictionary<string, List<TranslationModel>> TranslatedTexts { get; set; }
        Task GetTranslations();

        Task<string> GetUseTerms();

        Task<string> GetPrivacyPolicy();

        void SetCurrentLanguage(Languages language);


    }
}