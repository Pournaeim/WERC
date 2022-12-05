
using Model.Base;

using System.Collections.Generic;

namespace Model.ViewModels.Grade.Report
{
    public class VmTaskBaseGrade : BaseViewModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public List<VmGrade> GradeList { get; set; }
        public List<VmTeamGrade> TeamGradeList { get; set; }
    }
}
