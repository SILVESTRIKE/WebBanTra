using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebBanTra.Models;

namespace WebBanTra.OOP
{
    [NotMapped]
    public class DetailProduct : WebBanTra.Models.SanPham
    {
        public DetailProduct() : base()
        { }

        public string mota { get; set; }
        public string hinhanh { get; set; }
    }
}