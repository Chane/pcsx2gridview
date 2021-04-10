namespace PCSX2GridView.Views
{
    using System.Diagnostics.CodeAnalysis;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    [ExcludeFromCodeCoverage(Justification = "Avalonia Scaffolding")]
    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}