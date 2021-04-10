namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;

    public interface ICoverArtService
    {
        Task<IList<IFileInfo>> Fetch();

        Stream LoadCoverBitmap(string coverArt);
    }
}