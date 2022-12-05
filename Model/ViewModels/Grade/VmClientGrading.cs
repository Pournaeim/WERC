
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.Grade
{
    public class VmClientGrading
    {
        public int TeamId { get; set; }
        public int GradeDetailId { get; set; }
        public double? Point { get; set; }
        public string Description { get; set; }
        public string Signature { get; set; }
        public int GradeId { get; set; }
        public int? ABETId { get; set; }
        public string ABETName { get; set; }
    }
}