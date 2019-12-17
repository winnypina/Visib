using MvvmCross.ViewModels;
using Plugin.ContactService.Shared;

namespace Visib.Mobile.ViewModels.Models
{
    public class ContactModel : MvxNotifyPropertyChanged
    {
        private bool isSelected;

        public Contact Contact { get; set; }
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected,value); }
    }
}
