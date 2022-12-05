using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.Grade.Grading
{
    public class VmGradingManagement : BaseViewModel
    {
        public IEnumerable<VmGradingType> GradingTypeList { get; set; }
        public int TeamId { get; set; }
    }
}
