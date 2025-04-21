using System.Windows.Controls;
using Duru.UI.ViewModels.Pages;

namespace Duru.UI.Pages;

public partial class RoomManagementPage : Page
{
    public RoomManagementPage(RoomManagementViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
    }
}