//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dish_Decision_Project.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CTMA
    {
        public string MaMonAn { get; set; }
        public string MaNguyenLieu { get; set; }
        public string LieuLuong { get; set; }
    
        public virtual MONAN MONAN { get; set; }
        public virtual NGUYENLIEU NGUYENLIEU { get; set; }
    }
}
