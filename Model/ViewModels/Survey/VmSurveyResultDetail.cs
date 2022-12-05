
using System;
using System.Collections.Generic;

namespace Model.ViewModels.Survey
{
    public class VmSurveyResultDetail
    {
        public int Id { get; set; }
        public int SurveyResultId { get; set; }
        public int QuestionAnswerId { get; set; }
        public string Value { get; set; }
       
    }
}
