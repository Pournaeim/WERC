using Model.ViewModels.InlineWebPageHandler;
using System.Web.Configuration;
using System.Web.Mvc;
using WERC.Models;

namespace WERC.Controllers
{
    public class InlineWebPageHandlerController : BaseController
    {
        // GET: InlineWebPageHandler
        [HttpPost]
        [AllowAnonymous]
        [ActionName("liwp")]
        public ActionResult LoadInlineWebPage(LoginViewModel model)
        {
           // string secret = WebConfigurationManager.AppSettings["InlineSiteSecurityKey"];

           // if (model.UserName == "Admin" && model.Password == secret)
            {
                ViewBag.InlineWebPageUrl = model.Url;
                return View("LoadInlineWebPage", new VmMousePositionCollection());
            }
            
            //return View("LoadInlineWebPageManagement", new VmMousePositionCollection());

        }
        // GET: InlineWebPageHandler
        [ActionName("liwpm")]
        public ActionResult LoadInlineWebPageManagement()
        {

            return View("LoadInlineWebPageManagement", new VmMousePositionCollection());
        }

        // POST: InlineWebPageHandler/Create
        [ActionName("siwpd")]
        [HttpPost]
        public ActionResult SaveInlineWebPageData(VmMousePosition mousePositionList)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: InlineWebPageHandler/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
