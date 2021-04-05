namespace PCSX2GridView.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.FileProviders;

    public abstract class BaseMediaService
    {
        private readonly IFileProvider provider;

        protected BaseMediaService(IFileProvider provider)
        {
            // provider will be a PhysicalFileProvider set to the games directory
            this.provider = provider;
        }

        public abstract IList<IFileInfo> Fetch();

        protected IList<IFileInfo> FetchMedia(string extension)
        {
            var files = new List<IFileInfo>();

            var allFiles = this.provider.GetDirectoryContents(string.Empty);

            if (allFiles.Exists)
            {
                files.AddRange(allFiles.Where(f => f.Name.EndsWith(extension, StringComparison.OrdinalIgnoreCase)));
            }

            return files;
        }
    }
}