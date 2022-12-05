
using Model.Base;
using Model.ViewModels.Judge;
using System.Collections.Generic;

namespace Model.ViewModels.Grade.Report
{
    public class VmGradeDetail
    {
        public int Id { get; set; }
        public string EvaluationItem { get; set; }
        public List<VmJudgeGrade> JudgeList { get; set; }
    }
}
