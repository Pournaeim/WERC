using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.Survey
{

    public class VmSurveyManagement : BaseViewModel
    {
        public IEnumerable<VmSurvey> SurveyList { get; set; }
        public string ViewLayout { get; set; }

    }
}
