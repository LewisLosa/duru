using System.Windows;
using Duru.UI.ViewModels;

namespace Duru.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}