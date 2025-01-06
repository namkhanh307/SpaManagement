namespace Repos.ViewModels.TransactionVM
{
    public class PostTransactionVM
    {
        public string OrderId { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
