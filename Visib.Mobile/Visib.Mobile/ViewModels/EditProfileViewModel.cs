using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels
{
    public class EditProfileViewModel : MvxNavigationViewModel
    {
        private bool isLoading;
        private string name;
        private string country;
        private string email;
        private string phone;
        private string about;
        private string profilePicture;
        private string videoSource;
        private readonly ILoginService loginService;
        private readonly IMediaService mediaService;

        public EditProfileViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService, IMediaService mediaService) : base(logProvider, navigationService)
        {
            this.loginService = loginService;
            this.mediaService = mediaService;
        }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Country { get => country; set => SetProperty(ref country, value); }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string Phone { get => phone; set => SetProperty(ref phone, value); }
        public string About { get => about; set => SetProperty(ref about, value); }

        public string ProfilePicture { get => profilePicture; set => SetProperty(ref profilePicture, value); }

        public string VideoSource { get => videoSource; set => SetProperty(ref videoSource,value); }

        public override void Prepare()
        {
            Name = loginService.Account.Name;
            Country = loginService.Account.Country;
            Email = loginService.Account.Email;
            Phone = loginService.Account.MobilePhone;
            About = loginService.Account.About;
            ProfilePicture = $"https://s3.us-east-2.amazonaws.com/visib/{loginService.Account.Id}.png";
        }

        public MvxAsyncCommand TakeProfilePicture => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayConfirmation("Foto", "Deseja tirar a foto ou utilizar uma da biblioteca?", "Camera", "Biblioteca", async result =>
                  {
                      if(result)
                      {
                          var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                          {
                              SaveToAlbum = true,
                              MaxWidthHeight = 100
                          });

                          if (file != null)
                          {
                              ProfilePicture = file?.Path;
                          }
                      } 
                      else
                      {
                          var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                          {
                              MaxWidthHeight = 100
                          });

                          if (file != null)
                          {
                              ProfilePicture = file?.Path;
                          }
                      }
                  });
            }
            else
            {
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayMessage("Permissão de camera negada", "Por favor verifique as configurações do aplicativo no dispositivo");
                CrossPermissions.Current.OpenAppSettings();
            }
            IsLoading = false;
        });

        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;

            if (!string.IsNullOrEmpty(ProfilePicture) && !ProfilePicture.StartsWith("http", System.StringComparison.Ordinal))
            {
                var image = $"{loginService.Account.Id}.png";
                await mediaService.Upload(File.ReadAllBytes(ProfilePicture), image).ConfigureAwait(false);
                await ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);
            }

            loginService.Account.Name = Name;
            loginService.Account.Email = Email;
            loginService.Account.MobilePhone = phone;
            loginService.Account.About = About;
            loginService.Account.Country = Country;
            await loginService.UpdateAccount(loginService.Account).ConfigureAwait(false);
            await NavigationService.Close(this);
            IsLoading = false;
        });
    }
}
