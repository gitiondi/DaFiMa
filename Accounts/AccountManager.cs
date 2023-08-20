namespace Accounts
{
    public class AccountManager
    {
        public List<Account> Accounts { get; set; }

        public AccountManager()
        {
            Accounts = new List<Account>();
        }

        public void CreateAccount(string accountName)
        {
            var accointId = Guid.NewGuid();
            var account = new Account(accountName, accointId);
            Accounts.Add(account);
        }

        public void DeleteAccount(string accountName)
        {
            //todo
            //check if amount == 0 before deleting an account
        }

    }
}
