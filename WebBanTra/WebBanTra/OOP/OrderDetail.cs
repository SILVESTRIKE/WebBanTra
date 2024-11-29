using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebBanTra.Models;

namespace WebBanTra.OOP
{
    [NotMapped]
    public class OrderDetail : KhachHang
    {
        public KhachHang khachHang { get; set; }
        public List<CartDetails> listCart { get; set; }

        public OrderDetail()
        {
            khachHang = new KhachHang();
            listCart = new List<CartDetails>();
        }
    }
}