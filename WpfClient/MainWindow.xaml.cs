using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfClient.Helpers;
using WpfClient.Pages;
using WpfClient.Services;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly IApplicationContext _applicationContext;
        private readonly AccountServices _accountServices;

        public MainWindow(IApplicationContext applicationContext, AccountServices accountServices)
        {
            InitializeComponent();
            _applicationContext = applicationContext;
            _accountServices = accountServices;
        }

        private void MainFrameLoaded(object sender, RoutedEventArgs e)
        {
            var homeView = _applicationContext.ApplicationHost.Services.GetRequiredService<AccountManagerView>();
            MainFrame.Navigate(homeView);
        }

    }
}
