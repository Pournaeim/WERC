using BLL;
using System.Web.Mvc;
using System;
using Model.ViewModels.ParticipantRule;
using Model.ViewModels.SubmissionRule;
using Newtonsoft.Json;
using WERC.Models.CustomModelBinding;

namespace WERC.Controllers
{
    public class SubmissionRuleController : BaseController
    {

        [HttpGet]
        [ActionName("gpbf")]
        public JsonResult GetTeamMembersByFilter(VmSubmissionRule filterItem = null)
        {
            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRulesByFilter(filterItem);

            return Json(submissionRuleList, JsonRequestBehavior.AllowGet);
        }


        [ActionName("cpr")]
        [HttpPost]
        public ActionResult CreateSubmissionRule(VmSubmissionRule model)
        {
            var result = -1;
            var blSubmissionRule = new BLSubmissionRule();

            result = blSubmissionRule.CreateSubmissionRule(model);

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
                submissionRuleId = model.Id,
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UpdateSubmissionRule(VmSubmissionRule model)
        {
            var message = "";

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    var jsonEx = JsonConvert.SerializeObject(ModelState, Formatting.Indented,
                //               new JsonSerializerSettings
                //               {
                //                   ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                //               });

                //    var jsonException = new
                //    {
                //        submissionRuleId = model.Id,
                //        success = false,
                //        message = message + "\n" + jsonEx
                //    };

                //    return Json(jsonException, JsonRequestBehavior.AllowGet);
                //}

                var result = true;
                var blSubmissionRule = new BLSubmissionRule();

                result = blSubmissionRule.UpdateSubmissionRule(model);

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
                    submissionRuleId = model.Id,
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
                    submissionRuleId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("dpr")]
        [HttpPost]
        public ActionResult DeleteSubmissionRule(VmSubmissionRule model)
        {
            var result = true;
            var blSubmissionRule = new BLSubmissionRule();

            result = blSubmissionRule.DeleteSubmissionRule(model.Id);


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