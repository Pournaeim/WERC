
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.Grade.Grading
{
    public class VmGradingDetail
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string EvaluationItem { get; set; }
        public double MaxPoint { get; set; }
        public double? Point { get; set; }
        public double Coefficient { get; set; }
        public string Description{ get; set; }
        public object Signature { get; set; }
        public string ABETName { get; set; }
    }
}