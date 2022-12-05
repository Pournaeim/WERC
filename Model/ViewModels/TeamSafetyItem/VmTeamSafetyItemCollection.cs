using Model.Base;
using Model.ViewModels.Reference;
using Model.ViewModels.SafetyItem;
using Model.ViewModels.Team;
using System.Collections.Generic;

namespace Model.ViewModels.TeamSafetyItem
{
    public class VmTeamSafetyItemCollection : BaseViewModel
    {

        public VmReferenceCollection ReferenceFiles { get; set; }
        public IEnumerable<VmSafetyItem> SafetyItemList { get; set; }
        public IEnumerable<VmTeamSafetyItem> TeamSafetyItemList { get; set; }
        public IEnumerable<VmTeamMember> TeamMemberList { get; set; }
        public string TeamName { get; set; }
        public string TaskName { get; set; }
        public string Advisor { get; set; }
        public string University { get; set; }
        public int TeamId { get; set; }
    }
}
