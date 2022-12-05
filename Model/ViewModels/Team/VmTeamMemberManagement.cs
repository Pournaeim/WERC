using Model.Base;
using Model.ViewModels.SubmissionRule;
using System.Collections.Generic;

namespace Model.ViewModels.Team
{
    public class VmTeamMemberManagement : BaseViewModel
    {

        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string Task { get; set; }
        public string WrittenReportUrl { get; set; }
        public string PreliminaryReportUrl { get; set; }
        public string OpenTaskTestPlanUrl { get; set; }
        public string BrochureUrl { get; set; }
        public string AwardNominationUrl { get; set; }

        public List<VmSubmissionRule> SubmissionRuleList { get; set; }
        public bool? Preliminary { get; set; }
        public bool? OpenTaskTestPlan { get; set; }
        public bool? RegistrationStatus { get; set; }
        public bool? PayStatus { get; set; }
        public string FlashTalkReportUrl { get; set; }         
        public string ProjectTitle { get; set; }         
    }
}
