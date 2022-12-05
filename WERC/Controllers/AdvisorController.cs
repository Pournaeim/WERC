using BLL;

using Model.ViewModels.Advisor;
using Model.ViewModels.MealType;
using Model.ViewModels.Team;
using Model.ViewModels.TeamSafetyItem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers.Advisor
{
    [RoleBaseAuthorize(SystemRoles.Advisor, SystemRoles.CoAdvisor, SystemRoles.Leader)]
    public class AdvisorController : BaseController
    {
        // GET: Advisor
        public ActionResult Index()
        {
            return View(new VmAdvisor());
        }
        [HttpGet]
        [ActionName("ups_tfim")]
        public ActionResult UpdatePayStatusTeamFullInfoManagement()
        {
            var blInvoice = new BLInvoice();

            var invoiceList = blInvoice.GetInvoiceIds(false);

            if (invoiceList != null)
            {
                foreach (var invoiceId in invoiceList.Ids)
                {
                    int? lastOrderId = 0;

                    var blShopCart = new BLShopCart();
                    var lastOrderInfo = blShopCart.GetCheckoutStatus(invoiceId, out lastOrderId);

                    if (lastOrderInfo != null)
                    {
                        //Update all data in one transaction
                        blInvoice.UpdateInvoiceOrderStatus(lastOrderInfo, invoiceId, true, lastOrderId.Value, true, true);
                    }
                }
            }

            return View("NoneAdminTeamFullInfoManagement", new VmTeamFullInfoManagement());
        }

        [HttpGet]
        [ActionName("gesp")]
        public ActionResult GetESP(int id)
        {
            var blSafetyItem = new BLSafetyItem();
            var blTeamMember = new BLTeamMember();
            var teamId = id;
            var blTeamSafetyItem = new BLTeamSafetyItem();
            var vmTeamSafetyItemList = blTeamSafetyItem.GetTeamSafetyItemByTeamId(teamId);
            var blReference = new BLReference();

            var safetyItemList = blSafetyItem.GetAllSafetyItems(id);

            return View("ExperimentalSafetyPlan",
                new VmTeamSafetyItemManagement
                {
                    TeamSafetyItemCollection = new VmTeamSafetyItemCollection
                    {
                        SafetyItemList = safetyItemList,
                        TeamSafetyItemList = vmTeamSafetyItemList,
                        ReferenceFiles = blReference.GetAllReference(),
                        CurrentUserRoles = CurrentUserRoles

                    }
                });
        }

        [HttpGet]
        [ActionName("tl")]
        public ActionResult TeamList(int activeItemId = -1)
        {
            var bsTeam = new BLTeam();

            return View("TeamList", new VmTeamCollection
            {
                HtmlControlId = "Advisor_TeamList",
                DataAction = "ats",
                DataController = "Advisor",
                AllowDownlaod = true,
                AllowEdit = true,
                AllowDelete = true,
                ActiveItemId = activeItemId,
                Draggable = false,
                ShowSearchBox = false,
                ParentHtmlControlId = "TeamList_ParentHtmlControlId",
                OnItemSelected = "",
                TeamList = bsTeam.GetAdvisorTeams(CurrentUserId),
                RegisterForFlashTalk = new BLSystemSetting().GetSystemSettingById(2).Active,
            });
        }

        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonByUserId(CurrentUserId);

            vmPerson.OnActionSuccess = "loadAdvisorPanel";

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

        [HttpGet]
        [ActionName("ltef")]
        public ActionResult LoadTeamEditForm(int id)
        {
            var blTeam = new BLTeam();
            var team = blTeam.GetTeamById(id);

            team.OnActionSuccess = "loadTeamList";

            return View("EditTeam", team);
        }

        [HttpGet]
        [ActionName("tmm")]
        public ActionResult TeamMemberManagement(int id = -1)
        {
            var blTeam = new BLTeam();
            var team = blTeam.GetTeamById(id);
            var teamFullInfo = blTeam.GetTeamFullInfoById(id);
            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeam(team.Id);

            return View("TeamMemberManagement",
                new VmTeamMemberManagement
                {
                    TeamId = id,
                    TeamName = team.Name,
                    Task = team.Task,
                    PreliminaryReportUrl = team.PreliminaryReportUrl,
                    FlashTalkReportUrl = team.FlashTalkReportUrl,

                    BrochureUrl = team.BrochureUrl,
                    AwardNominationUrl = team.AwardNominationUrl,

                    OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl,
                    WrittenReportUrl = team.WrittenReportUrl,
                    SubmissionRuleList = submissionRuleList,

                    Preliminary = team.Preliminary,
                    OpenTaskTestPlan = team.OpenTaskTestPlan,
                    RegistrationStatus = teamFullInfo.RegistrationStatus,
                    PayStatus = teamFullInfo.PayStatus,
                    ProjectTitle = teamFullInfo.ProjectName,
                });
        }

        [HttpPost]
        [ActionName("ats")]
        public PartialViewResult SearchTeam(
            bool draggable,
            string OnItemSelected,
            bool allowDownlaod,
            bool allowEdit,
            bool showSearchBox,
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
            var teamList = bsTeam.GetAdvisorTeams(CurrentUserId, teamName);

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
                    ShowSearchBox = showSearchBox,
                    SearchText = teamName,
                    TeamList = teamList,
                    HtmlControlId = htmlControlId,
                    ParentHtmlControlId = ParentHtmlControlId,
                    OnItemSelected = OnItemSelected,
                    OnItemDragged = onItemDragged
                });
        }

        [HttpPost]
        [ActionName("sefftr")]
        public ActionResult SendEmailForFlashTalkRegistration(int teamId)
        {
            var result = true;
            var message = "Operation succeeded";

            var blTeam = new BLTeam();
            var team = blTeam.GetTeamById(teamId);

            BLPerson blPerson = new BLPerson();
            var adminEmail = blPerson.GetUsersByRoleNames(new string[] { "Admin" }).First().Email;


            adminEmail = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().Email;

            emailHelper = new EmailHelper
            {
                AdminEmail = adminEmail,
                SpecialEmail = specialEmail,
                SendInBcc = true,
                Subject = "Flash Talk registration",
                Body = $"{team.Name}, {team.Task} from {team.University} has registered to participate in Flash Talk",
                IsBodyHtml = true,
                EmailList = new string[] { adminEmail }
            };

            emailHelper.CurrentUserId = CurrentUserId;
            result = emailHelper.Send();

            var blTeamMember = new BLTeamMember();
            var teamMemberEmails = blTeamMember.GetTeamMembersEmails(teamId);
            emailHelper = new EmailHelper
            {
                AdminEmail = "",
                SpecialEmail = specialEmail,
                SendInBcc = true,
                Subject = "Flash Talk registration",
                Body = $"Thank you for registration your team {team.Name}, for the 2023 Flash Talk.<br/> " +
                $"We will contact you with more information.<br/>" +
                $"Please see the Team Manual for details about the Flash Talk.<br/>",
                IsBodyHtml = true,
                EmailList = teamMemberEmails
            };

            emailHelper.CurrentUserId = CurrentUserId;
            result = emailHelper.Send();

            if (result == false)
            {
                message = "Operation has been failed" + "\n" +
                   emailHelper.ErrorMessage;
            }
            else
            {
                blTeam.UpdateTeamRegisterForFlashTalk(teamId, true);
            }


            var jsonResult = new
            {
                result,
                success = result,
                message,
            };


            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }


    }
}
