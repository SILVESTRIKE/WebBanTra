using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class SanPham
    {
        [Key]
        public int MaSP { get; set; } // Khóa chính, tự động tăng (IDENTITY trong SQL)

        [Required]
        [StringLength(255)] // Giới hạn chiều dài tên sản phẩm
        public string TenSP { get; set; } // Tên sản phẩm

        [Required]
        [Range(0, double.MaxValue)] // Giá phải >= 0
        public int Gia { get; set; } // Giá sản phẩm

        [Required]
        [Range(0, int.MaxValue)] // Số lượng tồn kho phải >= 0
        public int SoLuongTon { get; set; } // Số lượng tồn kho

        public int MaDM { get; set; } // Mã danh mục (khóa ngoại)

        // Navigation property
        [ForeignKey("MaDM")]
        public DanhMuc DanhMuc { get; set; } // Mối quan hệ với bảng DanhMuc
        public SanPham()
        {
            Gia = 0;
            SoLuongTon = 0;
        }
    }
    
}