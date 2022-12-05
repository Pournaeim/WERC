using BLL;
using Model.ViewModels.Leader;
using Model.ViewModels.MealType;
using Model.ViewModels.Team;
using Model.ViewModels.TeamSafetyItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WERC.Filters.ActionFilterAttributes;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers.Leader
{
    [RoleBaseAuthorize(SystemRoles.Leader)]
    public class LeaderController : BaseController
    {
        // GET: Leader
        public ActionResult Index()
        { 
            return View(new VmLeader());
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
        public ActionResult GetESP()
        {
            var blSafetyItem = new BLSafetyItem();
            var blTeamMember = new BLTeamMember();
            var teamId = blTeamMember.GetTeamMemberByUserId(CurrentUserId).TeamId;
            var blTeamSafetyItem = new BLTeamSafetyItem();
            var vmTeamSafetyItemList = blTeamSafetyItem.GetTeamSafetyItemByTeamId(teamId);
            var blReference = new BLReference();

            var safetyItemList = blSafetyItem.GetAllSafetyItems(teamId);

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

            return View("../Leader/TeamList", new VmTeamCollection
            {
                HtmlControlId = "Leader_TeamList",
                DataAction = "ats",
                DataController = "Leader",
                AllowDownlaod = true,
                AllowEdit = true,
                AllowDelete = true,
                ActiveItemId = activeItemId,
                Draggable = false,
                ShowSearchBox = false,
                ParentHtmlControlId = "TeamList_ParentHtmlControlId",
                OnItemSelected = "",
                TeamList = bsTeam.GetTeamsByLeader(CurrentUserId)
            });
        }

        [HttpGet]
        [ActionName("tmm")]
        public ActionResult TeamMemberManagement()
        {
            var blTeam = new BLTeam();
            int teamId = blTeam.GetLeaderTeam(CurrentUserId);

            var team = blTeam.GetTeamById(teamId);
            var teamFullInfo = blTeam.GetTeamFullInfoById(teamId);
            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeam(teamId);

            return View("../Leader/TeamMemberManagement",
                new VmTeamMemberManagement
                {
                    TeamId = teamId,
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

                    CurrentUserId = CurrentUserId,
                });

        }

        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonByUserId(CurrentUserId);

            vmPerson.OnActionSuccess = "loadLeaderPanel";

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

    }
}
