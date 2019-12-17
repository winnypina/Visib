using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Visib.Mobile.Services;
using Visib.Mobile.ViewModels.Models;
using Xamarin.Forms;

namespace Visib.Mobile.ViewModels
{
    public class NewPostViewModel : MvxNavigationViewModel
    {
        private string videoSource;
        private bool isLoading;
        private bool isDetails;
        private bool isHashtags;
        private bool isCategories;
        private bool isSearching;
        private string currentCategorySearchTerm;
        private string categorySearchTerm;
        private List<CategoryModel> allCategories = new List<CategoryModel>();
        private string currentHashtag;
        private string selectedHastags;
        private bool hasHashtags;
        private string title;
        private string description;
        private readonly IPostService postService;
        private readonly ICompressionService compressionService;
        private readonly IMediaService mediaService;
        private byte[] video;
        private string loadingMessage;
        private bool isCovers;
        private string selectedCover;
        private bool isMusic;
        private string selectedMusic;
        private bool isText;

        public NewPostViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService, ICompressionService compressionService, IMediaService mediaService) : base(logProvider, navigationService)
        {
            SuggestedTags = new MvxObservableCollection<string>();
            Categories = new MvxObservableCollection<CategoryModel>();
            Covers = new MvxObservableCollection<string>();
            Musics = new MvxObservableCollection<string>();
            this.postService = postService;
            this.compressionService = compressionService;
            this.mediaService = mediaService;
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public bool IsDetails { get => isDetails; set => SetProperty(ref isDetails, value); }
        public bool IsHashtags { get => isHashtags; set => SetProperty(ref isHashtags, value); }
        public bool IsCategories { get => isCategories; set => SetProperty(ref isCategories, value); }
        public bool HasHashtags { get => hasHashtags; set => SetProperty(ref hasHashtags, value); }
        public string Title { get => title; set => SetProperty(ref title, value); }
        public string Description { get => description; set => SetProperty(ref description, value); }

        public string SelectedHashtags
        {
            get => selectedHastags;
            set
            {
                SetProperty(ref selectedHastags, value);
            }
        }
        public string CurrentHashtag
        {
            get => currentHashtag;
            set
            {
                if (!value.StartsWith("#", StringComparison.Ordinal))
                {
                    value = "#" + value;
                }
                SetProperty(ref currentHashtag, value);
                HasHashtags = !string.IsNullOrEmpty(value) && value != "#";
            }
        }


        public string CategorySearchTerm
        {
            get => categorySearchTerm;
            set
            {
                if (SetProperty(ref categorySearchTerm, value))
                {
                    CheckCategorySearch();
                }
            }
        }

        public bool IsMusic { get => isMusic; set => SetProperty(ref isMusic, value); }

        public string VideoSource { get => videoSource; set => SetProperty(ref videoSource, value); }

        public MvxObservableCollection<string> SuggestedTags { get; }

        public MvxObservableCollection<CategoryModel> Categories { get; }

        public MvxObservableCollection<string> Musics { get; }

        public MvxCommand CleanMusicCommand => new MvxCommand(()=>
        {
            SelectedMusic = null;
            VideoSource = Path.Combine(Path.GetDirectoryName(VideoSource), "compressed.mp4");
        });

        public MvxCommand NextCommand => new MvxCommand(() =>
        {
            if (IsCovers)
            {
                IsCovers = false;
                IsMusic = true;
                FillMusics();
                return;
            }

            if (IsMusic)
            {
                IsDetails = true;
                IsMusic = false;
                FillFonts();
                return;
            }
        });

        private void FillFonts()
        {

        }

        public bool IsText { get => isText; set => SetProperty(ref isText,value); }

        private void FillMusics()
        {
            var assembly = typeof(App).Assembly;

            var musics = new List<string>();

            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.Contains(".mp3"))
                {
                    musics.Add(Path.GetFileNameWithoutExtension(res.Replace("Visib.Mobile.Musics.", "")));
                }
            }
            if (Musics.Count > 0)
            {
                Musics.RemoveRange(0, Musics.Count);
            }
            Musics.AddRange(musics);
        }

        public string SelectedMusic
        {
            get => selectedMusic;
            set
            {
                SetProperty(ref selectedMusic, value);
                if(value != null)
                {
                    ApplyMusic();
                }
            }
        }

        private async void ApplyMusic()
        {
            IsLoading = true;
            var assembly = typeof(App).Assembly;
            var musicResource = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().Single(n => n.Contains(SelectedMusic)));
            var musicData = new byte[musicResource.Length];
            musicResource.Read(musicData, 0, musicData.Length);
            var musicPath = Path.Combine(Path.GetDirectoryName(VideoSource), "selectedmusic.mp3");
            if (File.Exists(musicPath))
            {
                File.Delete(musicPath);
            }
            File.WriteAllBytes(musicPath, musicData);
            LoadingMessage = "Adicionando música";
            var compressed = Path.Combine(Path.GetDirectoryName(VideoSource), "compressed.mp4");
            VideoSource = string.Empty;
            await compressionService.AddMusic(compressed, musicPath, () =>
            {
                VideoSource = Path.Combine(Path.GetDirectoryName(_originalFile), "videowithmusic.mp4");
                IsLoading = false;
            });
        }

        public MvxAsyncCommand PickMusicCommand => new MvxAsyncCommand(async ()=>
        {
            var fileData = await CrossFilePicker.Current.PickFile( new[] { "audio/mpeg" });
            if (fileData == null)
                return; // user canceled file picking

            SelectedMusic = fileData.FileName;
        });

        public MvxAsyncCommand GoBackCommand => new MvxAsyncCommand(async () =>
        {
            if (IsCovers)
            {
                _isTakingPicture = false;
                await TakePicture();
                IsCovers = false;
                return;
            }

            if (IsMusic)
            {
                IsCovers = true;
                IsMusic = false;
                return;
            }

            if(IsDetails)
            {
                IsDetails = false;
                IsMusic = true;
                return;
            }

            IsDetails = true;
            IsHashtags = false;
            IsCategories = false;
        });

        public MvxAsyncCommand ShowHashtagsCommand => new MvxAsyncCommand(async () =>
        {
            IsDetails = false;
            IsHashtags = true;
            IsCategories = false;
            IsLoading = true;
            await GetTags();
            IsLoading = false;
        });

        public MvxAsyncCommand ShowCategoriesCommand => new MvxAsyncCommand(async () =>
        {
            IsDetails = false;
            IsHashtags = false;
            IsCategories = true;
            IsLoading = true;
            allCategories = (await postService.GetCategories().ConfigureAwait(false)).ToList();
            FillCategories(allCategories);
            IsLoading = false;
        });

        public MvxCommand<string> AddHashtagCommand => new MvxCommand<string>(hashtag =>
        {
            if (string.IsNullOrEmpty(SelectedHashtags))
            {
                SelectedHashtags += hashtag;
                CurrentHashtag = string.Empty;
                return;
            }
            if (!SelectedHashtags.Contains(hashtag))
            {
                SelectedHashtags += $" {hashtag}";
                CurrentHashtag = string.Empty;
            }
        });

        public MvxCommand<CategoryModel> SelectCategoryCommand => new MvxCommand<CategoryModel>(selectedCategory =>
        {
            foreach (var category in allCategories)
            {
                category.IsSelectedForPost = category.Id == selectedCategory.Id;
            }
            foreach (var category in Categories)
            {
                category.IsSelectedForPost = category.Id == selectedCategory.Id;
            }
        });

        private bool _isTakingPicture;

        private async void CheckCategorySearch()
        {
            if (!isSearching)
            {
                isSearching = true;
                currentCategorySearchTerm = CategorySearchTerm;
                await Task.Delay(TimeSpan.FromSeconds(1));
                isSearching = false;
                if (currentCategorySearchTerm == CategorySearchTerm)
                {
                    DoCategorySearch();
                }
                else
                {
                    CheckCategorySearch();
                }
            }
        }

        private void FillCategories(IEnumerable<CategoryModel> categories)
        {
            if (Categories.Count > 0)
            {
                Categories.RemoveRange(0, Categories.Count);
            }
            Categories.AddRange(categories.OrderBy(n => n.Name));
        }

        private void DoCategorySearch()
        {
            FillCategories(allCategories.Where(n => n.Name.Contains(CategorySearchTerm)));
        }

        private async Task GetTags()
        {
            var tags = await postService.GetTags().ConfigureAwait(false);
            if (SuggestedTags.Count > 0)
            {
                SuggestedTags.RemoveRange(0, SuggestedTags.Count);
            }
            if (tags != null)
            {
                SuggestedTags.AddRange(tags);
            }
        }

        public override async void ViewAppeared()
        {
            await TakePicture();
        }

        public string LoadingMessage { get => loadingMessage; set => SetProperty(ref loadingMessage, value); }

        public async Task TakePicture()
        {
            if (!_isTakingPicture)
            {
                _isTakingPicture = true;
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
                    MediaFile file = null;
                    await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayConfirmation("Vídeo", "Deseja filmar ou utilizar um vídeo da biblioteca?", "Camera", "Biblioteca", async result =>
                    {
                        if(result)
                        {
                            file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                            {
                                SaveToAlbum = true,
                                CompressionQuality = 0,
                                DesiredLength = TimeSpan.FromMinutes(5),
                                Quality = Plugin.Media.Abstractions.VideoQuality.High
                            });
                        } else
                        {
                            file = await CrossMedia.Current.PickVideoAsync();
                        }

                        if (file != null)
                        {
                            var destination = Path.Combine(Path.GetDirectoryName(file.Path), "compressed.mp4");

                            //var info = new FileInfo(destination);
                            //Console.WriteLine(info.Length);
                            //if(File.Exists(destination))
                            //{
                            //    VideoSource = destination;
                            //} else
                            //{
                            //    await compressionService.Compress(file.Path, destination, () => {
                            //        Console.WriteLine();
                            //    }).ConfigureAwait(false);

                            //}
                            LoadingMessage = "Comprimindo o vídeo";
                            Console.WriteLine("OLD SIZE:" + new FileInfo(file.Path).Length);
                            _originalFile = file.Path;
                            await compressionService.Compress(file.Path, destination, async () =>
                            {
                                VideoSource = destination;
                                IsCovers = true;
                                LoadingMessage = "Carregando Capas";
                                await compressionService.GenerateCovers(destination, () =>
                                {
                                    //Covers.Clear();
                                    var files = Directory.GetFiles(Path.GetDirectoryName(destination)).Where(n => n.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase));
                                    if (Covers.Count > 0)
                                    {
                                        Covers.RemoveRange(0, Covers.Count);
                                    }
                                    Covers.AddRange(files);
                                    IsLoading = false;
                                });
                                Console.WriteLine("NEW SIZE:" + new FileInfo(destination).Length);
                            }).ConfigureAwait(false);
                        }
                    });
                    

                    
                }
                else
                {
                    await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayMessage("Permissão de camera negada", "Por favor verifique as configurações do aplicativo no dispositivo");
                    CrossPermissions.Current.OpenAppSettings();
                    IsLoading = false;
                    IsDetails = true;
                }
            }
        }

        private string _originalFile;

        public string SelectedCover { get => selectedCover; set => SetProperty(ref selectedCover, value); }

        public MvxObservableCollection<string> Covers { get; }
        public bool IsCovers { get => isCovers; set => SetProperty(ref isCovers, value); }

        public MvxAsyncCommand PublishCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            LoadingMessage = "Publicando";
            var userInteractionService = Mvx.IoCProvider.Resolve<IUserInteractionService>();

            if (string.IsNullOrEmpty(Title))
            {
                await userInteractionService.DisplayMessage("Campos obrigatórios", "Por favor preencha o título do seu post.").ConfigureAwait(false);
                IsLoading = false;
                return;
            }

            if (!Categories.Any(n => n.IsSelectedForPost))
            {
                await userInteractionService.DisplayMessage("Campos obrigatórios", "Por favor selecione uma categoria.").ConfigureAwait(false);
                IsLoading = false;
                return;
            }

            var destination = VideoSource;
            var data = File.ReadAllBytes(destination);
            video = data;

            var post = new PostModel
            {
                Title = Title,
                Description = Description,
                Tags = SelectedHashtags?.Split(' ').ToList(),
                CategoryId = allCategories.Single(n => n.IsSelectedForPost).Id,
                Id = Guid.NewGuid()
            };

            if (await postService.Publish(post, video).ConfigureAwait(false))
            {
                await mediaService.Upload(File.ReadAllBytes(SelectedCover), $"{post.Id}.jpg");
                await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(HomeViewModel)));
            }

            IsLoading = false;
            IsDetails = false;
        });
    }
}
