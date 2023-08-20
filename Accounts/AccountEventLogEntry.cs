namespace Accounts
{
    public class AccountEventLogEntry
    {
        public DateTime DateTime { get; set; }
        public double ChangeAmount { get; set; }
        public string Description { get; set; }
        public AccountEventLogger.LogEvent LogEvent { get; set; }
        public double OldAmount { get; set; }
        public double NewAmount { get; set; }
    }
}
