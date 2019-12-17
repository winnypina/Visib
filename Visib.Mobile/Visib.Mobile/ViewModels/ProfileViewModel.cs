using System.Linq;
using System.Threading.Tasks;
using FFImageLoading;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Connectivity;
using Visib.Mobile.Services;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.ViewModels
{
    public class ProfileViewModel : MvxNavigationViewModel
    {
        private readonly IPostService postService;
        private readonly ILoginService loginService;
        private bool isLoading;
        private string profileImage;
        private string name;
        private string country;
        private string email;
        private string description;
        private int postCount;
        private int follows;
        private int followers;
        private string videoSource;

        public ProfileViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService, ILoginService loginService) : base(logProvider, navigationService)
        {
            this.postService = postService;
            Posts = new MvxObservableCollection<PostModel>();
            this.loginService = loginService;
        }

        public string ProfileImage { get => profileImage; set => SetProperty(ref profileImage, value); }
        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Country { get => country; set => SetProperty(ref country, value); }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string Description { get => description; set => SetProperty(ref description, value); }
        public int PostCount { get => postCount; set => SetProperty(ref postCount, value); }
        public int Follows { get => follows; set => SetProperty(ref follows, value); }
        public int Followers { get => followers; set => SetProperty(ref followers, value); }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public string VideoSource { get => videoSource; set => SetProperty(ref videoSource,value); }

        public MvxObservableCollection<PostModel> Posts { get; }

        public override async void ViewAppeared()
        {
            IsLoading = true;
            ProfileImage = string.Empty;
            if (loginService.Account != null)
            {
                await loginService.GetProfile(loginService.Account.Id);
                Name = loginService.Account.Name;
                Email = loginService.Account.Email;
                Country = loginService.Account.Country;
                Description = loginService.Account.About;
                Follows = loginService.Account.Follows;
                Followers = loginService.Account.Followers;
                VideoSource = $"https://s3.us-east-2.amazonaws.com/visib/{loginService.Account.Id}.mp4";
                ProfileImage = $"https://s3.us-east-2.amazonaws.com/visib/{loginService.Account.Id}.png";
                await GetPosts();
            }
            IsLoading = false;
        }

        public MvxAsyncCommand<PostModel> ShowVideoCommand => new MvxAsyncCommand<PostModel>(async post => {
            await NavigationService.Navigate<VideoDisplayViewModel, PostModel>(post);
        });

        public MvxAsyncCommand<PostModel> ShowFollowersCommand => new MvxAsyncCommand<PostModel>(async post => {
            await NavigationService.Navigate<FollowersViewModel>();
        });

        public MvxAsyncCommand<PostModel> ShowFollowsCommand => new MvxAsyncCommand<PostModel>(async post => {
            await NavigationService.Navigate<FollowsViewModel>();
        });

        public MvxAsyncCommand ShowMenuCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<ConfigurationViewModel>());

        private async Task GetPosts()
        {
            IsLoading = true;
            var postsService = await postService.GetPostsForUser(loginService.Account.Id).ConfigureAwait(false);
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
            {
                
                if(postsService != null)
                {
                    var posts = postsService.ToList();
                    PostCount = posts?.Count ?? 0;
                    if (Posts.Count > 0)
                    {
                        Posts.RemoveRange(0, Posts.Count);
                    }
                    Posts.AddRange(posts);
                }
                
            });
            IsLoading = false;
        }
    }
}
