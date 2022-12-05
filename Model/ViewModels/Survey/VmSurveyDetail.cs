namespace Model.ViewModels.Survey
{

    public class VmSurveyDetail
    {
        public int AnswerId { get; set; }
        public int QuestionAnswerId { get; set; }
        public int AnswerType { get; set; }
        public decimal Weight { get; set; }
        public int AnswerPriority { get; set; }
        public string Answer { get; set; }
        public bool TitleVisible { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public bool ShowComment { get; set; }

    }
}
