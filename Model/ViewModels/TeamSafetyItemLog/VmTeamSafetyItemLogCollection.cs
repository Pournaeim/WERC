using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.TeamSafetyItemLog
{
    public class VmTeamSafetyItemLogCollection : BaseViewModel
    {
        public IEnumerable<VmSafetyItemLog> SafetyItemLogList { get; set; }
    }
}
