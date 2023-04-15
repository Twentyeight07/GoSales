using System;
using System.Collections.Generic;

namespace Entity;

public partial class RoleMenu
{
    public int RoleMenuId { get; set; }

    public int? Roleid { get; set; }

    public int? MenuId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? RegistryDate { get; set; }

    public virtual Menu? Menu { get; set; }

    public virtual Role? Role { get; set; }
}
