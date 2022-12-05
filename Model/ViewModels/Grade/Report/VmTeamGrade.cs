
using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.Grade.Report
{
    public class VmTeamGrade
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public List<VmGradeReport> GradeReportList { get; set; }

    }
}
