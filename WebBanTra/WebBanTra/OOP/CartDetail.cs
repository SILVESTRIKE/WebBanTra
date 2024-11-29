using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanTra.OOP
{
    [NotMapped]
    public class CartDetail
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public int MaDM { get; set; }
        public string LinkAnh { get; set; }

        public int TongTien
        {
            get
            {
                return (int)Gia * SoLuong;
            }
        }
    }
}