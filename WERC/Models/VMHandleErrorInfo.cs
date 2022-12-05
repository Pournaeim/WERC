using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WERC.Models
{
    public class VMHandleErrorInfo : BaseViewModel
    {
        public VMHandleErrorInfo()
        { }
        public VMHandleErrorInfo(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public System.Web.Mvc.HandleErrorInfo HandleErrorInfo;
        public string ErrorMessage { get; set; }
        public string ViewLayout { get; set; }
    }
}
