using System.Collections.Generic;

namespace WebBanTra.OOP
{
    public class ProductViewModel
    {
        public List<WebBanTra.Models.SanPham> listProducts { get; set; }
        public List<WebBanTra.Models.Anh_SanPham> listAnhSP { get; set; }
        public int totalPage { get; set; }
        public int currentPage { get; set; }


    }
    
}