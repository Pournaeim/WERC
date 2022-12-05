using BLL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model.ApplicationDomainModels;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WERC.Controllers;
using WERC.Models;
using static Model.ApplicationDomainModels.ConstantObjects;
using static WERC.AppDomainHelper.StaticObjects;

namespace WERC.Filters.ActionFilterAttributes
{
    public class PreLoadDataActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = null;

            var controller = (filterContext.Controller as BaseController);
            if (controller != null)
            {
                controller.LoadLastModelStateErrors();

                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    user = userManager.Users.First(u => u.UserName == HttpContext.Current.User.Identity.Name || u.Id == HttpContext.Current.User.Identity.Name);

                    controller.CurrentUserId = user.Id;

                    if (controller.CurrentUserRoles == null)
                    {
                        try
                        {
                            controller.CurrentUserRoles = (from roles in SmUserRolesList.UserRoles where roles.UserName == HttpContext.Current.User.Identity.Name select roles.RoleName).AsEnumerable();

                        }
                        catch
                        {
                        }
                    }

                    var controllerValue = filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower();
                    var actionValue = filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower();

                    #region Active Users

                    HandelActiveUserSession(filterContext, controller);

                    #endregion Active Users


                    if (
                        filterContext.HttpContext.Request.QueryString["updateProfile"] == null
                        &&
                        controllerValue != "person" && actionValue != "up" && controllerValue != "acount" && actionValue != "logoff"
                        )
                    {
                        var blPerson = new BLPerson();
                        var person = blPerson.GetPersonByUserId(controller.CurrentUserId);

                        if ((person.RoleName.Contains("Admin") == false && person.Agreement == false) || string.IsNullOrEmpty(person.StreetLine1) || string.IsNullOrEmpty(person.City) || string.IsNullOrEmpty(person.ZipCode))
                        {
                            filterContext.Result = new RedirectResult("/" + person.RoleName + "/lupf/?updateProfile=true");
                        }
                        else
                        {
                            if (user.EmailConfirmed == false && controller.CurrentUserRoles != null &&
                                               (controller.CurrentUserRoles.Contains(SystemRoles.Advisor.ToString())
                                               ||
                                               controller.CurrentUserRoles.Contains(SystemRoles.Judge.ToString()))
                                               )
                            {
                                if (controllerValue != "home" && actionValue != "index" && controllerValue != "acount" && actionValue != "logoff"
                                        && controllerValue != "pagecontent" && actionValue != "gfpc")
                                {
                                    filterContext.Result = new RedirectResult("/Home/Index");
                                }
                            }

                        }
                    }

                }


            }
        }

        private static void HandelActiveUserSession(ActionExecutingContext filterContext, BaseController controller)
        {
            var controllerValue = filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            var actionValue = filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower();

            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session.IsNewSession)
                {
                    string cookieHeader = filterContext.HttpContext.Request.Headers["Cookie"];
                    if ((cookieHeader != null) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        if (filterContext.HttpContext.Request.IsAuthenticated)
                        {
                            if (ActiveUsers.Count > 0 && !string.IsNullOrEmpty(controller.CurrentUserId))
                            {
                                ActiveUsers.Remove(controller.CurrentUserId);
                            }

                            filterContext.HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                            filterContext.HttpContext.Session["WelcomeMessage"] = null;
                            filterContext.Result = new RedirectResult("/Account/Login");
                        }

                    }
                }
                else if (filterContext.HttpContext.Request.IsAuthenticated && !string.IsNullOrEmpty(controller.CurrentUserId) && !ActiveUsers.ContainsKey(controller.CurrentUserId))
                {
                    var blPerson = new BLPerson();
                    var person = blPerson.GetPersonByUserId(controller.CurrentUserId);
                    ActiveUsers.Add(controller.CurrentUserId, person);
                }
            }

            if (actionValue.ToLower() == "tabclosed" || actionValue.ToLower() == "logoff")
            {
                if (ActiveUsers.Count > 0 && !string.IsNullOrEmpty(controller.CurrentUserId))
                {
                    ActiveUsers.Remove(controller.CurrentUserId);
                }

            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

        }
    }
}