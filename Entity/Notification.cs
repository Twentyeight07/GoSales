﻿using System;
using System.Collections.Generic;

namespace Entity
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int? UserRole { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? SaleNum { get; set; }
    }
}
