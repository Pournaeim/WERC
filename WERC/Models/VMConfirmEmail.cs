using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WERC.Models
{
    public class VMConfirmEmail : BaseViewModel
    {
        public VMConfirmEmail()
        { }
        public VMConfirmEmail(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
