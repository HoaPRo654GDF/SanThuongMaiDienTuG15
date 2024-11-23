using SanThuongMaiDienTuG15.Models;
using System.Collections.Generic;

namespace SanThuongMaiDienTuG15.ModelViews
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string BuyerName { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
