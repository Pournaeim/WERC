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
    
    public partial class SubSafetyItem
    {
        public int Id { get; set; }
        public int SafetyItemId { get; set; }
        public string Name { get; set; }
        public string Instruction { get; set; }
        public int Priority { get; set; }
        public bool AttachmentRequired { get; set; }
        public bool TextRequired { get; set; }
    
        public virtual SafetyItem SafetyItem { get; set; }
    }
}
