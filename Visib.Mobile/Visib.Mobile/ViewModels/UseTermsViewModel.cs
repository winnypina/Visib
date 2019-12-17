using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class UseTermsViewModel : MvxNavigationViewModel
    {
        private readonly ITranslationService _translationService;
        private string useTermsText;
        private bool isLoading;

        public UseTermsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService)
        {
            _translationService = translationService;
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public string UseTermText
        {
            get => useTermsText;
            set => SetProperty(ref useTermsText, value);
        }

        public override async Task Initialize()
        {
            IsLoading = true;
            var terms = await _translationService.GetUseTerms().ConfigureAwait(false);
            UseTermText = terms;
            IsLoading = false;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));
        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));
    }
}
