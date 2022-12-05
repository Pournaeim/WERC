using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.User
{
    public class VmUserFullInfo
    {
        public string Id { get; set; }

        [EmailAddress]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public IEnumerable<string> Roles { get; set; }
        
        public string RoleName { get; set; }

        [Display(Name = "Register Date")]
        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; }

        public bool EmailConfirmed { get; set; }
        public string FullName { get; set; }
        public string Tasks { get; set; }
    }
}
