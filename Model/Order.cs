//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int Id { get; set; }
        public int ShopOrderId { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public int InvoiceId { get; set; }
        public bool Complete { get; set; }
        public string TransactionNo { get; set; }
        public Nullable<System.DateTime> Received { get; set; }
    }
}