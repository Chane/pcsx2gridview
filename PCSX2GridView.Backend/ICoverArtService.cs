namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;

    public interface ICoverArtService
    {
        Task<IList<IFileInfo>> Fetch();
    }
}