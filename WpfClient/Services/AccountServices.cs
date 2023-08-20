using Accounts;

namespace WpfClient.Services
{
    public class AccountServices
    {
        private AccountManager? _accountManager;

        public AccountManager GetAccountManager()
        {
            return _accountManager ??= new AccountManager();
        }

        public void SetAccountManager(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public AccountServices()
        {
        }
    }
}
