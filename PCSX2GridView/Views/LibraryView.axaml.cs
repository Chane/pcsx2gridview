namespace PCSX2GridView.Views
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

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