namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    public interface ICoverArtService
    {
        IList<IFileInfo> Fetch();
    }
}