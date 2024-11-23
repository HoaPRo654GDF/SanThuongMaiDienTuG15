using System;
using System.Collections.Generic;

#nullable disable
namespace SanThuongMaiDienTuG15.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
