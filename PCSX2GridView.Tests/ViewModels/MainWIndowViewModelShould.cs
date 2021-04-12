namespace PCSX2GridView.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Microsoft.Extensions.FileProviders;
    using Moq;
    using NUnit.Framework;
    using PCSX2GridView.Backend;
    using PCSX2GridView.ViewModels;

    [TestFixture]
    public class MainWindowViewModelShould
    {
        private readonly Fixture fixture = new ();
        private Mock<ICoverArtService> artService;
        private Mock<IGameMediaService> mediaService;

        [SetUp]
        public void Setup()
        {
            this.artService = new Mock<ICoverArtService>();
            this.mediaService = new Mock<IGameMediaService>();

            this.mediaService.Setup(ms => ms.Fetch()).ReturnsAsync(new List<IFileInfo>());
            this.artService.Setup(ass => ass.Fetch()).ReturnsAsync(new List<IFileInfo>());
        }

        [Test]
        public void Be_Of_Type_ViewModelBase()
        {
            // var sut = new MainWindowViewModel();
            var sut = this.CreateViewModel();
            Assert.That(sut, Is.InstanceOf<ViewModelBase>());
        }

        [Test]
        public void Load_Library()
        {
            var files = new List<IFileInfo>
            {
                this.CreateFile("iso"),
            };

            this.mediaService.Setup(ms => ms.Fetch()).ReturnsAsync(files);

            var sut = this.CreateViewModel();

            Assert.That(sut.Games.Count(game => game.GameName == files.First().Name), Is.EqualTo(1));
        }

        [Test]
        public void Load_Art()
        {
            var file = this.CreateFile("iso");

            var artFile = new Mock<IFileInfo>();
            artFile.Setup(m => m.Name).Returns(file.Name.Replace("iso", "jpg"));

            var games = new List<IFileInfo> { file };
            var art = new List<IFileInfo> { artFile.Object };

            this.mediaService.Setup(ms => ms.Fetch()).ReturnsAsync(games);
            this.artService.Setup(ass => ass.Fetch()).ReturnsAsync(art);

            var sut = this.CreateViewModel();

            Assert.That(sut.Games.Count(game => game.GameName == games.First().Name), Is.EqualTo(1));
            StringAssert.Contains(art.First().Name, sut.Games.First().CoverArt);
        }

        [Test]
        public void Refresh_Loads_Games_And_Art()
        {
            var file = this.CreateFile("iso");

            var artFile = new Mock<IFileInfo>();
            artFile.Setup(m => m.Name).Returns(file.Name.Replace("iso", "jpg"));

            var games = new List<IFileInfo> { file };
            var art = new List<IFileInfo> { artFile.Object };

            this.mediaService.Setup(ms => ms.Fetch()).ReturnsAsync(games);
            this.artService.Setup(ass => ass.Fetch()).ReturnsAsync(art);

            var sut = this.CreateViewModel();
            sut.Games.Clear();

            sut.OnClickCommand();

            Assert.That(sut.Games.Count(game => game.GameName == games.First().Name), Is.EqualTo(1));
            StringAssert.Contains(art.First().Name, sut.Games.First().CoverArt);
        }

        private MainWindowViewModel CreateViewModel()
        {
            return new MainWindowViewModel(this.mediaService.Object, this.artService.Object);
        }

        private IFileInfo CreateFile(string extension)
        {
            var file = new Mock<IFileInfo>();
            file.Setup(m => m.Name).Returns($"{this.fixture.Create<string>()}.{extension}");
            return file.Object;
        }
    }
}