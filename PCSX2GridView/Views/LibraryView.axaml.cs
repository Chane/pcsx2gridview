namespace PCSX2GridView.Views
{
    using System.Diagnostics.CodeAnalysis;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    [ExcludeFromCodeCoverage(Justification = "Avalonia Scaffolding")]
    public class LibraryView : UserControl
    {
        public LibraryView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}