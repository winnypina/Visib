using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.ViewModels
{
    public class UserProfileViewModel : MvxNavigationViewModel<string>
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
        private string userId;
        private bool isFollowing;

        public UserProfileViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService, ILoginService loginService) : base(logProvider, navigationService)
        {
            this.postService = postService;
            this.loginService = loginService;
            Posts = new MvxObservableCollection<PostModel>();
        }

        public override void Prepare(string parameter)
        {
            userId = parameter;
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

        public string VideoSource { get => videoSource; set => SetProperty(ref videoSource, value); }

        public bool IsFollowing { get => isFollowing; set => SetProperty(ref isFollowing, value); }

        public MvxObservableCollection<PostModel> Posts { get; }

        public MvxAsyncCommand FollowCommand => new MvxAsyncCommand(async () =>
        {
            IsFollowing = true;
            Followers++;
            loginService.Account.Follows++;
            await loginService.Follow(userId).ConfigureAwait(false);

        });

        public MvxAsyncCommand<PostModel> ShowVideoCommand => new MvxAsyncCommand<PostModel>(async post => {
            await NavigationService.Navigate<VideoDisplayViewModel, PostModel>(post);
        });

        public MvxAsyncCommand UnfollowCommand => new MvxAsyncCommand(async () =>
        {
            IsFollowing = false;
            Followers--;
            loginService.Account.Follows--;
            await loginService.Follow(userId).ConfigureAwait(false);
        });

        public override async Task Initialize()
        {
            IsLoading = true;
            var account = await loginService.GetProfile(userId).ConfigureAwait(false);
            if (account != null)
            {
                var thisFollowers = await loginService.GetFollowers(userId).ConfigureAwait(false);
                IsFollowing = thisFollowers.Select(n => n.Id).Contains(loginService.Account.Id);
                Name = account.Name;
                Email = account.Email;
                Country = account.Country;
                Description = account.About;
                Follows = account.Follows;
                Followers = account.Followers;
                VideoSource = $"https://s3.us-east-2.amazonaws.com/visib/{account.Id}.mp4";
                ProfileImage = $"https://s3.us-east-2.amazonaws.com/visib/{account.Id}.png";
                await GetPosts();
            }
            IsLoading = false;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        private async Task GetPosts()
        {
            IsLoading = true;
            var postsService = await postService.GetPostsForUser(userId).ConfigureAwait(false);
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
            {
                var posts = postsService?.ToList();
                PostCount = posts?.Count ?? 0;
                if (Posts.Count > 0)
                {
                    Posts.RemoveRange(0, Posts.Count);
                }
                Posts.AddRange(posts);
            });
            IsLoading = false;
        }
    }
}
