using System;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WpfClient.ViewModels;

namespace WpfClient.Helpers
{
    public interface IApplicationContext
    {
        /// <summary>
        /// Provides access to the ApplicationHost.
        /// </summary>
        IHost ApplicationHost { get; }

        /// <summary>
        /// Provides access to the Config.
        /// </summary>
        IConfigurationRoot Config { get; }

        /// <summary>
        /// Provides access to the MainWindow.
        /// </summary>
        WpfClient.MainWindow MainWindow { get; }

        /// <summary>
        /// Provides access to the MainFrame. Shortcut for MainWindow.MainFrame.
        /// </summary>
        Frame MainFrame { get; }

        /// <summary>
        /// Provides access to the AccountManagerViewModel
        /// </summary>
        AccountManagerViewModel? AccountManagerViewModel { get; set; }

        /// <summary>
        /// Dispatch the workload to the UI thread.
        /// </summary>
        /// <param name="action">The workload to dispatch.</param>
        void Dispatch(Action action);

        /// <summary>
        /// Dispatch the workload to the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The workload to dispatch.</param>
        void DispatchAsync(Action action);
    }
}
