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
    
    public partial class EmailLog
    {
        public int Id { get; set; }
        public string RecepientId { get; set; }
        public string SenderId { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachUrl { get; set; }
    }
}
