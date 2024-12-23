﻿namespace Repos.Entities;

public partial class ServiceImage
{
    public string ServiceId { get; set; } = string.Empty;
    public string ImageId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public bool? Status { get; set; }
    public virtual Image? Image { get; set; }
    public virtual Service? Service { get; set; }
}