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
    
    public partial class TeamSafetyItemLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TeamSafetyItemId { get; set; }
        public string Content { get; set; }
        public string AttachedFileUrl { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<bool> Type { get; set; }
    
        public virtual TeamSafetyItem TeamSafetyItem { get; set; }
    }
}
