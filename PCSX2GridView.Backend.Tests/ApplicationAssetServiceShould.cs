namespace PCSX2GridView.Backend.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Microsoft.Extensions.FileProviders;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationAssetServiceShould
    {
        private readonly Mock<ICoverArtService> coverArtService = new ();
        private readonly Mock<IGameMediaService> gameMediaService = new ();
        private readonly Fixture fixture = new Fixture();

        [Test]
        public async Task Return_Game_With_No_Cover_If_No_Match_Found()
        {
            var games = new List<IFileInfo> { this.CreateFile("iso") };
            var art = new List<IFileInfo> { this.CreateFile("jpg") };

            this.gameMediaService.Setup(m => m.Fetch()).ReturnsAsync(games);
            this.coverArtService.Setup(m => m.Fetch()).ReturnsAsync(art);

            var service = this.CreateService();

            var expected = new GameModel { FileName = games.First().Name };

            var result = await service.Fetch();

            Assert.That(result.First().FileName, Is.EqualTo(expected.FileName));
            Assert.That(result.First().CoverArt, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task Return_Game_With_Cover_If_Match_Found()
        {
            var file = this.CreateFile("iso");

            var artFile = new Mock<IFileInfo>();
            artFile.Setup(m => m.Name).Returns(file.Name.Replace("iso", "jpg"));
            var games = new List<IFileInfo> { file };
            var art = new List<IFileInfo> { artFile.Object };

            this.gameMediaService.Setup(m => m.Fetch()).ReturnsAsync(games);
            this.coverArtService.Setup(m => m.Fetch()).ReturnsAsync(art);

            var service = this.CreateService();

            var expected = new GameModel
            {
                FileName = games.First().Name,
                CoverArt = art.First().Name,
            };

            var result = await service.Fetch();

            Assert.That(result.First().FileName, Is.EqualTo(expected.FileName));
            Assert.That(result.First().CoverArt, Is.EqualTo(expected.CoverArt));
        }

        [Test]
        public async Task Return_Games_With_Covers_If_Matches_Found()
        {
            var file = this.CreateFile("iso");

            var artFile = new Mock<IFileInfo>();
            artFile.Setup(m => m.Name).Returns(file.Name.Replace("iso", "jpg"));

            var file2 = this.CreateFile("iso");

            var artFile2 = new Mock<IFileInfo>();
            artFile2.Setup(m => m.Name).Returns(file2.Name.Replace("iso", "jpg"));

            var games = new List<IFileInfo> { file, file2 };
            var art = new List<IFileInfo> { artFile.Object, artFile2.Object };

            this.gameMediaService.Setup(m => m.Fetch()).ReturnsAsync(games);
            this.coverArtService.Setup(m => m.Fetch()).ReturnsAsync(art);

            var service = this.CreateService();

            var result = await service.Fetch();

            var expected = new GameModel
            {
                FileName = games.First().Name,
                CoverArt = art.First().Name,
            };

            var expected2 = new GameModel
            {
                FileName = games.Last().Name,
                CoverArt = art.Last().Name,
            };

            Assert.That(result.First().FileName, Is.EqualTo(expected.FileName));
            Assert.That(result.First().CoverArt, Is.EqualTo(expected.CoverArt));
            Assert.That(result.Last().FileName, Is.EqualTo(expected2.FileName));
            Assert.That(result.Last().CoverArt, Is.EqualTo(expected2.CoverArt));
        }

        private ApplicationAssetService CreateService()
        {
            return new ApplicationAssetService(this.gameMediaService.Object, this.coverArtService.Object);
        }

        private IFileInfo CreateFile(string extension)
        {
            var file = new Mock<IFileInfo>();
            file.Setup(m => m.Name).Returns($"{this.fixture.Create<string>()}.{extension}");
            return file.Object;
        }
    }
}