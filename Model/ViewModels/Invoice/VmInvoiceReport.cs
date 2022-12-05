using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model.ViewModels.Invoice
{
    public class VmInvoiceReport:BaseViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        [DisplayName("Date Of Issue")]
        public DateTime DateOfIssue { get; set; }

        [DisplayName("Invoice Number")]
        public long InvoiceNumber { get; set; }

        [DisplayName("Invoice Total")]
        public decimal InvoiceTotal { get; set; }

        [DisplayName("To")]
        public string AccountOwner { get; set; }

        public string Address { get; set; }

        public List<VmInvoiceDetail> InvoiceDetails { get; set; }

        [DisplayName("Subtotal")]
        public decimal Subtotal { get; set; }

        [DisplayName("Total Service Fee")]
        public decimal TotalConventionalFee { get; set; }

        [DisplayName("Tax")]
        public decimal Tax { get; set; }

        [DisplayName("Total")]
        public decimal Total { get; set; }

        [DisplayName("Amount Due (USD)")]
        public decimal AmountDue { get; set; }

        [DisplayName("Service Fee (USD)")]
        public decimal ConventionalFee { get; set; }
        public bool Finished { get; set; }
        public int LastCheckedId { get; set; }
        public bool AllowCheckFisrtTeam { get; set; }
        [DisplayName("Receipt #")]
        public string TransactionNo { get; set; }
        public string Received { get; set; }
        public string University { get; set; }
        public bool teamDiscountLowerThan100 { get; set; }
        public decimal ScholarshipDiscount { get; set; }
        public decimal TeamAmountDue { get; set; }
        
    }
}
