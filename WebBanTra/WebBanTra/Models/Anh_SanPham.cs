using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanTra.Models
{
    public class Anh_SanPham
    {
        [Key, Column(Order = 0)] // MaAnh là phần đầu tiên của Primary Key
        public int MaAnh { get; set; }

        [Key, Column(Order = 1)] // MaSP là phần thứ hai của Primary Key (phối hợp với MaAnh)
        public int MaSP { get; set; }

        [StringLength(255)]
        public string LinhAnh { get; set; } // Link đến ảnh của sản phẩm

        // Navigation property
        [ForeignKey("MaSP")]
        public SanPham SanPham { get; set; } // Quan hệ với bảng SanPham
    }
}