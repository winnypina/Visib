using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class PrivacyPolicyViewModel : MvxNavigationViewModel
    {
        private readonly ITranslationService _translationService;
        private string _privacyPolicyText;
        private bool isLoading;

        public PrivacyPolicyViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService)
        {
            _translationService = translationService;
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading,value); }

        public string PrivacyPolicyText
        {
            get => _privacyPolicyText;
            set => SetProperty(ref _privacyPolicyText, value);
        }

        public override async Task Initialize()
        {
            IsLoading = true;
            var terms = await _translationService.GetPrivacyPolicy().ConfigureAwait(false);
            PrivacyPolicyText = terms;
            IsLoading = false;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));
        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));
    }
}
