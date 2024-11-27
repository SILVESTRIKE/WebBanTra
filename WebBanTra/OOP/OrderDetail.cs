using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanTra.Models;

namespace WebBanTra.OOP
{
    public class OrderDetail : KhachHang
    {
        public KhachHang khachHang { get; set; }
        public List<CartDetail> listCart { get; set; }

        public OrderDetail()
        {
            khachHang = new KhachHang();
            listCart = new List<CartDetail>();
        }
    }
}