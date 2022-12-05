using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model.ViewModels.Invoice
{
    public class VmInvoiceIds : BaseViewModel
    {
        public IEnumerable<int> Ids { get; set; }
        
    }
}
