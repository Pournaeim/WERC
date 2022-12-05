
using System.Collections.Generic;

namespace Model.ViewModels.Grade.Report
{
    public class VmGradeReport
    {
        public int GradeId { get; set; }
        public string GradeType { get; set; }
        public double? Average { get; set; }
        public List<VmTeamGradeDetail> TeamGradeDetails { get; set; }
        public string Comment { get; set; }
    }
}
