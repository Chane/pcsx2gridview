namespace PCSX2GridView.Backend.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoFixture;
    using Microsoft.Extensions.FileProviders;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CoverArtServiceShould
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

            var service = new CoverArtService(provider.Object);

            var contents = await service.Fetch();

            Assert.That(contents.Count, Is.Zero);
        }

        [Test]
        public async Task Return_Directory_Contents()
        {
            var files = new List<IFileInfo> { this.CreateFile("jpg") };

            var directoryContents = new Mock<IDirectoryContents>();
            directoryContents.Setup(m => m.Exists).Returns(true);
            directoryContents.Setup(m => m.GetEnumerator())
                .Returns(files.GetEnumerator());

            var provider = new Mock<IFileProvider>();
            provider.Setup(m => m.GetDirectoryContents(string.Empty))
                .Returns(directoryContents.Object);

            var service = new CoverArtService(provider.Object);

            var contents = await service.Fetch();

            StringAssert.EndsWith(".jpg", contents.First().Name);
        }

        [Test]
        public async Task Return_Just_Pictures()
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

            var service = new CoverArtService(provider.Object);

            var contents = await service.Fetch();

            Assert.That(contents.Count, Is.EqualTo(1));
            StringAssert.EndsWith(".jpg", contents.First().Name);
        }

        [Test]
        public async Task Return_Just_Pictures_Ignoring_Case()
        {
            var files = new List<IFileInfo>
            {
                this.CreateFile("ISO"),
                this.CreateFile("JPG"),
            };

            var directoryContents = new Mock<IDirectoryContents>();
            directoryContents.Setup(m => m.Exists).Returns(true);
            directoryContents.Setup(m => m.GetEnumerator())
                .Returns(files.GetEnumerator());

            var provider = new Mock<IFileProvider>();
            provider.Setup(m => m.GetDirectoryContents(string.Empty))
                .Returns(directoryContents.Object);

            var service = new CoverArtService(provider.Object);

            var contents = await service.Fetch();

            Assert.That(contents.Count, Is.EqualTo(1));
            StringAssert.EndsWith(".JPG", contents.First().Name);
        }

        [Test]
        public void Load_Cover_If_Found()
        {
            using (var expectedStream = new MemoryStream(Encoding.UTF8.GetBytes("streamedText")))
            {
                var file = this.fixture.Create<string>();
                var fileInfo = new Mock<IFileInfo>();
                fileInfo.Setup(m => m.CreateReadStream())
                        .Returns(expectedStream);

                var provider = new Mock<IFileProvider>();
                provider.Setup(m => m.GetFileInfo(file))
                        .Returns(fileInfo.Object);

                var service = new CoverArtService(provider.Object);
                var stream = service.LoadCoverBitmap(file);

                Assert.That(stream, Is.EqualTo(expectedStream));
            }
        }

        [TestCase("")]
        [TestCase(null)]
        public void Return_Null_If_No_Cover_Found(string coverArt)
        {
            var provider = new Mock<IFileProvider>();

            var service = new CoverArtService(provider.Object);
            var stream = service.LoadCoverBitmap(coverArt);

            Assert.That(stream, Is.Null);
        }

        private IFileInfo CreateFile(string extension)
        {
            var file = new Mock<IFileInfo>();
            file.Setup(m => m.Name).Returns($"{this.fixture.Create<string>()}.{extension}");
            return file.Object;
        }
    }
}