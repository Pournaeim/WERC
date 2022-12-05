using Model.Base;
using Model.ViewModels.Task;
using System.Collections.Generic;

namespace Model.ViewModels.Admin
{
    public class VmAssignTaskToJudgeManagement : BaseViewModel
    {
        public IEnumerable<VmTask> Tasks { get; set; }
        public object Judges { get; set; }
    }
}
