using System;
using Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using WpfClient.Helpers;
using WpfClient.Services;


namespace WpfClient.ViewModels
{
    public class AccountViewModel : ObservableObject
    {
        private readonly IApplicationContext _applicationContext;
        private readonly AccountServices _accountServices;
        private double _amount;
        private Account _account;
        private string _accountName;
        private RelayCommand _setAmountCommand;
        private RelayCommand _addAmountCommand;
        private RelayCommand _transferAmountCommand;
        private RelayCommand _withdrawAmountCommand;
        private ObservableCollection<Account> _accounts;
        private AccountManager _accountManager;
        private double _setAmountValue;
        private string _setAmountDescription;
        private double _addAmountValue;
        private string _addAmountDescription;
        private double _transferAmountValue;
        private string _transferAmountDescription;
        private double _withdrawAmountValue;
        private string _withdrawAmountDescription;

        public RelayCommand SetAmountCommand => _setAmountCommand = new RelayCommand(SetAmount, CanSetAmount);
        public RelayCommand AddAmountCommand => _addAmountCommand = new RelayCommand(AddAmount, CanAddAmount);

        public RelayCommand TransferAmountCommand => _transferAmountCommand = new RelayCommand(TransferAmount, CanTransferAmount);
        public RelayCommand WithdrawAmountCommand => _withdrawAmountCommand = new RelayCommand(WithdrawAmount, CanWithdrawAmount);

        private void TransferAmount()
        {
            _account.TransferToAccount(SelectedAccount, TransferAmountValue, TransferAmountDescription);
            Amount = _account.Amount;
            TransferAmountValue = 0;
        }

        private bool CanTransferAmount()
        {
            return TransferAmountValue != 0;
        }

        private void SetAmount()
        {
            _account.SetAmount(SetAmountValue, SetAmountDescription);
            SetAmountValue = 0;
            SetAmountDescription = String.Empty;
            Amount = _account.Amount;
        }

        private bool CanSetAmount()
        {
            return SetAmountValue != 0;
        }

        private void AddAmount()
        {
            _account.SetAmount(_account.Amount + AddAmountValue, AddAmountDescription);
            AddAmountValue = 0;
            AddAmountDescription = string.Empty;
            Amount = _account.Amount;
        }

        private bool CanAddAmount()
        {
            return AddAmountValue != 0;
        }
        private bool CanWithdrawAmount()
        {
            return WithdrawAmountValue != 0;
        }

        private void WithdrawAmount()
        {
            _account.SetAmount(_account.Amount - WithdrawAmountValue, WithdrawAmountDescription);
            WithdrawAmountValue = 0;
            WithdrawAmountDescription = String.Empty;
            Amount = _account.Amount;
        }

        public void SetAccount(Account account)
        {
            _account = account;
            Amount = _account.Amount;
            AccountName = _account.AccountName;
            _accountManager = _accountServices.GetAccountManager();
            Accounts = new ObservableCollection<Account>(_accountManager.Accounts.Where(x => _account.AccountId != x.AccountId));
            if (Accounts.Any())
            {
                SelectedAccount = Accounts[0];
            }
        }

        public double Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public string AccountName
        {
            get => _accountName;
            set => SetProperty(ref _accountName, value);
        }

        public double SetAmountValue
        {
            get => _setAmountValue;
            set
            {
                SetProperty(ref _setAmountValue, value);
                _setAmountCommand.NotifyCanExecuteChanged();
                UpdateAccountManagerViewModel();
            }
        }

        public double AddAmountValue
        {
            get => _addAmountValue;
            set
            {
                SetProperty(ref _addAmountValue, value);
                _addAmountCommand.NotifyCanExecuteChanged();
                UpdateAccountManagerViewModel();
            }
        }

        public double WithdrawAmountValue
        {
            get => _withdrawAmountValue;
            set
            {
                SetProperty(ref _withdrawAmountValue, value);
                _withdrawAmountCommand.NotifyCanExecuteChanged();
                UpdateAccountManagerViewModel();
            }
        }

        public string SetAmountDescription
        {
            get => _setAmountDescription;
            set => SetProperty(ref _setAmountDescription, value);
        }

        public string AddAmountDescription
        {
            get => _addAmountDescription;
            set => SetProperty(ref _addAmountDescription, value);
        }

        public string WithdrawAmountDescription
        {
            get => _withdrawAmountDescription;
            set => SetProperty(ref _withdrawAmountDescription, value);
        }

        public double TransferAmountValue
        {
            get => _transferAmountValue;
            set
            {
                SetProperty(ref _transferAmountValue, value);
                _transferAmountCommand.NotifyCanExecuteChanged();
                UpdateAccountManagerViewModel();
            }
        }

        public string TransferAmountDescription
        {
            get => _transferAmountDescription;
            set => SetProperty(ref _transferAmountDescription, value);
        }
    
        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }


        public AccountViewModel(IApplicationContext applicationContext, AccountServices accountServices)
        {
            _applicationContext = applicationContext;
            _accountServices = accountServices;
        }

        private void UpdateAccountManagerViewModel()
        {
            _applicationContext.AccountManagerViewModel?.UpdateTotal();
        }

    }
}
