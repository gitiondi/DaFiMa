using System;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WpfClient.ViewModels;

namespace WpfClient.Helpers
{
    public class ApplicationContext : IApplicationContext
    {
        public IHost ApplicationHost => ((App)System.Windows.Application.Current).ApplicationHost;
        public IConfigurationRoot Config => ((App)System.Windows.Application.Current).Config;
        public MainWindow MainWindow => ((App)System.Windows.Application.Current).MainWindow as MainWindow ?? throw new InvalidOperationException();

        public Frame MainFrame => MainWindow.MainFrame ?? throw new InvalidOperationException("MainFrame not set.");

        public AccountManagerViewModel? AccountManagerViewModel { get; set; } = null;

        public void Dispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        public void DispatchAsync(Action action)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}