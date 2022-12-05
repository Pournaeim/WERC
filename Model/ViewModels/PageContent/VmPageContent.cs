using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Model.ViewModels.PageContent
{
    public class VmPageContent : BaseViewModel
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Type { get; set; }
    }
}
