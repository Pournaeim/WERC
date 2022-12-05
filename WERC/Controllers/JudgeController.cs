
using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Model.ViewModels.EmailLog;
using Model.ViewModels.Judge;
using Model.ViewModels.MealType;
using Model.ViewModels.Task;
using Model.ViewModels.Team;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers.Judge
{
    [RoleBaseAuthorize(SystemRoles.Judge, SystemRoles.Admin)]
    public class JudgeController : BaseController
    {
        // GET: Judge
        public ActionResult Index()
        {
            return View(new VmJudge());
        }

        [HttpGet]
        [ActionName("lnag")]
        public ActionResult NoneAnsweredGrades()
        {
            var blGrade = new BLGrade();
            var noneAnsweredGradegReports = blGrade.GetNoneAnsweredGradeReportListBasedOnJudge().Where(j => j.JudgeUserId == CurrentUserId);

            return View("NoneAnsweredGrades", new VmNoneAnsweredGradeManagement
            {
                JudgeBaseGrades = noneAnsweredGradegReports
            });
        }

        [ActionName("lttf")]
        [HttpPost]
        public PartialViewResult LoadTeamTestTableForm(int teamId)
        {
            var blTest = new BLTest();
            var teamTestCollection = blTest.GetTeamTaskTestByTeam(teamId);
            var userIds = teamTestCollection.TeamTestList.First().TestList.Select(i => i.UserId).Distinct().ToList();
            teamTestCollection.UserIds = userIds;
            return PartialView("_TeamTestTableJudge", teamTestCollection);
        }

        [ActionName("gjfibf")]
        [HttpGet]
        public JsonResult GetJudgeFullInfoByFilter(VmJudgeFullInfo filterItem = null)
        {
            var blPerson = new BLPerson();

            var judgeFullInfoList = blPerson.GetJudgeFullInfoByFilter(filterItem).ToList();

            return Json(judgeFullInfoList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("sjat")]
        [HttpPost]
        public async Task<JsonResult> SubmitJudgesAssignedTasks(string[] userIdList)
        { //var domainName = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host;
            var mealTypeList = "";
            var blUserTask = new BLUserTask();
            var userTaskList = blUserTask.GetUserTasksByUsers(userIdList);

            var domainName = "33rd WERC Environmental Design Contest 2023";

            var subject = "Task Assignment for the 33rd WERC Design Contest";
            var body = "";

            foreach (var userId in userIdList)
            {
                var usermealTypeList = from u in userTaskList where u.UserId == userId select u;
                var fullName = usermealTypeList.First().Name;

                mealTypeList = string.Join(",", usermealTypeList.Select(t => t.TaskName));

                body = "<h1>" + domainName + "</h1>" +
                "Dear " + fullName + ",<br/>" +
                "<br/>" +

                "I would like to thank you in advance for your time and effort that you will put into this design contest.<br/>" +

                "Tasks has been assigned to judges.We've worked so hard trying to accommodate as much as we could your preferences.<br/> " +

                "You will be assigned to judge teams in the following task(s): <strong>" + mealTypeList + "</strong><br/>" +

                "Each team must submit a single PDF of the complete report(including the audits) no later than March 25th, 2023.Late reports will be penalized at a rate of 25 points per day.<br/>" +

                "The guidelines sent to teams about the criteria of their written report can be seen " +
                "<a href='https://iee.nmsu.edu/outreach/events/international-environmental-design-contest/guidelines/written-report/'>here</a>.<br/>" +

                "Another email will be sent shortly to discuss in detail the scoring process.<br/>" +

                "Once again, thank you.<br/>" +
                "<br/>" +
                "Kind Regards, <br/>" +
                "WERC team <br/>" +

                "<br/>" +
                "Environmental Design Contest <br/>" +

                "<br/>" +
                "Phone: 575-646-8171<br/>" +
                "Fax:     575-646-5440<br/>" +

                "<br/>" +
                "College of Engineering<br/>" +
                "1025 Stewart Street<br/>" +
                "MSC Eng NM<br/>" +
                "New Mexico State University<br/>" +
                "P.O. Box 30001<br/>" +
                "Las Cruces, NM 88003-8001<br/>" +

                "<br/>" +
                "<h5 style='color:#8c0b42'>BE BOLD. Shape the Future.</h5>" +
                "<strong style='color:#8c0b42'>New Mexico State University</strong><br/>";

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

            CheckSuppressScoring();

            var bsTeam = new BLTeam();
            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetAllSubmissionRule().ToList();

            return View("TeamList", new VmTeamCollection
            {
                HtmlControlId = "Judge_TeamList",
                DataAction = "jts",
                DataController = "Judge",
                AllowDownlaod = false,
                AllowEdit = false,
                AllowDelete = false,
                ActiveItemId = activeItemId,
                Draggable = false,
                ShowSearchBox = true,
                ParentHtmlControlId = "TeamList_ParentHtmlControlId",
                OnItemSelected = "",
                //TeamList = bsTeam.GetMemberUserTeamsByTaskConfirm(CurrentUserId, true)
                TeamList = bsTeam.GetJudgeTeams(CurrentUserId),
                SubmissionRuleList = submissionRuleList,

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
            //var teamList = bsTeam.GetMemberUserTeamsByTaskConfirm(CurrentUserId, true, teamName);
            var teamList = bsTeam.GetJudgeTeams(CurrentUserId, teamName);
            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetAllSubmissionRule();

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
                    OnItemDragged = onItemDragged,
                    SubmissionRuleList = submissionRuleList,

                });
        }

        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonProfileByUserId(CurrentUserId);

            vmPerson.HideEmergency = false;
            vmPerson.OnActionSuccess = "loadJudgePanel";

            #region Goals

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

            #endregion Goals

            #region Task
            var bsTask = new BLTask();

            var clientTaskIds = vmPerson.ClientTaskIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var allTaskList = bsTask.GetTaskByPreferenceList(true);

            if (clientTaskIds.Count() > 0)
            {

                var TempTaskIds = new int[allTaskList.Count()];

                for (var i = 0; i < clientTaskIds.Length; i++)
                {
                    TempTaskIds[i] = int.Parse(clientTaskIds[i]);
                }

                var orderedTaskList = new List<VmTask>();

                foreach (var item in clientTaskIds)
                {
                    var task = allTaskList.Where(t => t.Id == int.Parse(item)).First();
                    task.Checked = "checked";
                    orderedTaskList.Add(task);
                }

                foreach (var item in allTaskList)
                {
                    if (orderedTaskList.Where(t => t.Id == item.Id).Count() == 0)
                    {
                        orderedTaskList.Add(item);
                    }
                }

                if (allTaskList.Count() > 0)
                {
                    vmPerson.TaskList = orderedTaskList;
                }
            }
            else
            {
                vmPerson.TaskList = allTaskList;
            }

            #endregion Task

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

        #region Grading Report

        [HttpGet]
        [ActionName("fgrm")]
        public ActionResult LoadFinalGradesReport()
        {
            var blGrade = new BLGrade();
            /// var gradegReportList = blGrade.GetGradeReportList(CurrentUserId);

            int gradeId = int.Parse(WebConfigurationManager.AppSettings["GradeId"]);

            var gradegReportList = blGrade.GetGradeReportListWithoutGradeId(CurrentUserId, gradeId);

            ///var currentJudgeGradegReportList = blGrade.GetCurrentJudgeGradeReportList(CurrentUserId);

            var currentJudgeGradegReportList = blGrade.GetCurrentJudgeGradeReportListWithoutGradeId(CurrentUserId, gradeId);
            ViewBag.BS_Title = "Final Score";

            return View("FinalGradeReportManagement", new VmFinalGradeReportManagement
            {
                CurrentUserId = CurrentUserId,
                GradeReportList = gradegReportList,
                CurrentJudgeGradeReportList = currentJudgeGradegReportList
            });
        }

        [HttpGet]
        [ActionName("bsfgrm")]
        public ActionResult BenchScaleLoadFinalGradesReport()
        {
            var blGrade = new BLGrade();
            int gradeId = int.Parse(WebConfigurationManager.AppSettings["GradeId"]);
            var gradegReportList = blGrade.BenchScaleGetGradeReportList(CurrentUserId, gradeId);
            var currentJudgeGradegReportList = blGrade.BenchScaleGetCurrentJudgeGradeReportList(CurrentUserId, gradeId);

            ViewBag.BS_Title = "Bench Scale Quality Final Score";

            return View("FinalGradeReportManagement", new VmFinalGradeReportManagement
            {
                CurrentUserId = CurrentUserId,
                GradeReportList = gradegReportList,
                CurrentJudgeGradeReportList = currentJudgeGradegReportList
            });
        }

        [HttpPost]
        [ActionName("lged")]
        [RoleBaseAuthorize(SystemRoles.Judge, SystemRoles.Admin, SystemRoles.Student, SystemRoles.Advisor, SystemRoles.CoAdvisor, SystemRoles.Leader)]
        public PartialViewResult LoadGradingEvaluationDetail(int taskId, int teamId, int gradeId)
        {
            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetGradingEvaluationDetail(taskId, teamId, gradeId).ToList();

            return PartialView("_FinalGradesReportDetailTable", new VmGradeDetailManagement
            {
                CurrentUserId = CurrentUserId,
                GradeDetailList = teamGradeDetailList
            });
        }

        #endregion Grading Report
    }
}
