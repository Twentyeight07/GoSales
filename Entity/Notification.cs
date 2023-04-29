using System;
using System.Collections.Generic;

namespace Entity
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ProductId { get; set; }
        public int? Stock { get; set; }
    }
}
