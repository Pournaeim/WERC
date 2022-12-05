using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.User
{
    public class VmUserList : BaseViewModel
    {
        
        public List<VmUserFullInfo> Users { get; set; }
    }
}
