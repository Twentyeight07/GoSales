using System;
using System.Collections.Generic;

namespace Entity;

public partial class Product
{
    public int ProductId { get; set; }

    public string? BarCode { get; set; }

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public int? Stock { get; set; }

    public string? PicUrl { get; set; }

    public string? PicName { get; set; }

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? RegistryDate { get; set; }

    public virtual Category? Category { get; set; }
}
