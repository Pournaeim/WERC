using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Model.ViewModels.EmailLog;
using Model.ViewModels.Lab;
using Model.ViewModels.MealType;
using Model.ViewModels.Team;
using Model.ViewModels.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;

using static Model.ApplicationDomainModels.ConstantObjects;
 
namespace WERC.Controllers.Lab
{
    [RoleBaseAuthorize(SystemRoles.Lab, SystemRoles.Admin)]
    public class LabController : BaseController
    {
        // GET: Lab
        public ActionResult Index()
        {
            return View(new VmLab());
        }

        #region Team Test
        [ActionName("lttmf")]
        [HttpGet]
        public ActionResult LoadTeamTestManagementForm()
        {
            return View("TeamTestManagementForm", new VmTeamTestManagement
            {
                CurrentUserId = CurrentUserId,
            });
        }

        [ActionName("lttf")]
        [HttpPost]
        public PartialViewResult LoadTeamTestTableForm(int taskId)
        {
            var blTest = new BLTest();
            var teamTestCollection = blTest.GetTeamTaskTest(CurrentUserId, taskId);

            return PartialView("_TeamTestTable", teamTestCollection);
        }

        [ActionName("st")]
        [HttpPost]
        public ActionResult SaveTest(VmTeamTestResult[] clientTest)
        {
            var result = true;
            var blTest = new BLTest();

            try
            {
                if (ModelState.IsValid)
                {
                    result = blTest.UpdateTest(CurrentUserId, clientTest);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                success = result,
                message = "",
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        #endregion Team Test

        [ActionName("sjat")]
        [HttpPost]
        public async Task<JsonResult> SubmitLabsAssignedTasks(string[] userIdList)
        {
            //var domainName = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host;
            var domainName = "33rd WERC Environmental Design Contest 2023";

            var subject = "Submitted your Assigned Tasks";
            var body = "<h1>" + domainName + "</h1>" +
                "<h2>Submitted your Assigned Tasks</h2>";

            foreach (var userId in userIdList)
            {
                await UserManager.SendEmailAsync(userId, subject, body);

                BLEmailLog bLEmailLog = new BLEmailLog();
                bLEmailLog.CreateEmailLog(new VmEmailLog
                {
                    RecepientId = userId,
                    SenderId = CurrentUserId,
                    SendDate = DateTime.Now,
                    Subject = subject,
                    Body = body,
                    AttachUrl = "",
                });

                emailHelper = new EmailHelper()
                {
                    EmailLog = false,
                    SpecialEmail = specialEmail,
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    EmailList = new string[] { specialEmail }
                };
                for (var i = 0; i < emailHelper.EmailList.Length; i++)
                {
                    emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                }
                emailHelper.CurrentUserId = CurrentUserId;
                emailHelper.Send();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("tl")]
        public ActionResult TeamList(int activeItemId = -1)
        {
            var bsTeam = new BLTeam();

            return View("TeamList", new VmTeamCollection
            {
                HtmlControlId = "Lab_TeamList",
                DataAction = "jts",
                DataController = "Lab",
                AllowDownlaod = false,
                AllowEdit = false,
                AllowDelete = false,
                ActiveItemId = activeItemId,
                Draggable = false,
                ShowSearchBox = true,
                ParentHtmlControlId = "TeamList_ParentHtmlControlId",
                OnItemSelected = "",
                TeamList = bsTeam.GetLabTeams(CurrentUserId)
            });
        }

        [HttpPost]
        [ActionName("jts")]
        public PartialViewResult SearchTeam(
            bool draggable,
            string OnItemSelected,
            bool allowDownlaod,
            bool allowEdit,
            bool allowReject,
            string onItemRejecting,
            bool allowAccept,
            string onItemAccepting,
            bool allowDelete,
            string htmlControlId,
            string dataAction,
            string dataController,
            string ParentHtmlControlId, string onItemDragged,
            string teamName = "")
        {
            var bsTeam = new BLTeam();
            var teamList = bsTeam.GetLabTeams(CurrentUserId, teamName);

            return PartialView("_TeamList",
                new VmTeamCollection
                {
                    DataAction = dataAction,
                    DataController = dataController,
                    AllowDownlaod = allowDownlaod,
                    AllowEdit = allowEdit,
                    AllowReject = allowReject,
                    OnItemRejecting = onItemRejecting,
                    AllowAccept = allowAccept,
                    OnItemAccepting = onItemAccepting,
                    AllowDelete = allowDelete,
                    Draggable = draggable,
                    ShowSearchBox = true,
                    SearchText = teamName,
                    TeamList = teamList,
                    HtmlControlId = htmlControlId,
                    ParentHtmlControlId = ParentHtmlControlId,
                    OnItemSelected = OnItemSelected,
                    OnItemDragged = onItemDragged
                });
        }
        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonByUserId(CurrentUserId);

            vmPerson.HideEmergency = false;
            vmPerson.OnActionSuccess = "loadLabPanel";


            var bsGoalsAfterGraduation = new BLGoalsAfterGraduation();
            var allGoalsAfterGraduationList = bsGoalsAfterGraduation.GetAllGoalsAfterGraduation();

            if (vmPerson.ClientGoalsAfterGraduationIds != null && vmPerson.ClientGoalsAfterGraduationIds.Count() > 0)
            {
                var clientGoalsAfterGraduationIds = vmPerson.ClientGoalsAfterGraduationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (clientGoalsAfterGraduationIds.Count() > 0)
                {

                    foreach (var item in clientGoalsAfterGraduationIds)
                    {
                        var goalsAfterGraduation = allGoalsAfterGraduationList.Where(t => t.Id == int.Parse(item)).First();
                        goalsAfterGraduation.Checked = "checked";
                    }

                    if (allGoalsAfterGraduationList.Count() > 0)
                    {
                        vmPerson.GoalsAfterGraduationList = allGoalsAfterGraduationList;
                    }
                }

            }
            else
            {
                vmPerson.GoalsAfterGraduationList = allGoalsAfterGraduationList;
            }


            #region Ethincity
            var bsEthnicity = new BLEthnicity();
            var allEthnicityList = bsEthnicity.GetAllEthnicity();

            if (vmPerson.ClientEthnicityIds != null && vmPerson.ClientEthnicityIds.Count() > 0)
            {
                var clientEthnicityIds = vmPerson.ClientEthnicityIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (clientEthnicityIds.Count() > 0)
                {

                    foreach (var item in clientEthnicityIds)
                    {
                        var goalsAfterGraduation = allEthnicityList.Where(t => t.Id == int.Parse(item)).First();
                        goalsAfterGraduation.Checked = "checked";
                    }

                    if (allEthnicityList.Count() > 0)
                    {
                        vmPerson.EthnicityList = allEthnicityList;
                    }
                }
            }
            else
            {
                vmPerson.EthnicityList = allEthnicityList;
            }
            #endregion Ethincity

            #region MealType
            var bsMealType = new BLMealType();

            var clientMealTypeIds = vmPerson.ClientMealTypeIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var allMealTypeList = bsMealType.GetAllMealType();

            if (clientMealTypeIds.Count() > 0)
            {

                var TempMealTypeIds = new int[allMealTypeList.Count()];

                for (var i = 0; i < clientMealTypeIds.Length; i++)
                {
                    TempMealTypeIds[i] = int.Parse(clientMealTypeIds[i]);
                }

                var orderedMealTypeList = new List<VmMealType>();

                foreach (var item in clientMealTypeIds)
                {
                    var mealType = allMealTypeList.Where(t => t.Id == int.Parse(item)).First();
                    mealType.Checked = "checked";
                    orderedMealTypeList.Add(mealType);
                }

                foreach (var item in allMealTypeList)
                {
                    if (orderedMealTypeList.Where(t => t.Id == item.Id).Count() == 0)
                    {
                        orderedMealTypeList.Add(item);
                    }
                }

                if (allMealTypeList.Count() > 0)
                {
                    vmPerson.MealTypeList = orderedMealTypeList;
                }
            }
            else
            {
                vmPerson.MealTypeList = allMealTypeList;
            }

            #endregion MealType


            return View("UpdateProfile", vmPerson);
        }
    }
}
