using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanTra.OOP
{
    [NotMapped]
    public class ProductDetailViewModel
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public double Gia { get; set; }
        public int SoLuongTon { get; set; }
        public string TenDM { get; set; }
        public List<string> Images { get; set; }
        public List<string> Descriptions { get; set; }
        public string DanhMuc { get; set; }
        public List<ProductViewModel> SanPhamTuongTu { get; set; }
        public List<DanhGiaViewModel> DanhGias { get; set; }
    }

    public class DanhGiaViewModel
    {
        public string TenKH { get; set; }        // Tên khách hàng
        public int DiemDG { get; set; }         // Điểm đánh giá
        public string BinhLuan { get; set; }    // Bình luận
        public DateTime NgayDG { get; set; }    // Ngày đánh giá
    }

}