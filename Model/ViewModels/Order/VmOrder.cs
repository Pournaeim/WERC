using Model.ViewModels.Product;
using System;
using System.Collections.Generic;

namespace Model.ViewModels.Order
{
    public class VmOrder
    {
        public int Id { get; set; }
        public int ShopOrderId { get; set; }
        public string UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int InvoiceId { get; set; }
        public bool Complete { get; set; }
        public string Title { get; set; }
        public DateTime DateOfIssue { get; set; }
        public long InvoiceNumber { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal AmountDue { get; set; }
        public bool Finished { get; set; }
        public decimal? ConventionalFee { get; set; }
        public List<VmProduct> Products { get; set; }
        public DateTime? Received { get; set; }
        public string TransactionNo { get; set; }
    }
}
