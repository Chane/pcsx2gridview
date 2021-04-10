namespace PCSX2GridView.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Media.Imaging;
    using PCSX2GridView.Backend;
    using ReactiveUI;

    public class LibraryItemViewModel : ViewModelBase
    {
        private readonly IProcessManager processManager;

        private Bitmap? cover;

        private string? gameName;

        public LibraryItemViewModel()
            : this(new ProcessManager())
        {
        }

        public LibraryItemViewModel(IProcessManager processManager)
        {
            this.processManager = processManager;
        }

        public string GameName
        {
            get => this.gameName ?? string.Empty;
            set => this.gameName = value.Split('(')
                                        .First()
                                        .Trim();
        }

        public string? PhysicalPath { get; init; }

        public string? CoverArt { get; init; }

        public IApplicationAssetService? AssetService { get; init; }

        public Bitmap? Cover
        {
            get => this.cover;
            private set => this.RaiseAndSetIfChanged(ref this.cover, value);
        }

        public async Task LoadCoverAsync()
        {
            await using var imageStream = this.AssetService?.LoadCoverBitmap(this.CoverArt);
            if (imageStream != null)
            {
                this.Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            }
        }

        public void OnClickCommand()
        {
            var command = $"PCSX2 \"{this.PhysicalPath}\" --nogui &";
            command = command.Replace("\"", "\"\"");

            this.processManager.Start(command);
        }
    }
}