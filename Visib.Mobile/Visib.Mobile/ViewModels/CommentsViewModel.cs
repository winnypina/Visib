using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Visib.Mobile.Services;
using Visib.Mobile.Services.Requests;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.ViewModels
{
    public class CommentsViewModel : MvxNavigationViewModel<PostModel>
    {
        private readonly IPostService _postService;

        private PostModel _currentPost;
        private string _commentText;
        private bool _canPublish;
        private bool isLoading;

        public CommentsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService) : base(logProvider, navigationService)
        {
            _postService = postService;
            Comments = new MvxObservableCollection<CommentModel>();
        }

        public override void Prepare(PostModel parameter)
        {
            _currentPost = parameter;
        }

        public MvxObservableCollection<CommentModel> Comments { get; }

        public MvxAsyncCommand RefreshCommand => new MvxAsyncCommand(LoadComments);

        public MvxAsyncCommand CreateCommentCommand => new MvxAsyncCommand(ExecuteCreateCommentCommand, CanExecuteCreateCommentCommand);


        public string CommentText
        {
            get => _commentText;
            set
            {
                SetProperty(ref _commentText, value);
                if (!string.IsNullOrEmpty(value))
                {
                    CanPublish = true;
                }
                else
                {
                    CanPublish = false;
                }
                CreateCommentCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanExecuteCreateCommentCommand()
        {
            return !string.IsNullOrEmpty(CommentText);
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading,value); }

        private async Task ExecuteCreateCommentCommand()
        {
            IsLoading = true;

            await _postService.CreateComment(new CommentRequest
            {
                Description = CommentText,
                PostId = _currentPost.Id
            }).ConfigureAwait(false);

            await LoadComments();
            CommentText = string.Empty;
            IsLoading = false;
        }

        public bool CanPublish
        {
            get => _canPublish;
            set => SetProperty(ref _canPublish, value);
        }

        public override async Task Initialize()
        {
            await LoadComments();
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        private async Task LoadComments()
        {
            IsLoading = true;
            Comments.Clear();
            var commentsList = new List<CommentModel>();

            var comments = await _postService.GetComments(_currentPost.Id).ConfigureAwait(false);
            if (comments != null)
            {
                foreach (var commentRequest in comments.OrderByDescending(n => n.PublishDate))
                {
                    var span = (DateTime.UtcNow - commentRequest.PublishDate);

                    var since = string.Empty;

                    if (span.Days > 0)
                    {
                        since += $"{span.Days}d";
                    }
                    else
                    {
                        if (span.Hours > 0)
                        {
                            since += $"{span.Hours}h";
                        }
                        else
                        {
                            if (span.Minutes > 0)
                            {
                                since += $"{span.Minutes}m";
                            }
                            else
                            {
                                since += $"{span.Seconds}s";
                            }
                        }
                    }

                    commentsList.Add(new CommentModel
                    {
                        Description = commentRequest.Description,
                        AppUserId = commentRequest.AppUserId,
                        Id = commentRequest.Id,
                        PostId = commentRequest.PostId,
                        PublishDate = commentRequest.PublishDate,
                        AppUsername = commentRequest.AppUsername,
                        Since = since,
                        UserPicture = ApiService.MediaBaseAddress + $"{commentRequest.AppUserId}.png"
                    });
                }
                if (Comments.Count > 0)
                {
                    Comments.RemoveRange(0, Comments.Count);
                }
                Comments.AddRange(commentsList);
            }


            IsLoading = false;
        }
    }
}