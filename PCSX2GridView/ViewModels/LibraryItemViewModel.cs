namespace PCSX2GridView.ViewModels
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Media.Imaging;
    using ReactiveUI;

    public class LibraryItemViewModel : ViewModelBase
    {
        private Bitmap? cover;

        private string? gameName;

        public string GameName
        {
            get => this.gameName ?? string.Empty;
            set => this.gameName = value.Split('(').First();
        }

        public string? PhysicalPath { get; init; }

        public string? CoverArt { get; init; }

        public Bitmap? Cover
        {
            get => this.cover;
            private set => this.RaiseAndSetIfChanged(ref this.cover, value);
        }

        private string CachePath => $"./Cache/{this.GameName}";

        public async Task LoadCoverAsync()
        {
            await using var imageStream = this.LoadCoverBitmap();
            if (imageStream != null)
            {
                this.Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            }
        }

        public void OnClickCommand()
        {
            var command = $"PCSX2 \"{this.PhysicalPath}\" --nogui &";
            //// Console.WriteLine(command);

            command = command.Replace("\"", "\"\"");

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                },
            };

            proc.Start();
        }

        private Stream? LoadCoverBitmap()
        {
            if (File.Exists(this.CachePath + ".bmp"))
            {
                return File.OpenRead(this.CachePath + ".bmp");
            }

            if (!string.IsNullOrWhiteSpace(this.CoverArt))
            {
                return File.OpenRead(this.CoverArt);
            }

            return null;
        }
    }
}