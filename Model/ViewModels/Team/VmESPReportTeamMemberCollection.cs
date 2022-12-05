using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Model.ViewModels.Team
{
    [DataObject]
    public class VmESPReportTeamMemberCollection:DataSet
    {

        List<VmESPReportTeamMember> VmESPReportTeamMembers { get; set; }

    }
}