using Model.Base;
using Model.ViewModels.Grade.Report;
using System.Collections.Generic;

namespace Model.ViewModels.Judge
{
    public class VmGradeDetailManagement : BaseViewModel
    {
        public IEnumerable<VmGradeDetail> GradeDetailList { get; set; }
    }
}
