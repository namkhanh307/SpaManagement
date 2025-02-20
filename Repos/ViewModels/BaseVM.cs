namespace Repos.ViewModels
{
    public class BaseVM
    {
        public string Id { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool? Status { get; set; }
        public bool CanDelete { get; set; } = false;
        public bool CanUpdate { get; set; } = false;
    }
}
