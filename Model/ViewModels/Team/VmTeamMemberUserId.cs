using System.Collections;

namespace Model.ViewModels.Team
{
    public class VmTeamMemberUserId
    {
        public int TeamId { get; set; }
        public IEnumerable[] UserIds { get; set; }
    }
    public class VmRawTeamMemberUserId
    {
        public int TeamId { get; set; }
        public string MemberUserId { get; set; }
    }
}