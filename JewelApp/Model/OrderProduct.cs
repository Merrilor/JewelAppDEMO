//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JewelApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderProduct
    {
        public int OrderProductId { get; set; }
        public int OrderID { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
