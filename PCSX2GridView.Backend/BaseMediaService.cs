namespace PCSX2GridView.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;

    public abstract class BaseMediaService
    {
        protected readonly IFileProvider Provider;

        protected BaseMediaService(IFileProvider provider)
        {
            this.Provider = provider;
        }

        public abstract Task<IList<IFileInfo>> Fetch();

        protected Task<IList<IFileInfo>> FetchMedia(string extension)
        {
            var files = new List<IFileInfo>();

            var allFiles = this.Provider.GetDirectoryContents(string.Empty);

            if (allFiles.Exists)
            {
                files.AddRange(allFiles.Where(f => f.Name.EndsWith(extension, StringComparison.OrdinalIgnoreCase)));
            }

            return Task.FromResult((IList<IFileInfo>)files);
        }
    }
}