using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Model.ViewModels.Role
{
    public class VmRole : BaseViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
