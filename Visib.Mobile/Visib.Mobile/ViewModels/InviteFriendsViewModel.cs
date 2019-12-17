using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Visib.Mobile.Services;
using Visib.Mobile.ViewModels.Models;

namespace Visib.Mobile.ViewModels
{
    public class InviteFriendsViewModel : MvxNavigationViewModel
    {

        private bool isLoading;

        public InviteFriendsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            Contacts = new MvxObservableCollection<ContactModel>();
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public MvxObservableCollection<ContactModel> Contacts { get; }

        public MvxAsyncCommand InviteCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            if (Contacts.Any(n=>n.IsSelected))
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayMessage("Sucesso", "Convites enviados com sucesso");
                await NavigationService.Close(this);
            }
            IsLoading = false;
        });

        public override async Task Initialize()
        {

            IsLoading = true;

            var contactsStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);

            if (contactsStatus != PermissionStatus.Granted)
            {
                var results =
                    await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);
                contactsStatus = results[Permission.Contacts];
            }

            if (contactsStatus == PermissionStatus.Granted)
            {
                var contactList = new List<ContactModel>();
                var contacts = (await Plugin.ContactService.CrossContactService.Current.GetContactListAsync()).Where(n=>!string.IsNullOrEmpty(n.Email)).OrderBy(n => n.Name).ToList();
                contacts.ForEach(contact =>
                {
                    if (string.IsNullOrEmpty(contact.PhotoUri))
                    {
                        contact.PhotoUri = "defaultUser.png";
                    }
                    contactList.Add(new ContactModel { Contact = contact });
                });
                if (Contacts.Count > 0)
                {
                    Contacts.RemoveRange(0, Contacts.Count);
                }
                Contacts.AddRange(contactList);
            }
            else
            {
                await NavigationService.Close(this);
            }
            IsLoading = false;
        }

        public MvxCommand<ContactModel> SelectContactCommand => new MvxCommand<ContactModel>((obj) => obj.IsSelected = !obj.IsSelected);

        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

    }
}
