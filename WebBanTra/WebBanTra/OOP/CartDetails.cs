using System.ComponentModel.DataAnnotations;

namespace WebBanTra.OOP
{
    public class CartDetails
    {
        [Key]
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