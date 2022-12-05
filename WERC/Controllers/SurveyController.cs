using BLL;
using Model.ViewModels.Survey;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WERC.Filters.ActionFilterAttributes;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers.Survey
{
    [RoleBaseAuthorize(SystemRoles.Student, SystemRoles.Judge, SystemRoles.Leader, SystemRoles.Advisor, SystemRoles.CoAdvisor)]
    public class SurveyController : BaseController
    {
        #region Survey

        [HttpGet]
        [ActionName("lsf")]
        public ActionResult LoadSurveyForm()
        {
            var blSurvey = new BLSurvey();
            var type = QuestionType.Faculty;
            var layout = "";

            if (CurrentUserRoles.Contains("Advisor"))
            {
                layout = "~/Views/Shared/_LayoutAdvisor.cshtml";
                type = QuestionType.Faculty;
            }

            if (CurrentUserRoles.Contains("Judge"))
            {
                layout = "~/Views/Shared/_LayoutJudge.cshtml";
                type = QuestionType.Judge;
            }

            if (CurrentUserRoles.Contains(SystemRoles.Student.ToString()))
            {
                layout = "~/Views/Shared/_LayoutStudent.cshtml";
                type = QuestionType.Student;
            }

            if (CurrentUserRoles.Contains(SystemRoles.Leader.ToString()))
            {
                layout = "~/Views/Shared/_LayoutLeader.cshtml";
                type = QuestionType.Student;
            }

            if (CurrentUserRoles.Contains(SystemRoles.CoAdvisor.ToString()))
            {
                layout = "~/Views/Shared/_LayoutCoAdvisor.cshtml";
                type = QuestionType.Faculty;
            }

            var surveyList = blSurvey.GetSurveyList(CurrentUserId, type);

            return View("SurveyManagement",
                new VmSurveyManagement
                {
                    CurrentUserId = CurrentUserId,
                    CurrentUserRoles = CurrentUserRoles,
                    SurveyList = surveyList,
                    ViewLayout = layout,
                });
        }

        [HttpPost]
        [ActionName("ss")]
        public ActionResult SaveSurvey(List<VmClientSurveyResult> clientSurveyResult)
        {
            var result = true;
            var blSurvey = new BLSurvey();
            result = blSurvey.UpdateSurvey(CurrentUserId, clientSurveyResult);

            var jsonData = new
            {
                success = result,
                message = ""
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion Survey
    }
}
