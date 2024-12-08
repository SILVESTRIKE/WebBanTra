using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanTra.OOP
{
    public class CartForNCC
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }

        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public decimal Tongtien
        {
            get
            {
                return Gia * Convert.ToDecimal(SoLuong);
            }
        }

    }
}