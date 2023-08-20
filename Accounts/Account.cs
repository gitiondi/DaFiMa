namespace Accounts
{
    public class Account
    {
        public double Amount { get; set; }
        public string AccountName { get; set; }
        public Guid AccountId { get; set; }

        public AccountEventLogger AccountEventLogger { get; private set; }
        
        public Account(string accountName, Guid accountId)
        {
            AccountName = accountName;
            AccountId = accountId;
            AccountEventLogger = new AccountEventLogger(accountName, accountId);
        }

        public void SetAmount(double amount, string description)
        {
            var oldAmount = Amount;
            Amount = amount;
            AccountEventLogger.Log(amount, description, AccountEventLogger.LogEvent.SetAmount, oldAmount, Amount);
        }

        public void Credit(double amount, string description)
        {
            var oldAmount = Amount;
            Amount += amount;
            AccountEventLogger.Log(amount, description, AccountEventLogger.LogEvent.Credit, oldAmount, Amount);
        }

        public void Payment(double amount, string description)
        {
            var oldAmount = Amount;
            Amount -= amount;
            AccountEventLogger.Log(amount, description, AccountEventLogger.LogEvent.Payment, oldAmount, Amount);
        }

        public void TransferToAccount(Account targetAccount, double amount, string description)
        {
            targetAccount.Credit(amount, description);
            this.Payment(amount, description);
        }
    }
}