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
    
    public partial class MoTa_SanPham
    {
        public int MaMoTa { get; set; }
        public int MaSP { get; set; }
        public string MoTa { get; set; }
    
        public virtual SanPham SanPham { get; set; }
    }
}
