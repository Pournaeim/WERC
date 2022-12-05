using Model.Base;
using Model.ViewModels.Reference;
using Model.ViewModels.SafetyItem;
using Model.ViewModels.Team;
using System.Collections.Generic;

namespace Model.ViewModels.TeamSafetyItem
{
    public class VmESPReportTeamSafetyItem : BaseViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Instruction { get; set; }
        public int Priority { get; set; }
        public int? SafetyItemStatus { get; set; }
        public bool AttachmentRequired { get; set; }
        public bool TextRequired { get; set; }
        public string TeamName { get; set; }
        public string TaskName { get; set; }
        public string Advisor { get; set; }
        public string University { get; set; }
        public int TeamId { get; set; }
        public List<VmTeamSafetyItem> TeamSafetyItems { get; set; }
        public List<VmTeamSafetyItemDetail> TeamSafetyItemDetails { get; set; }

    }
}
