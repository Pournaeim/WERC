using BLL;
using System.Web.Mvc;
using System;
using Model.ViewModels.ParticipantRule;
using Model.ViewModels.SystemSetting;
using Newtonsoft.Json;
using WERC.Models.CustomModelBinding;

namespace WERC.Controllers
{
    public class SystemSettingController : BaseController
    {

        [HttpGet]
        [ActionName("gptddl")]
        public JsonResult GetSystemSettingDropdownList()
        {
            var blSystemSetting = new BLSystemSetting();
            var systemSettingList = blSystemSetting.GetSystemSettingSelectListItem(0, int.MaxValue);

            return Json(systemSettingList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("gpbf")]
        public JsonResult GetTeamMembersByFilter(VmSystemSetting filterItem = null)
        {
            var blSystemSetting = new BLSystemSetting();
            var systemSettingList = blSystemSetting.GetSystemSettingsByFilter(filterItem);

            return Json(systemSettingList, JsonRequestBehavior.AllowGet);
        }


        [ActionName("cpr")]
        [HttpPost]
        public ActionResult CreateSystemSetting(VmSystemSetting model)
        {
            var result = -1;
            var blSystemSetting = new BLSystemSetting();

            result = blSystemSetting.CreateSystemSetting(model);


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
                systemSettingId = model.Id,
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UpdateSystemSetting(VmSystemSetting model)
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
                        systemSettingId = model.Id,
                        success = false,
                        message = message + "\n" + jsonEx
                    };

                    return Json(jsonException, JsonRequestBehavior.AllowGet);
                }

                var result = true;
                var blSystemSetting = new BLSystemSetting();

                result = blSystemSetting.UpdateSystemSetting(model);

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
                    systemSettingId = model.Id,
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
                    systemSettingId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("dpr")]
        [HttpPost]
        public ActionResult DeleteSystemSetting(VmSystemSetting model)
        {
            var result = true;
            var blSystemSetting = new BLSystemSetting();

            result = blSystemSetting.DeleteSystemSetting(model.Id);


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