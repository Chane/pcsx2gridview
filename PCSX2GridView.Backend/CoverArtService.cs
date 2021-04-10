namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;

    public class CoverArtService : BaseMediaService, ICoverArtService
    {
        public CoverArtService(IFileProvider provider)
            : base(provider)
        {
        }

        public override async Task<IList<IFileInfo>> Fetch()
        {
            return await this.FetchMedia("jpg")
                .ConfigureAwait(false);
        }

        public Stream LoadCoverBitmap(string coverArt)
        {
            // private string CachePath => $"./Cache/{this.GameName}";
            // if (File.Exists(this.CachePath + ".bmp"))
            // {
            //     return File.OpenRead(this.CachePath + ".bmp");
            // }
            if (!string.IsNullOrWhiteSpace(coverArt))
            {
                return this.Provider
                            .GetFileInfo(coverArt)
                            .CreateReadStream();
            }

            return null;
        }
    }
}