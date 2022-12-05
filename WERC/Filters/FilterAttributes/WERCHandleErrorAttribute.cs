using BLL;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace WERC.Filters.FilterAttributes
{
    public class WERCHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            controller.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            controller.Response.TrySkipIisCustomErrors = true;
            filterContext.ExceptionHandled = true;

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            //var controllerName = "error";
            //var actionName = "handelError";
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            try
            {
                var blSiteInfo = new BLPerson();
                blSiteInfo.CreateSiteInfo("Err: " + filterContext.Exception.Message + " \n " + (filterContext.Exception.InnerException) ?? "");

            }
            catch
            {

            }
            
            if (filterContext.Exception is HttpAntiForgeryException)
            {
                filterContext.Result = new RedirectToRouteResult(
                                 new RouteValueDictionary
                                 {
                                    { "action", "Login" },
                                    { "controller", "Account" }
                                 });

            }
            else
            {

                var view = new ViewResult
                {
                    ViewName = "Errorhandler",
                    ViewData = new ViewDataDictionary()
                };

                view.ViewData.Model = model;

                //copy any view data from original control over to error view
                //so they can be accessible.
                var viewData = controller.ViewData;
                if (viewData != null && viewData.Count > 0)
                {
                    viewData.ToList().ForEach(view.ViewData.Add);
                }

                //Instead of this
                ////filterContext.Result = view;
                //Allow the error view to display on the same URL the error occurred
                view.ExecuteResult(filterContext);
            }

        }
    }
}