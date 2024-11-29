using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class MoTa_SanPham
    {
        [Key, Column(Order = 0)] // MaMoTa là phần đầu tiên của Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaMoTa { get; set; }

        [Key, Column(Order = 1)] // MaSP là phần thứ hai của Primary Key (phối hợp với MaMoTa)
        public int MaSP { get; set; }

        [StringLength(255)]
        public string MoTa { get; set; } // Mô tả sản phẩm

        // Navigation property
        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; } // Quan hệ với bảng SanPham
    }
}