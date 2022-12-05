using System.Collections.Generic;

namespace Model.ViewModels.Survey
{
    public class VmSurveyResult
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public int QuestionPriority { get; set; }
        public int QuestionType { get; set; }
        public int AnswerId { get; set; }
        public int QuestionAnswerId { get; set; }
        public int AnswerType { get; set; }
        public decimal Weight { get; set; }
        public int AnswerPriority { get; set; }
        public string Answer { get; set; }
        public bool TitleVisible { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public int? TeamId { get; set; }
        public string TeamName { get; set; }
        public int? TaskId { get; set; }
        public string TaskName { get; set; }
        public bool? Sex { get; set; }
        public string UserId { get; set; }
        public IEnumerable<VmSurveyResultDetail> SurveyResultDetailList { get; set; }
        public string Description { get; set; }
    }
}
