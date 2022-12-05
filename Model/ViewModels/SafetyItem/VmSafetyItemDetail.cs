using System;
using System.Collections.Generic;
using System.Web;

namespace Model.ViewModels.SafetyItem
{
    public class VmSafetyItemDetail
    {
        public int Id { get; set; }
        public int SafetyItemId { get; set; }
        public string Name { get; set; }
        public bool? Value { get; set; }
        public string Comment { get; set; }
    }
}
