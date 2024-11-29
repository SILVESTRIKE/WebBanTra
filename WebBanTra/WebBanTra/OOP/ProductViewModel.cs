using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanTra.OOP
{
    [NotMapped]
    public class ProductViewModel
    {
        public List<WebBanTra.Models.SanPham> listProducts { get; set; }
        public List<WebBanTra.Models.Anh_SanPham> listAnhSP { get; set; }
        public int totalPage { get; set; }
        public int currentPage { get; set; }


    }
    
}