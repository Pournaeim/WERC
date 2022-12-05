
using Model.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Model.ViewModels.Task
{
    public class VmTask : BaseViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string[] Grades { get; set; }
        public int[] GradeIds { get; set; }
        public string ClientGradeIds { get; set; }
        public string[] Tests { get; set; }
        public int[] TestIds { get; set; }
        public string ClientTestIds { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase UploadedDocument { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        [Required]
        public string Description { get; set; }
        public string Checked { get; set; }
        public int Priority { get; set; }
        public bool Preliminary { get; set; }
        public bool OpenTaskTestPlan { get; set; }
        public List<VmUserTask> UserTasks { get; set; }
        public int? PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeDescription { get; set; }
        public bool RegisterForFlashTalk { get; set; }
    }
}