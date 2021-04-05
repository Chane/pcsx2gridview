namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    public class GameMediaService : BaseMediaService, IGameMediaService
    {
        public GameMediaService(IFileProvider provider)
            : base(provider)
        {
        }

        public override IList<IFileInfo> Fetch()
        {
            return this.FetchMedia("iso");
        }
    }
}