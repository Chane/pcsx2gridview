namespace PCSX2GridView.Views
{
    using System;
    using System.IO;
    using System.Linq;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

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