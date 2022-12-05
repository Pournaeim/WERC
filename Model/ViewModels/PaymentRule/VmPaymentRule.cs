using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.PaymentRule
{
    public class VmPaymentRule
    {
        public int Id { get; set; }
        public string TypeOfRegistration { get; set; }
        public decimal FirstTeamFee { get; set; }
        public decimal ExtraTeamDiscount { get; set; }
        public string DueDatePrefix { get; set; }
        public string DueDate { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeDescription { get; set; }
    }
}
