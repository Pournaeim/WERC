//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewTeamSafetyItem
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string LastContent { get; set; }
        public string LastComment { get; set; }
        public Nullable<int> ItemStatus { get; set; }
        public string AttachedFileUrl { get; set; }
        public string SafetyItemName { get; set; }
        public string Instruction { get; set; }
        public int Priority { get; set; }
        public int SafetyItemId { get; set; }
        public bool AttachmentRequired { get; set; }
        public bool TextRequired { get; set; }
        public Nullable<int> SubSafetyItemId { get; set; }
        public string TeamName { get; set; }
    }
}
