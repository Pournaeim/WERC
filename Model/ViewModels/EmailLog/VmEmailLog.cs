using Model.Base;
using Model.ViewModels.Ethnicity;
using Model.ViewModels.GoalsAfterGraduation;
using Model.ViewModels.Task;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.EmailLog
{
    public partial class VmEmailLog : BaseViewModel
    {
        public int Id { get; set; }
        public string RecepientId { get; set; }
        public string SenderId { get; set; }
        public DateTime? SendDate { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }

        [AllowHtml]
        public string Body { get; set; }
        public string AttachUrl { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderUserName { get; set; }
        public string SenderRoleName { get; set; }
        public string RecepientName { get; set; }
        public string RecepientEmail { get; set; }
        public string RecepientUserName { get; set; }
        public string RecepientRoleName { get; set; }
        public string ExcelFileUrl { get; set; }
    }
}
