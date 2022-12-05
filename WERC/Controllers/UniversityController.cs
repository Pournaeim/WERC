using BLL;
using Model.ViewModels.Admin;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace WERC.Controllers
{
    public class UniversityController : BaseController
    {
        [ActionName("guddl")]
        public ActionResult GetUniversityDropDownList()
        {
            var bsUniversity = new BLUniversity();

            var universityList = bsUniversity.GetUniversitySelectListItem(0, int.MaxValue);

            return Json(universityList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("gpbf")]
        public JsonResult GetTeamMembersByFilter(VmUniversity filterItem = null)
        {

            var blUniversity = new BLUniversity();
            var universityList = blUniversity.GetUniversitysByFilter(filterItem);

            return Json(universityList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("cpr")]
        [HttpPost]
        public ActionResult CreateUniversity(VmUniversity model)
        {
            var result = -1;
            var blUniversity = new BLUniversity();

            result = blUniversity.CreateUniversity(model);


            var message = "";
            if (result == -1)
            {
                message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                message = "Operation has been successful";
            }

            var jsonData = new
            {
                universityId = model.Id,
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UpdateUniversity(VmUniversity model)
        {
            var message = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    var jsonEx = JsonConvert.SerializeObject(ModelState, Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                               });

                    var jsonException = new
                    {
                        universityId = model.Id,
                        success = false,
                        message = message + "\n" + jsonEx
                    };

                    return Json(jsonException, JsonRequestBehavior.AllowGet);
                }

                var result = true;
                var blUniversity = new BLUniversity();

                result = blUniversity.UpdateUniversity(model);

                if (result == false)
                {
                    message += "Operation has been failed...\n call system Admin\n";
                }
                else
                {
                    message += "Operation has been successful";
                }

                var jsonData = new
                {
                    universityId = model.Id,
                    success = result,
                    message

                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var jsonEx = JsonConvert.SerializeObject(ex, Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                               });

                var jsonException = new
                {
                    universityId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("dpr")]
        [HttpPost]
        public ActionResult DeleteUniversity(VmUniversity model)
        {
            var result = true;
            var blUniversity = new BLUniversity();

            result = blUniversity.DeleteUniversity(model.Id);


            var message = "";
            if (result == false)
            {
                message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                message = "Operation has been successful";
            }

            var jsonData = new
            {
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}