namespace Model.ViewModels.Grade.Grading
{
    public class VmSingleGradingType
    {
        public int GradeId { get; set; }
        public string GradeType { get; set; }
        public VmTeamGrading TeamGrading { get; set; }
        public string CurrentUserId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public double? TotalScore { get; set; }
        public double? GradeTotalScore { get; set; }
    }
}
