using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.ViewModels;
using Visib.Mobile.Controls;

namespace Visib.Mobile.ViewModels.Models
{
    public class PostModel : MvxNotifyPropertyChanged
    {
        private bool hasUserApplause;
        private int applauseCount;
        private int viewCount;

        public Guid Id { get; set; }
        public string VideoSource { get; set; }
        public string VideoCover { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string UserPicture { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool ViewedByUser { get; set; }
        public List<string> Tags { get; set; }
        public string Category { get; set; }
        public int ViewCount { get => viewCount; set => SetProperty(ref viewCount,value); }
        public int ApplauseCount { get => applauseCount; set => SetProperty(ref applauseCount, value); }
        public int CommentCount { get; set; }
        public DateTime CreationDate { get; set; }
        public VideoSourceType SourceType { get; set; }
        public bool HasUserApplause { get => hasUserApplause; set => SetProperty(ref hasUserApplause, value); }

        public string TagString
        {
            get
            {
                return Tags != null ? string.Join(" ", Tags.Select(tag => $"#{tag.ToUpper()}")) : string.Empty;
            }
        }

        public Guid CategoryId { get; set; }
    }
}
