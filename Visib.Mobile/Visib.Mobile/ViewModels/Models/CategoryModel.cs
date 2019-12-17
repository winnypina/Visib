using System;
using MvvmCross.ViewModels;

namespace Visib.Mobile.ViewModels.Models
{
    public class CategoryModel : MvxNotifyPropertyChanged
    {
        private bool isSelectedForPost;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsSelectedForPost { get => isSelectedForPost; set => SetProperty(ref isSelectedForPost, value); }
    }
}
