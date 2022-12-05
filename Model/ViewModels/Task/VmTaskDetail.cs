
using Model.Base;
using System;
using System.Web.Mvc;

namespace Model.ViewModels.Task
{
    public class VmTaskDetail:BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SubjectId { get; set; }
        public string Subject { get; set; }
        public string ImageUrl { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string ISSN { get; set; }
    }
}