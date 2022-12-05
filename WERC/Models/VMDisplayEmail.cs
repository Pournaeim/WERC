using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WERC.Models
{
    public class VMDisplayEmail : BaseViewModel
    {
        public VMDisplayEmail()
        { }
        public VMDisplayEmail(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
        public string RoleName { get; set; }
    }
}
