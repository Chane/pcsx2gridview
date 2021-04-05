namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    public class CoverArtService : BaseMediaService, ICoverArtService
    {
        public CoverArtService(IFileProvider provider)
            : base(provider)
        {
        }

        public override IList<IFileInfo> Fetch()
        {
            return this.FetchMedia("jpg");
        }
    }
}