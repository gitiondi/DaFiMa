using Accounts;
using System.Text.Json;

namespace ConsoleClient
{
    public class Client
    {
        public static void Main()
        {
            AccountManager accountManager = new AccountManager();
            accountManager.CreateAccount("Hauptkonto");
            accountManager.CreateAccount("Velokonto");

            var account1 = accountManager.Accounts[0];
            account1.Credit(4883.2, "Lotogewinn");
            account1.Payment(23.25, "Mittwochseinkauf");

            var account2 = accountManager.Accounts[1];
            account1.TransferToAccount(account2, 200, "Einlage für Stossdämpfer");

            string serializedAccountManager = JsonSerializer.Serialize<AccountManager>(accountManager);
            string jsonPath = @"d:\data\txt\account.json";
            File.WriteAllText(jsonPath, serializedAccountManager);

            string json = File.ReadAllText(jsonPath);
            AccountManager accountManager2 = JsonSerializer.Deserialize<AccountManager>(json);

        }

    }
}