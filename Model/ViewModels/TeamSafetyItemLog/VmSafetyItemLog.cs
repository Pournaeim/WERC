using Model.Base;
using System;
using System.Web;

namespace Model.ViewModels.TeamSafetyItemLog
{
    public class VmSafetyItemLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TeamSafetyItemId { get; set; }
        public string Content { get; set; }
        public string AttachedFileUrl { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Type { get; set; }
    }
}
