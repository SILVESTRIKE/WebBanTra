//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBanTra.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonNhapHang
    {
        public DonNhapHang()
        {
            this.ChiTietDNHs = new HashSet<ChiTietDNH>();
        }
    
        public int MaDNH { get; set; }
        public Nullable<int> MaNCC { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<decimal> TongTien { get; set; }
    
        public virtual ICollection<ChiTietDNH> ChiTietDNHs { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
