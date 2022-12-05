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
    public class VmTeamSafetyItem
    {

        public int Id { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int SafetyItemId { get; set; }
        public string LastContent { get; set; }
        public string LastComment { get; set; }
        public int? ItemStatus { get; set; }
        public string AttachedFileUrl { get; set; }
        public HttpPostedFileBase UploadedAttachedFile { get; set; }
        public bool Type { get; set; }
 
        public IEnumerable<VmTeamSafetyItemLog> TeamSafetyItemLogList { get; set; }
        public string UserId { get; set; }
        public string SafetyItemName { get; set; }
        public string BaseSafetyItemName { get; set; }
        public string SubSafetyItemName { get; set; }
        public int BasePriority { get; set; }
        public int Priority { get; set; }
        public string Instruction { get; set; }
        public bool AttachmentRequired { get; set; }
        public bool TextRequired { get; set; }
        public int SubSafetyItemId { get; set; }
        public string SafetyItemDetailIds { get; set; }
        public string BaseInstruction { get; set; }
    }
}
