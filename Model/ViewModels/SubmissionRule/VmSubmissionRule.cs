using Model.Base;

using System;
using System.Collections.Generic;

namespace Model.ViewModels.SubmissionRule
{
    public class VmSubmissionRule
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string DueDate { get; set; }
        public List<VmTaskSubmissionRule> TaskSubmissionRuleList { get; set; }
        public int[] TaskIds { get; set; }
        public string ClientTaskIds { get; set; }
        public string SubmissionRuleUrl { get; set; }
        public bool? PayStatus { get; set; }
        public bool? RegistrationStatus { get; set; }
        public bool TeamPayStatus { get; set; }
        public bool? TeamRegistrationStatus { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public bool? ShowLate { get; set; }
        public bool? ShowReport { get; set; }
        public bool? SendEmail { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? DueDateOrder { get; set; }
    }
}
