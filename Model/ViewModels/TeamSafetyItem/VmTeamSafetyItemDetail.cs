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
    public class VmTeamSafetyItemDetail
    {
        public int Id { get; set; }
        public int SafetyItemDetailId { get; set; }
        public string Name { get; set; }
        public bool? Value { get; set; }
        public string Comment { get; set; }
        public int? TeamId { get; set; }
    }
}
