
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.Grade
{
    public class VmTeamGradeDetail : BaseViewModel
    {
        public int Id { get; set; }
        public string GradeDetailIds { get; set; }
        public int GradeId { get; set; }
        public string Grade { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string EvaluationItems { get; set; }
        public string MaxPoints { get; set; }
        public string Points { get; set; }
        public string Coefficients { get; set; }
        public int TaskId { get; set; }
        public DateTime Date { get; set; }
        public int? State { get; set; }
        public string ImageUrl { get; set; }
        public string LabResultUrl { get; set; }
        public string WrittenReportUrl { get; set; }
        public string EvaluationItem { get; set; }
        public double MaxPoint { get; set; }
        public double? Point { get; set; }
        public int? TeamNumber { get; set; }
        public string Description { get; set; }
        public string JudgeUserId { get; set; }
        public double Coefficient { get; set; }
        public string Signature { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public string PreliminaryReportUrl { get; set; }
        public string OpenTaskTestPlanUrl { get; set; }
        public int GradeDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}