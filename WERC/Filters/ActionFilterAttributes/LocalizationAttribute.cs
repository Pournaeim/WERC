using WERC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WERC.Filters.ActionFilterAttributes
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //if (filterContext.RequestContext.HttpContext.Session["jimakb"] == null && filterContext.RequestContext.HttpContext.Request.QueryString["jimakb"] == null)
            //{
            //    var result = new ViewResult();

            //    result.ViewData.Model = new VMHandleErrorInfo("System is under construction...");
            //    result.ViewName = "ErrorWithoutLayout";

            //    filterContext.Result = result;

            //}
            //else
            //    filterContext.RequestContext.HttpContext.Session["jimakb"] = 1;

            var culture = filterContext.RequestContext.RouteData.Values["Lang"] ?? (filterContext.HttpContext.Session["lang"] ?? "en-US");

            try
            {
                filterContext.HttpContext.Session["lang"] = culture;
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture.ToString());
            }
            catch (Exception)
            {
                throw new NotSupportedException($"Invalid language code '{culture}'.");
            }
        }
    }
}