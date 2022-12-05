
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Model.ViewModels.Task
{
    public class VmTaskFullInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Grades { get; set; }
        public int GradeId { get; set; }
        public string Judges { get; set; }
        public string Description { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public bool? PreliminaryStatus { get; set; }
        public bool? OpenTaskTestPlanStatus { get; set; }
        public string AssignedTasks { get; set; }
        public string JudgesPreferred { get; set; }
        public bool? RegisterForFlashTalk { get; set; }
    }
}