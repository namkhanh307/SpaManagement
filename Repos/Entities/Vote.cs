namespace Repos.Entities;

public partial class Vote : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int? Number { get; set; }
    public string? Content { get; set; }
}