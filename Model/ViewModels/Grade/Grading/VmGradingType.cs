using System.Collections.Generic;

namespace Model.ViewModels.Grade.Grading
{
    public class VmGradingType
    {
        public int GradeId { get; set; }
        public string GradeType { get; set; }
        public IEnumerable<VmTeamGrading> TeamGradingList { get; set; }
        public string CurrentUserId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public double TotalScore { get; set; }
        public double GradeTotalScore { get; set; }
    }
}
