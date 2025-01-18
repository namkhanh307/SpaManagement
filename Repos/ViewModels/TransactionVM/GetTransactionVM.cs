namespace Repos.ViewModels.TransactionVM
{
    public class GetTransactionVM : BaseVM
    {
        public double Amount { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
