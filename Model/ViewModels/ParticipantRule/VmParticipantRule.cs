using Model.Base;

namespace Model.ViewModels.ParticipantRule
{
    public class VmParticipantRule : BaseViewModel
    {
        public int Id { get; set; }
        public int FirstTeamMaxMember { get; set; }
        public int EachExtraTeamMaxMember { get; set; }
        public decimal ExtraParticipantFee { get; set; }
        public string UIExtraParticipantFee { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeDescription { get; set; }
        public string PaymentType { get; set; }
    }
}
