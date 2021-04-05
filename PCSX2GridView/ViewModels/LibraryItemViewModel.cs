namespace PCSX2GridView.ViewModels
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Media.Imaging;
    using ReactiveUI;

    public class LibraryItemViewModel : ViewModelBase
    {
        private Bitmap? cover;

        private string gameName;

        public string GameName
        {
            get => this.gameName;
            set => this.gameName = value.Split('(').First();
        }

        public string PhysicalPath { get; init; }

        public string CoverArt { get; init; }

        public Bitmap? Cover
        {
            get => this.cover;
            private set => this.RaiseAndSetIfChanged(ref this.cover, value);
        }

        private string CachePath => $"./Cache/{this.GameName}";

        public async Task LoadCoverAsync()
        {
            await using (var imageStream = this.LoadCoverBitmapAsync())
            {
                this.Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            }
        }

        public Stream LoadCoverBitmapAsync()
        {
            if (File.Exists(this.CachePath + ".bmp"))
            {
                return File.OpenRead(this.CachePath + ".bmp");
            }
            else
            {
                return File.OpenRead(this.CoverArt);
            }
        }
    }
}