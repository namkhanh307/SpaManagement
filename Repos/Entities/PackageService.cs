﻿using Core.Enum;
namespace Repos.Entities;

public partial class PackageService
{
    public string PackageId { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public EnumPackageService Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public bool? Status { get; set; }
    public virtual Package? Package { get; set; }
    public virtual Service? Service { get; set; }
}