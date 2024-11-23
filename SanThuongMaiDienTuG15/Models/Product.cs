﻿using System;
using System.Collections.Generic;

namespace SanThuongMaiDienTuG15.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public int CatId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int? SellerId { get; set; }
        public DateTime? DatePosted { get; set; }
        public string ImageUrl { get; set; }
        public string ProductStatus { get; set; }
        public string Thumb { get; set; }

        public virtual Category Cat { get; set; } = null!;
        public virtual User Seller { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
