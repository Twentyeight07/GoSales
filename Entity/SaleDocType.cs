using System;
using System.Collections.Generic;

namespace Entity;

public partial class SaleDocType
{
    public int SaleDocTypeId { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? RegistryDate { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
