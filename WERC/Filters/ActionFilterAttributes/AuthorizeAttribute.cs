using Model.ApplicationDomainModels;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WERC.AppDomainHelper;
using Model.Base;
using System.Threading;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Filters.ActionFilterAttributes
{
    public class RoleBaseAuthorizeAttribute : ActionFilterAttribute
    {
        SystemRoles[] Roles { get; set; }
        public RoleBaseAuthorizeAttribute(params SystemRoles[] roles)
        {
            Roles = roles;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string userName = filterContext.HttpContext.User.Identity.Name;

             
            bool isStudentised = AppAccessControl.CheckRoleAccessRight(userName, Roles);

            if (!isStudentised)
            {
                //This will redirect the user to the login page
                //You could use a view displaying an error message
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}
