using Model.Base;
using Model.ToolsModels.DropDownList;
using System.Collections.Generic;

namespace Model.ViewModels.Grade.Grading
{
    public class VmSingleTeamGradingManagement : BaseViewModel
    {
        public IEnumerable<VmSelectListItem> GradeList { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string Task { get; set; }
        public int TaskId { get; set; }
        public string University { get; set; }
        public int? ABETId { get; set; }
    }
}
