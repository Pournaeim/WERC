using System.Collections.Generic;

namespace Model.ViewModels.Survey
{

    public class VmSurvey
    {

        public int Id { get; set; }
        public string Question { get; set; }
        public string QuestionComment { get; set; }
        public int QuestionPriority { get; set; }
        public int QuestionType { get; set; }
        public IEnumerable<VmSurveyDetail> SurveyDetailList { get; set; }
    }
}
