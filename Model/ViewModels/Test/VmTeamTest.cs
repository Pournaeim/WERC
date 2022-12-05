
using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.Test
{
    public class VmTeamTest
    {
        public int TaskId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TaskName { get; set; }
        public List<VmTest> TestList { get; set; }

    }
}