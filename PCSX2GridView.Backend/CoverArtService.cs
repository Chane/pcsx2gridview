namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
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
    }
}