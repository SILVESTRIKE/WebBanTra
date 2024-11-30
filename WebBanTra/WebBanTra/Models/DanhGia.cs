using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class DanhGia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]// MaAnh là phần đầu tiên của Primary Key
        public int MaDG { get; set; } // Khóa chính (Primary Key)

        [ForeignKey("KhachHang")]
        public int? MaKH { get; set; } // Mã khách hàng (Khóa ngoại)

        [ForeignKey("ChiTietDH")]
        public int MaCTDH { get; set; } // Mã chi tiết đơn hàng (Khóa ngoại)

        [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5.")]
        public int DiemDG { get; set; } // Điểm đánh giá (từ 1 đến 5)

        public string BinhLuan { get; set; } // Bình luận (có thể null)

        [Required]
        public DateTime NgayDG { get; set; } // Ngày đánh giá

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } // Quan hệ với bảng KhachHang
        public virtual ChiTietDH ChiTietDH { get; set; } // Quan hệ với bảng ChiTietDH
    }
}