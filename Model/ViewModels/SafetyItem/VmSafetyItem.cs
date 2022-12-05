using System;
using System.Collections.Generic;
using System.Web;

namespace Model.ViewModels.SafetyItem
{
    public class VmSafetyItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Instruction { get; set; }
        public int Priority { get; set; }
        public int? SafetyItemStatus { get; set; }
        public bool AttachmentRequired { get; set; }
        public bool TextRequired { get; set; }
        public List<SubSafetyItem> SubSafetyItems { get; set; }
        public List<SafetyItemDetail> SafetyItemDetails{ get; set; }
        
        public int[] ChecklistIds { get; set; }
        public string ClientChecklistIds { get; set; }

        public string AttachedFileUrl { get; set; }
    }
}
