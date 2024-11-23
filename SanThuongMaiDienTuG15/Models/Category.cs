using System;
using System.Collections.Generic;

#nullable disable
namespace SanThuongMaiDienTuG15.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CatId { get; set; }
        public string CatName { get; set; } = null!;
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
