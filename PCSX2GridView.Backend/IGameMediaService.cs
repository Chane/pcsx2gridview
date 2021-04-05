namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    public interface IGameMediaService
    {
        IList<IFileInfo> Fetch();
    }
}