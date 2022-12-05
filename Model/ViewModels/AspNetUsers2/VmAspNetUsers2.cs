using System;
using System.Collections.Generic;
using System.Web;

namespace Model.ViewModels.AspNetUsers2
{
    public class VmAspNetUsers2
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
