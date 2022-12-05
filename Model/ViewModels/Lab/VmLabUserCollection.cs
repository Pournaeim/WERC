using Model.Base;
using Model.ViewModels.Task;
using System.Collections.Generic;

namespace Model.ViewModels.Lab
{
    public class VmLabUsers : BaseViewModel
    {
        public string Name { get; set; }

        public List<VmTask> Tasks { get; set; }
    }
}
