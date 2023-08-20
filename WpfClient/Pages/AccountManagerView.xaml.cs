using System.Windows.Controls;
using WpfClient.ViewModels;

namespace WpfClient.Pages
{
    /// <summary>
    /// Interaction logic for AccountManagerView.xaml
    /// </summary>
    public partial class AccountManagerView : Page
    {
        public AccountManagerView(AccountManagerViewModel? viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
