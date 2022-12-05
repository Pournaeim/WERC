using Model.Base;
using Model.ViewModels.SafetyItem;
using Model.ViewModels.Task;
using Model.ViewModels.TeamSafetyItemLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Model.ViewModels.TeamSafetyItem
{
    public class VmWERCESPObject
    {
        public string BaseSafetyItemName { get; set; }
        public string BaseInstruction { get; set; }
        public int BasePriority { get; set; }
        public string LastContent { get; set; }
        public string LastComment { get; set; }
        public string SubSafetyItemName { get; set; }
        public string Instruction { get; set; }
        public int Priority { get; set; }
        public string TeamSafetyItemDetails { get; set; }

    }
}
