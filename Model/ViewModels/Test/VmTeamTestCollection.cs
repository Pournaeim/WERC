using Model.Base;
using Model.ViewModels.Test;
using System.Collections.Generic;

namespace Model.ViewModels.Test
{
    public class VmTeamTestCollection : BaseViewModel
    {
        public int TaskId { get; set; }
        public IEnumerable<VmTeamTest> TeamTestList { get; set; }
        public List<string> UserIds { get; set; }
    }
}
