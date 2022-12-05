using System.Collections.Generic;
using Model.Base;
using Model.ViewModels.Grade.Report;

namespace Model.ViewModels.Judge
{
    public class VmNoneAnsweredGradeManagement : BaseViewModel
    {
        public IEnumerable<VmJudgeBaseGrade> JudgeBaseGrades { get; set; }
        public string ViewLayout { get; set; }
    }
}
