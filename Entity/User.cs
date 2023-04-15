﻿using System;
using System.Collections.Generic;

namespace Entity;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? IdRole { get; set; }

    public string? PicUrl { get; set; }

    public string? PicName { get; set; }

    public string? Password { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? RegistryDate { get; set; }

    public virtual Role? IdRoleNavigation { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
