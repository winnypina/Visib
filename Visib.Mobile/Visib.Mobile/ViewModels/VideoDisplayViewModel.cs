using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Visib.Mobile.ViewModels.Models;
using Xamarin.Forms;

namespace Visib.Mobile.ViewModels
{
    public class VideoDisplayViewModel : MvxNavigationViewModel<PostModel>
    {
        private bool isFilterRecent;
        private bool isFilterMostViewed;
        private bool isFilterMostApplauded;
        private bool isLoading;
        private readonly IPostService postService;
        private readonly ILoginService loginService;

        public VideoDisplayViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService, ILoginService loginService) : base(logProvider, navigationService)
        {
            Posts = new MvxObservableCollection<PostModel>();
            this.postService = postService;
            this.loginService = loginService;
            IsFilterRecent = true;
        }

        public int ScreenHeight { get { return (int)Application.Current.MainPage.Height; } }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public MvxObservableCollection<PostModel> Posts { get; }

        public bool IsFilterRecent { get => isFilterRecent; set => SetProperty(ref isFilterRecent, value); }

        public bool IsFilterMostViewed { get => isFilterMostViewed; set => SetProperty(ref isFilterMostViewed, value); }

        public bool IsFilterMostApplauded { get => isFilterMostApplauded; set => SetProperty(ref isFilterMostApplauded, value); }

        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public MvxAsyncCommand<string> GoToUserProfileCommand => new MvxAsyncCommand<string>(async (userId) =>
        {
            if (userId == loginService.Account.Id)
            {
                await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(ProfileViewModel)));
            }
            else
            {
                await NavigationService.Navigate<UserProfileViewModel, string>(userId);
            }
        });

        public MvxAsyncCommand<PostModel> ApplauseCommand => new MvxAsyncCommand<PostModel>(async (post) =>
        {
            post.HasUserApplause = !post.HasUserApplause;
            if (post.HasUserApplause)
            {
                post.ApplauseCount++;
            }
            else
            {
                post.ApplauseCount--;
            }
            await postService.Like(post.Id).ConfigureAwait(false);
        });

        public MvxAsyncCommand<PostModel> ShareCommand => new MvxAsyncCommand<PostModel>(async (post) =>
        {
            await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayConfirmation("Compartilhar", "Deseja compartilhar esse post?", "Sim", "Não", async result =>
            {
                if (result)
                {
                    IsLoading = true;
                    await postService.Share(post.Id).ConfigureAwait(false);
                    IsLoading = false;
                }
            });
        });

        public MvxAsyncCommand<PostModel> CommentsCommand => new MvxAsyncCommand<PostModel>(async (post) =>
        {
            IsLoading = true;
            await NavigationService.Navigate<CommentsViewModel, PostModel>(post);
            IsLoading = false;
        });

        public MvxAsyncCommand<PostModel> ViewCommand => new MvxAsyncCommand<PostModel>(async (post) =>
        {
            if (post.ViewedByUser == false)
            {
                post.ViewedByUser = true;
                post.ViewCount++;
                await postService.View(post.Id).ConfigureAwait(false);
            }

        });

        public MvxAsyncCommand ApplyRecentFilter => new MvxAsyncCommand(async () =>
        {
            IsFilterRecent = true;
            IsFilterMostViewed = false;
            IsFilterMostApplauded = false;
            await ApplyFilter();
        });

        public MvxAsyncCommand ApplyViewedFilter => new MvxAsyncCommand(async () =>
        {
            IsFilterRecent = false;
            IsFilterMostViewed = true;
            IsFilterMostApplauded = false;
            await ApplyFilter();
        });

        public MvxAsyncCommand ApplyApplaudedFilter => new MvxAsyncCommand(async () =>
        {
            IsFilterRecent = false;
            IsFilterMostViewed = false;
            IsFilterMostApplauded = true;
            await ApplyFilter();
        });

        public async Task ApplyFilter()
        {
            if (!IsLoading)
            {
                IsLoading = true;

                FilterType filter;
                if (IsFilterRecent)
                {
                    filter = FilterType.Recent;
                }
                else if (IsFilterMostViewed)
                {
                    filter = FilterType.Viewed;
                }
                else
                {
                    filter = FilterType.Applauded;
                }

                var posts = await postService.GetPosts(filter, 1).ConfigureAwait(false);
                await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
                {
                    posts?.ToList().ForEach(Posts.Add);
                });

                IsLoading = false;
            }

        }


        public override void Prepare(PostModel parameter)
        {
            Posts.Add(parameter);
        }
    }
}
