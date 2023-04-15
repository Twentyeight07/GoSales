using System;
using System.Collections.Generic;

namespace Entity;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? RegistryDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
