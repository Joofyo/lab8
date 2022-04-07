using Avalonia.Controls;
using Avalonia.Interactivity;

namespace KanbanTable.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void ShowAboutWindow(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new AboutView();
            dialogWindow.ShowDialog(this);
        }
    }
}
