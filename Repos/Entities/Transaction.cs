namespace Repos.Entities
{
    public partial class Transaction : BaseEntity
    {
        public string OrderId { get; set; } = string.Empty;
        public double Amound { get; set; }
        public string Note { get; set; } = string.Empty;
        public virtual Order? Order { get; set; }
    }
}
