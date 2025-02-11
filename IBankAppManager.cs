namespace BankApp
{
    public interface IBankAppManager
    {
        void CreateAnAccount();
        void Login();
        void DepositCash(Data info);
        void TransferCash(Data info);
        void DisplayAccountBalance(Data info);
        void ViewAccountInfo(Data info);

    }
}