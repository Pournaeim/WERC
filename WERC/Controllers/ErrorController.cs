using System.Web.Mvc;
using WERC.Models;

namespace WERC.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {

            return View("Error", (VMHandleErrorInfo)TempData["vmHandleErrorInfo"]);
        }

    }
}