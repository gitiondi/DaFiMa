using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Controls;
using Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using WpfClient.Helpers;
using WpfClient.Pages;
using WpfClient.Services;

namespace WpfClient.ViewModels
{
    public class AccountManagerViewModel : ObservableObject
    {
        private readonly IApplicationContext _applicationContext;
        private readonly AccountServices _accountServices;
        private AccountManager _accountManager;

        private RelayCommand? _addAccountCommand;
        private RelayCommand? _loadAccountCommand;
        private RelayCommand? _saveAccountCommand;
        private RelayCommand? _accountChangedCommand;
        private ObservableCollection<Account>? _accounts;
        private Page _currentAccountView;
        private string? _currentFileName;
        private double _total;
        private string? _newAccountName;

        public RelayCommand AddAccountCommand => _addAccountCommand = new RelayCommand(AddAccount, CanAddAccount);

        public RelayCommand LoadAccountsCommand => _loadAccountCommand = new RelayCommand(LoadAccounts);
        public RelayCommand SaveAccountsCommand => _saveAccountCommand = new RelayCommand(SaveAccounts);
        public RelayCommand AccountChangedCommand => _accountChangedCommand = new RelayCommand(AccountChanged);

        public string? NewAccountName
        {
            get => _newAccountName;
            set
            {
                SetProperty(ref _newAccountName, value);
                _addAccountCommand.NotifyCanExecuteChanged();
            }
        }

        public Account? SelectedAccount { get; set; }

        public Page CurrentAccountView
        {
            get => _currentAccountView;
            set => SetProperty(ref _currentAccountView, value);
        }

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        public double Total
        {
            get
            {
                _total = 0;
                if (_accounts != null)
                {
                    foreach (var account in _accounts)
                    {
                        _total += account.Amount;
                    }
                }
                return _total;
            }
            set => SetProperty(ref _total, value);
        }

        public string? CurrentFileName
        {
            get => _currentFileName;
            set => SetProperty(ref _currentFileName, value);
        }

        public void UpdateTotal()
        {
            OnPropertyChanged(nameof(Total));
        }

        private bool CanAddAccount()
        {
            return !string.IsNullOrEmpty(NewAccountName);
        }

        private void AddAccount()
        {
            _accountManager.CreateAccount(NewAccountName);
            Accounts = new ObservableCollection<Account>(_accountManager.Accounts);
            NewAccountName = string.Empty;
            AccountChanged();
        }

        private void LoadAccounts()
        {
            var openFileDialog = new OpenFileDialog();
            var path = ConfigurationManager.AppSettings["AccountDataDirectory"];
            openFileDialog.InitialDirectory = path;
            openFileDialog.DefaultExt = "json";
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                CurrentFileName = openFileDialog.FileName; //Path.GetFileName(openFileDialog.FileName);
                var jsonPath = openFileDialog.FileName;
                var json = File.ReadAllText(jsonPath);
                var accountManager = JsonSerializer.Deserialize<AccountManager>(json);
                _accountServices.SetAccountManager(accountManager);
                _accountManager = accountManager;

                if (_accountManager.Accounts.Any())
                {
                    SelectedAccount = _accountManager.Accounts.First();
                    Accounts = new ObservableCollection<Account>(_accountManager.Accounts);
                }
                else
                {
                    var serviceProvider = _applicationContext.ApplicationHost.Services;
                    var noDataView = serviceProvider.GetRequiredService<NoDataView>();
                    CurrentAccountView = noDataView;
                    Accounts = new ObservableCollection<Account>();
                }
            }
            UpdateTotal();
        }

        private void SaveAccounts()
        {
            var serializedAccountManager = JsonSerializer.Serialize<AccountManager>(_accountManager);
            if (CurrentFileName != null)
            {
                File.WriteAllText(CurrentFileName, serializedAccountManager);
            }
            else
            {
                var saveFileDialog = new SaveFileDialog();
                var path = ConfigurationManager.AppSettings["AccountDataDirectory"];
                saveFileDialog.InitialDirectory = path;
                saveFileDialog.DefaultExt = "json";
                saveFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    var jsonPath = saveFileDialog.FileName;
                    File.WriteAllText(jsonPath, serializedAccountManager);
                    CurrentFileName = saveFileDialog.FileName;
                }
            }
        }

        private void AccountChanged()
        {
            var serviceProvider = _applicationContext.ApplicationHost.Services;
            var accountView = serviceProvider.GetRequiredService<AccountView>();
            var accountViewModel = serviceProvider.GetRequiredService<AccountViewModel>();
            accountView.DataContext = accountViewModel;

            if (SelectedAccount != null)
            {
                accountViewModel.SetAccount(SelectedAccount);
                CurrentAccountView = accountView;
            }
            else
            {
                // Happens after loading a file because there is no selection in the data bound combo box.
                CurrentAccountView = serviceProvider.GetRequiredService<NoDataView>();
            }
        }

        public AccountManagerViewModel(IApplicationContext applicationContext, AccountServices accountServices)
        {
            _applicationContext = applicationContext;
            _accountServices = accountServices;
            _accountManager = _accountServices.GetAccountManager();

            var serviceProvider = _applicationContext.ApplicationHost.Services;
            var noDataView = serviceProvider.GetRequiredService<NoDataView>();

            _applicationContext.AccountManagerViewModel = this;
            CurrentAccountView = noDataView;
        }
    }
}
