using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using WpfClient.Helpers;
using WpfClient.Pages;
using WpfClient.Services;
using WpfClient.ViewModels;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public IHost ApplicationHost { get; private set; }
        public IConfigurationRoot Config { get; private set; }

        public App()
        {
            // Dummies to allow non nullable ref type
            ApplicationHost = Host.CreateDefaultBuilder().Build();
            Config = new ConfigurationBuilder().Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"));
            var builder = new ConfigurationBuilder();
            Config = builder.Build();

            ApplicationHost = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                services.AddSingleton<IApplicationContext, ApplicationContext>();
                services.AddTransient<MainWindow>();

                //views
                services.AddTransient<AccountManagerView>();
                services.AddTransient<AccountView>();
                services.AddTransient<NoDataView>();

                //viewmodels 
                services.AddTransient<AccountManagerViewModel>();
                services.AddTransient<AccountViewModel>();

                //services
                services.AddSingleton<AccountServices>();
            }).Build();

            var mainWindow = ApplicationHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}