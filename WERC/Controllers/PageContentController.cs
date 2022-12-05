using BLL;
using Model.Base;
using Model.ViewModels.PageContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WERC.Controllers
{
    public class PageContentController : BaseController
    {
        // GET: PageContent
        public ActionResult Index()
        {
            return View();
        }

        // GET: PageContent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PageContent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PageContent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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

        [ActionName("gfpc")]
        public IHtmlString GetFirstPageContent()
        {
            var blPageContent = new BLPageContent();
            var vmPageContent = blPageContent.GetById(1);

            return MvcHtmlString.Create(vmPageContent.Content);

        }

        [ActionName("glpc")]
        public IHtmlString GetLoginPageContent()
        {
            var blPageContent = new BLPageContent();
            var vmPageContent = blPageContent.GetById(2);

            return MvcHtmlString.Create(vmPageContent.Content);

        }

        [ActionName("gspc")]
        public IHtmlString GetSignUpPageContent()
        {
            var blPageContent = new BLPageContent();
            var vmPageContent = blPageContent.GetById(3);

            return MvcHtmlString.Create(vmPageContent.Content);

        }

        [ActionName("lfpc")]
        public ActionResult Edit()
        {
            return View("FirstPageContent", new VmPageContent
            {
                Id = 1,
                FormTitle = "Edit Announcement"
            });
        }

        [ActionName("llpc")]
        public ActionResult LoginPageContent()
        {
            return View("LoginPageContent", new VmPageContent
            {
                Id = 2,
                FormTitle = "Edit Login Page Content"
            });
        }

        [ActionName("lspc")]
        public ActionResult SignUpPageContent()
        {
            return View("SignUpPageContent", new VmPageContent
            {
                Id = 3,
                FormTitle = "Edit Sign Up Page Content"
            });
        }


        [ActionName("gfphc")]
        public JsonResult GetFirstPageHtmlContent()
        {
            var blPageContent = new BLPageContent();
            var vmPageContent = blPageContent.GetById(1);

            vmPageContent.FormTitle = "Home Page Content";

            return Json(vmPageContent.Content, JsonRequestBehavior.AllowGet);

        }

        [ActionName("glphc")]
        public JsonResult GetLoginPageHtmlContent()
        {
            var blPageContent = new BLPageContent();
            var vmPageContent = blPageContent.GetById(2);

            vmPageContent.FormTitle = "Edit Login Page Content";

            return Json(vmPageContent.Content, JsonRequestBehavior.AllowGet);

        }

        [ActionName("gsphc")]
        public JsonResult GetSignUpPageHtmlContent()
        {
            var blPageContent = new BLPageContent();
            var vmPageContent = blPageContent.GetById(3);

            vmPageContent.FormTitle = "Edit  Sign Up Page Content";

            return Json(vmPageContent.Content, JsonRequestBehavior.AllowGet);

        }
        // POST: PageContent/Edit/5

        [HttpPost]
        [ActionName("sfpc")]
        public ActionResult Edit(VmPageContent model)
        {
            try
            {
                var blPageContent = new BLPageContent();
                blPageContent.UpdatePageContent(model);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("slpc")]
        public ActionResult SaveLoginPageContent(VmPageContent model)
        {
            try
            {
                var blPageContent = new BLPageContent();
                blPageContent.UpdatePageContent(model);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("sspc")]
        public ActionResult SaveSignUpPageContent(VmPageContent model)
        {
            try
            {
                var blPageContent = new BLPageContent();
                blPageContent.UpdatePageContent(model);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: PageContent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PageContent/Delete/5
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
