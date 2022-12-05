using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.Invoice
{
    public class VmInvoiceCollection : BaseViewModel
    {
        public List<VmInvoiceReport> Invoices { get; set; }

    }
}
