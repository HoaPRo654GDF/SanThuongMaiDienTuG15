using System;
using System.Collections.Generic;

#nullable disable
namespace SanThuongMaiDienTuG15.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
