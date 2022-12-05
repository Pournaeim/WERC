namespace Model.ViewModels.Survey
{
    public class VmClientSurveyResult
    {
        public int QuestionId { get; set; }
        public int QuestionAnswerId { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public int Type { get; set; }
}
}