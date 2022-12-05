using System.Collections.Generic;

namespace Model.ViewModels.Grade.Grading
{
    public class VmTeamGrading
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IEnumerable<VmGradingDetail> GradingDetailList { get; set; }

    }
}