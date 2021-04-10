namespace PCSX2GridView.Tests.ViewModels
{
    using AutoFixture;
    using Moq;
    using NUnit.Framework;
    using PCSX2GridView.Backend;
    using PCSX2GridView.ViewModels;

    [TestFixture]
    public class LibraryItemViewModelShould
    {
        private readonly Fixture fixture = new ();

        [Test]
        public void Be_An_Instance_Of_ViewModelBase()
        {
            var model = new LibraryItemViewModel();
            Assert.That(model, Is.InstanceOf<ViewModelBase>());
        }

        [TestCase("BloodRayne (Full Price).iso", ExpectedResult = "BloodRayne")]
        [TestCase("007 - Nightfire (Platinum)", ExpectedResult = "007 - Nightfire")]
        public string GameName_Should_Be_The_Text_Before_A_Bracket(string fileName)
        {
            var model = new LibraryItemViewModel { GameName = fileName };
            return model.GameName;
        }

        [Test]
        public void Start_Process_When_Clicked()
        {
            var processManager = new Mock<IProcessManager>();
            var physicalPath = this.fixture.Create<string>();

            var model = new LibraryItemViewModel(processManager.Object)
            {
                PhysicalPath = physicalPath,
            };

            model.OnClickCommand();

            processManager.Verify(v => v.Start($"PCSX2 \"\"{physicalPath}\"\" --nogui &"), Times.Once);
        }
    }
}