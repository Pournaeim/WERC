using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Microsoft.AspNet.Identity;

using Model.ViewModels;

using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using WERC.Models;

using static WERC.AppDomainHelper.StaticObjects;

namespace WERC.Controllers.SkilledWorkers
{
    public class HomeController : BaseController
    {

        [ActionName("uhef")]
        [HttpPost]
        public JsonResult SaveUploadedFile(HttpPostedFileBase file)
        {
            var imageUrl = string.Empty;

            imageUrl = $"{Request.Url.Scheme}{System.Uri.SchemeDelimiter}{Request.Url.Authority}" + UIHelper.UploadFile(file, "/Resources/Uploaded/");
            //imageUrl = "werc.nmsu.edu" + UIHelper.UploadFile(file, "/Resources/Uploaded/");

            return Json(imageUrl, JsonRequestBehavior.AllowGet);

        }

        //
        // Get: /Account/LogOff
        [ActionName("serrp")]
        [HttpGet]
        public ActionResult ShowErrorPage(int id)
        {

            return View("ErrorWithoutLayout", new VMHandleErrorInfo("Request refused..."));

        }
        //
        // Get: /Account/LogOff
        [ActionName("lo")]
        [HttpGet]
        public ActionResult LogOut(int id)
        {

            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public ActionResult GetActiveUsers()
        {

            var jsonResult = new
            {
                activeUserCount = ActiveUsers.Count,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult tabClosed()
        {

            var jsonResult = new
            {
                activeUserCount = ActiveUsers.Count,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {

            string URL = HttpContext.Request.Url.Host.ToString();

            var blSiteInfo = new BLPerson();
            blSiteInfo.CreateSiteInfo("Home: " + URL);

            var SetWelcomMessage = Request.QueryString["SetWelcomMessage"] != null ? bool.Parse(Request.QueryString["SetWelcomMessage"]) : false;


            if (SetWelcomMessage == true)
            {
                return View("Home", new VmHome()
                {
                    MostSetWelcomeMessage = false,


                });

            }

            return View("Home", new VmHome()
            {

            });
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
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

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
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
        public ActionResult studentList()
        {
            return View();
        }
        [ActionName("gbi")]
        [HttpPost]
        //[OutputCache(Duration = 20, VaryByParam = "id;city", Location = OutputCacheLocation.Server)]
        public ActionResult GetStudent(string id, string name, string city = "")
        {
            Student student = new Student();
            string record = "";
            string indexRecord = "";

            var path = Server.MapPath("~/App_Data/");

            StreamReader StudentsStreamReader = new StreamReader(path + "/student.txt");
            StreamReader indexStreamReader = new StreamReader(path + "/IndexFile.txt");
            
            while (true)
            {

                indexRecord = indexStreamReader.ReadLine();

                if (indexRecord == id)
                {
                    break;
                }
                else if (indexRecord == null)
                {
                    break;
                }

            }

            if (indexRecord != null)
            {
                while (true)
                {
                    record = StudentsStreamReader.ReadLine();
                    if (record == null)
                    {
                        break;
                    }
                    string[] idFind = record.Split(',');

                    if (idFind[0] == id)
                    {

                        student.Id = idFind[0];
                        student.FirstName = idFind[1];
                        student.LastName = idFind[2];
                        student.Age = int.Parse(idFind[3]);
                    }
                }
            }

            StudentsStreamReader.Close();
            indexStreamReader.Close();

            return Json(student, JsonRequestBehavior.AllowGet);

        }

    }
    class Student
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
