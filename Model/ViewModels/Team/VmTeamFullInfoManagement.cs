
using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.Team
{
    public class VmTeamFullInfoManagement : BaseViewModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IEnumerable<VmTeamMemberUserId> MemberUserIds { get; set; }
    }
}
