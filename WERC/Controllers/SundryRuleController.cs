using BLL;
using System.Web.Mvc;
using System;
using Model.ViewModels.ParticipantRule;
using Model.ViewModels.SundryRule;
using Newtonsoft.Json;
using WERC.Models.CustomModelBinding;

namespace WERC.Controllers
{
    public class SundryRuleController : BaseController
    {

        [HttpGet]
        [ActionName("gpbf")]
        public JsonResult GetTeamMembersByFilter(VmSundryRule filterItem = null)
        {
            var blSundryRule = new BLSundryRule();
            var sundryRuleList = blSundryRule.GetSundryRulesByFilter(filterItem);

            return Json(sundryRuleList, JsonRequestBehavior.AllowGet);
        }

        
        [ActionName("cpr")]
        [HttpPost]
        public ActionResult CreateSundryRule(VmSundryRule model)
        {
            var result = -1;
            var blSundryRule = new BLSundryRule();

            result = blSundryRule.CreateSundryRule(model);


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
                sundryRuleId = model.Id,
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UpdateSundryRule(VmSundryRule model)
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
                        sundryRuleId = model.Id,
                        success = false,
                        message = message + "\n" + jsonEx
                    };

                    return Json(jsonException, JsonRequestBehavior.AllowGet);
                }

                var result = true;
                var blSundryRule = new BLSundryRule();

                result = blSundryRule.UpdateSundryRule(model);

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
                    sundryRuleId = model.Id,
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
                    sundryRuleId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("dpr")]
        [HttpPost]
        public ActionResult DeleteSundryRule(VmSundryRule model)
        {
            var result = true;
            var blSundryRule = new BLSundryRule();

            result = blSundryRule.DeleteSundryRule(model.Id);


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