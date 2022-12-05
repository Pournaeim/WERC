using System.Collections.Generic;

namespace Model.ViewModels.Grade.Report
{
    public class VmJudgeBaseGrade
    {
        public string JudgeUserId { get; set; }
        public string Signature { get; set; }
        public string JudgeName { get; set; }
        public double? Point { get; set; }
        public string Description { get; set; }
        public List<VmTeamGrade> TeamGradeList { get; set; }



    }
}
