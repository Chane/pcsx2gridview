namespace PCSX2GridView.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Concurrency;
    using Microsoft.Extensions.FileProviders;
    using PCSX2GridView.Backend;
    using ReactiveUI;

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            RxApp.MainThreadScheduler.Schedule(this.LoadLibrary);
        }

        public ObservableCollection<LibraryItemViewModel> Games { get; } = new ();

        private static IEnumerable<LibraryItemViewModel> LoadCached()
        {
            var result = new List<LibraryItemViewModel>();

            var rootFilePath = "/home/chane/Games/ROMs/PlayStation2";
            var rootCoverPath = $"{rootFilePath}/Covers";

            var gameProvider = new PhysicalFileProvider(rootFilePath);
            var gameMediaService = new GameMediaService(gameProvider);

            var artProvider = new PhysicalFileProvider(rootCoverPath);
            var artService = new CoverArtService(artProvider);

            var assetService = new ApplicationAssetService(gameMediaService, artService);

            var games = assetService.Fetch();

            foreach (var game in games)
            {
                var item = new LibraryItemViewModel
                {
                    GameName = game.FileName,
                    PhysicalPath = $"{rootFilePath}/{game.FileName}",
                    CoverArt = $"{rootCoverPath}/{game.CoverArt}",
                };

                result.Add(item);
            }

            return result.OrderBy(g => g.GameName);
        }

        private async void LoadLibrary()
        {
            var items = LoadCached();

            foreach (var item in items)
            {
                this.Games.Add(item);
            }

            this.LoadCovers();
        }

        private async void LoadCovers()
        {
            foreach (var gameViewModel in this.Games.ToList())
            {
                await gameViewModel.LoadCoverAsync();
            }
        }
    }
}