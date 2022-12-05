using Model.Base;
using Model.ViewModels.SubmissionRule;

using System.Collections.Generic;

namespace Model.ViewModels.Team
{
    public class VmTeamCollection : BaseViewModel
    {
        public string DataAction { get; set; }
        public string DataController { get; set; }
        public bool AllowDownlaod { get; set; }
        public bool AllowEdit { get; set; }
        public bool ReadOnlyForm { get; set; }
        public bool AllowReject { get; set; }
        public string OnItemRejecting { get; set; }
        public bool AllowAccept { get; set; }
        public string OnItemAccepting { get; set; }
        public bool AllowDelete { get; set; }
        public bool ShowSearchBox { get; set; }
        public bool ShowNewTeamSign { get; set; }
        public bool Draggable { get; set; }
        public string SearchText { get; set; }
        public int ActiveItemId { get; set; }
        public string HtmlControlId { get; set; }
        public string ParentHtmlControlId { get; set; }
        public string OnItemSelected { get; set; }
        public string OnItemDragged { get; set; }
        public string SelectedItemHtmlControlId { get; set; }
        public IEnumerable<VmTeam> TeamList { get; set; }
        public List<VmSubmissionRule> SubmissionRuleList { get; set; }
        public bool RegisterForFlashTalk { get; set; }
    }
}
