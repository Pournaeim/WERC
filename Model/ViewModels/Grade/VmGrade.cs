
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.Grade
{
    public class VmGrade : BaseViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string EvaluationItems { get; set; }
        public string EvaluationItemIds { get; set; }
        public string DeletableEvaluationItemIds { get; set; }
        public string AddedEvaluationItemIds { get; set; }
        public string Points { get; set; }
        public string Coefficients { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public bool ABET { get; set; }
    }
}