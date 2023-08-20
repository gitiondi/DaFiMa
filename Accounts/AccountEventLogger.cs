namespace Accounts
{
    public class AccountEventLogger
    {
        public enum LogEvent {SetAmount, Credit, Payment }

        public List<AccountEventLogEntry> Logs { get; set; }

        public string AccountName { get; private set; }
        public Guid AccountId { get; private set; }
        public AccountEventLogger(string accountName, Guid accountId)
        {
            AccountName = accountName;
            AccountId = accountId;
            Logs = new List<AccountEventLogEntry>();
        }

        public void Log(double changeAmount, string description, LogEvent logEvent, double oldAmount, double newAmount)
        {
            var accountEventLogEntry = new AccountEventLogEntry
            {
                ChangeAmount = changeAmount,
                Description = description,
                LogEvent = logEvent,
                DateTime = DateTime.Now,
                OldAmount = oldAmount,
                NewAmount = newAmount
            };
            Logs.Add(accountEventLogEntry);
        }
    }
}
