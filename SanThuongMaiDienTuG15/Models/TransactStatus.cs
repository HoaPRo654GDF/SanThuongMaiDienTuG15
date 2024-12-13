﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SanThuongMaiDienTuG15.Models
{
    public partial class TransactStatus
    {
        public TransactStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int TransactStatusId { get; set; }
        public string Status { get; set; } = null!;
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
