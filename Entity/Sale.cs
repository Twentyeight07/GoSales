using System;
using System.Collections.Generic;

namespace Entity;

public partial class Sale
{
    public int SaleId { get; set; }

    public string? SaleNumber { get; set; }

    public int? SaleDocTypeId { get; set; }

    public int? UserId { get; set; }

    public string? ClientDoc { get; set; }

    public string? ClientName { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? TotalTax { get; set; }

    public decimal? Total { get; set; }

    public DateTime? RegistryDate { get; set; }

    public virtual ICollection<SaleDetail> SaleDetail { get; set; } = new List<SaleDetail>();

    public virtual SaleDocType? SaleDocType { get; set; }

    public virtual User? User { get; set; }
}
