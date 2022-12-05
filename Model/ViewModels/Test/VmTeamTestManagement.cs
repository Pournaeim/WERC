
using Model.Base;
using Model.ViewModels.Lab;
using Model.ViewModels.Task;
using System.Collections.Generic;

namespace Model.ViewModels.Test
{
    public class VmTeamTestManagement : BaseViewModel
    {
        public List<VmLabUsers> LabUsers { get; set; }
    }
}