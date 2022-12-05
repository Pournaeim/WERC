
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.Grade
{
    public class VmTotalScoreData
    {
        public int TeamId { get; set; }
        public bool CurrentJudgeHasResult { get; set; }
        public int JudgeCount { get; set; }
        public int GradeId { get; set; }
        public double? TotalScore { get; set; }
    }
}