using Model.Base;
using Model.ViewModels.ParticipantRule;
using Model.ViewModels.PaymentRule;
using System.Collections.Generic;

namespace Model.ViewModels.Admin
{
    public class VmPaymentRulesManagement : BaseViewModel
    {
        public VmParticipantRule ParticipantRule { get; set; }
        public VmParticipantRule ParticipantRule_Desktop { get; set; }
        public VmParticipantRule ParticipantRule_BenchScale { get; set; }
    }
}
