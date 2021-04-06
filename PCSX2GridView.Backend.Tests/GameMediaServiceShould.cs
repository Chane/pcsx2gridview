namespace PCSX2GridView.Backend.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Microsoft.Extensions.FileProviders;
    using Moq;
    using NUnit.Framework;

    public class GameMediaServiceShould
    {
        private readonly Fixture fixture = new Fixture();

        [Test]
        public async Task Return_Nothing_When_Directory_Contents_Is_Empty()
        {
            var directoryContents = new Mock<IDirectoryContents>();
            directoryContents.Setup(m => m.Exists).Returns(false);

            var provider = new Mock<IFileProvider>();
            provider.Setup(m => m.GetDirectoryContents(string.Empty))
                .Returns(directoryContents.Object);

            var service = new GameMediaService(provider.Object);

            var contents = await service.Fetch();

            Assert.That(contents.Count, Is.Zero);
        }

        [Test]
        public async Task Return_Directory_Contents()
        {
            var files = new List<IFileInfo> { this.CreateFile("iso") };

            var directoryContents = new Mock<IDirectoryContents>();
            directoryContents.Setup(m => m.Exists).Returns(true);
            directoryContents.Setup(m => m.GetEnumerator())
                .Returns(files.GetEnumerator());

            var provider = new Mock<IFileProvider>();
            provider.Setup(m => m.GetDirectoryContents(string.Empty))
                .Returns(directoryContents.Object);

            var service = new GameMediaService(provider.Object);

            var contents = await service.Fetch();

            StringAssert.EndsWith(".iso", contents.First().Name);
        }

        [Test]
        public async Task Return_Just_Games()
        {
            var files = new List<IFileInfo>
            {
                this.CreateFile("iso"),
                this.CreateFile("jpg"),
            };

            var directoryContents = new Mock<IDirectoryContents>();
            directoryContents.Setup(m => m.Exists).Returns(true);
            directoryContents.Setup(m => m.GetEnumerator())
                .Returns(files.GetEnumerator());

            var provider = new Mock<IFileProvider>();
            provider.Setup(m => m.GetDirectoryContents(string.Empty))
                .Returns(directoryContents.Object);

            var service = new GameMediaService(provider.Object);

            var contents = await service.Fetch();

            Assert.That(contents.Count, Is.EqualTo(1));
            StringAssert.EndsWith(".iso", contents.First().Name);
        }

        [Test]
        public async Task Return_Just_Games_Ignoring_Case()
        {
            var files = new List<IFileInfo>
            {
                this.CreateFile("ISO"),
                this.CreateFile("jpg"),
            };

            var directoryContents = new Mock<IDirectoryContents>();
            directoryContents.Setup(m => m.Exists).Returns(true);
            directoryContents.Setup(m => m.GetEnumerator())
                .Returns(files.GetEnumerator());

            var provider = new Mock<IFileProvider>();
            provider.Setup(m => m.GetDirectoryContents(string.Empty))
                .Returns(directoryContents.Object);

            var service = new GameMediaService(provider.Object);

            var contents = await service.Fetch();

            Assert.That(contents.Count, Is.EqualTo(1));
            StringAssert.EndsWith(".ISO", contents.First().Name);
        }

        private IFileInfo CreateFile(string extension)
        {
            var file = new Mock<IFileInfo>();
            file.Setup(m => m.Name).Returns($"{this.fixture.Create<string>()}.{extension}");
            return file.Object;
        }
    }
}