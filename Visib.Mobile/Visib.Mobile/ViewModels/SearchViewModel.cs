using System;
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
    public class SearchViewModel : MvxNavigationViewModel
    {
        private readonly IPostService postService;
        private string searchTerm;
        private bool isSearching;
        private string currentSearchTerm;
        private bool isLoading;

        public SearchViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService) : base(logProvider, navigationService)
        {
            Posts = new MvxObservableCollection<PostModel>();
            this.postService = postService;
        }

        public MvxObservableCollection<PostModel> Posts { get; }

        public string SearchTerm
        {
            get => searchTerm; 
            set
            {
                if(SetProperty(ref searchTerm, value))
                {
                    CheckSearch();
                }

            }
        }

        public MvxAsyncCommand<PostModel> ShowVideoCommand => new MvxAsyncCommand<PostModel>(async post => {
            await NavigationService.Navigate<VideoDisplayViewModel, PostModel>(post);
        });

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        private async void CheckSearch()
        {
            if (!isSearching)
            {
                isSearching = true;
                currentSearchTerm = SearchTerm;
                await Task.Delay(TimeSpan.FromSeconds(1));
                isSearching = false;
                if(currentSearchTerm == SearchTerm)
                {
                    await DoSearchAsync();
                } else
                {
                    CheckSearch();
                }
            }
        }

        private async Task DoSearchAsync()
        {
            IsLoading = true;
            var posts = await postService.Search(SearchTerm,1).ConfigureAwait(false);
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
            {
                if(posts != null)
                {
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
