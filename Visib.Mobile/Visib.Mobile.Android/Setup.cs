﻿using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using Visib.Mobile.Droid.Services;
using Visib.Mobile.Services;
using Xamarin.Forms;

namespace Visib.Mobile.Droid
{
    public class Setup : MvxFormsAndroidSetup<VisibApp, App>
    {
        protected override IMvxIoCProvider CreateIocProvider()
        {
            var provider = base.CreateIocProvider();
            provider.RegisterType<ICompressionService, CompressionService>();
            return provider;
        }

        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            return new CustomMvxFormsPagePresenter(viewPresenter);
        }

        private Application _formsApplication;

        public override Application FormsApplication
        {
            get
            {
                if (!Xamarin.Forms.Forms.IsInitialized)
                {
                    var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity ?? ApplicationContext;
                    var asmb = activity.GetType().Assembly;
                    Forms.SetFlags("CollectionView_Experimental");
                    Xamarin.Forms.Forms.Init(activity, null, ExecutableAssembly ?? asmb);
                }
                if (_formsApplication == null)
                {
                    _formsApplication = CreateFormsApplication();
                }
                if (Application.Current != _formsApplication)
                {
                    Application.Current = _formsApplication;
                }
                return _formsApplication;
            }
        }

    }

    public class CustomMvxFormsPagePresenter : MvxFormsPagePresenter
    {
        public CustomMvxFormsPagePresenter(IMvxFormsViewPresenter platformPresenter) : base(platformPresenter)
        {
        }

        public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            var navigation = GetPageOfType<NavigationPage>()?.Navigation;
            if (hint is MvxPopToRootPresentationHint popToRootHint)
            {
                // Make sure all modals are closed
                await CloseAllModals(popToRootHint.Animated);

                // Double check we have a navigation page, otherwise
                // we can just return as we must be already at the root page
                if (navigation == null)
                    return true;

                // Close all pages back to the root
                await navigation.PopToRootAsync(popToRootHint.Animated);
                return true;
            }
            if (hint is MvxPopPresentationHint popHint)
            {
                var matched = await PopModalToViewModel(navigation, popHint);
                if (matched) return true;


                await PopToViewModel(navigation, popHint.ViewModelToPopTo, popHint.Animated);
                return true;
            }
            if (hint is MvxRemovePresentationHint removeHint)
            {
                foreach (var modal in navigation.ModalStack)
                {
                    var removed = RemoveByViewModel(modal.Navigation, removeHint.ViewModelToRemove);
                    if (removed)
                        return true;
                }

                RemoveByViewModel(navigation, removeHint.ViewModelToRemove);
                return true;
            }
            if (hint is MvxPagePresentationHint pageHint)
            {
                var pageType = ViewsContainer.GetViewType(pageHint.ViewModel);
                if (GetPageOfTypeByType(pageType) is Page page)
                {
                    if (page.Parent is TabbedPage tabbedPage)
                        tabbedPage.CurrentPage = page;
                    else if (page.Parent is CarouselPage carouselPage && page is ContentPage contentPage)
                        carouselPage.CurrentPage = contentPage;
                }
                return true;
            }
            if (hint is MvxPopRecursivePresentationHint popRecursiveHint)
            {
                var levels = popRecursiveHint.LevelsDeep;
                if (levels > navigation.NavigationStack.Count())
                    levels = navigation.NavigationStack.Count();
                for (int i = 0; i < levels; i++)
                {
                    await navigation.PopAsync(popRecursiveHint.Animated);
                }

                return true;
            }

            return true;
        }

        protected override NavigationPage CreateNavigationPage(Page pageRoot = null)
        {
            return new CustomNavigationPage(pageRoot);
        }
    }
}
