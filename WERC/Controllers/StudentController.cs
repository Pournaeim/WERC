using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Model.ViewModels.Judge;
using Model.ViewModels.MealType;
using Model.ViewModels.SafetyItem;
using Model.ViewModels.Student;
using Model.ViewModels.Team;
using Model.ViewModels.TeamSafetyItem;
using Model.ViewModels.TeamSafetyItemLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;
using WERC.Models;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers.Student
{
    [RoleBaseAuthorize(SystemRoles.Student, SystemRoles.Leader, SystemRoles.Advisor, SystemRoles.CoAdvisor)]
    public class StudentController : BaseController
    {
        // GET: Student
        [RoleBaseAuthorize(SystemRoles.Student)]
        public ActionResult Index()
        {
            return View(new VmStudent());
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
        [ActionName("tmm")]
        public ActionResult TeamMemberManagement()
        {
            var blTeam = new BLTeam();
            int teamId = blTeam.GetLeaderTeam(CurrentUserId);

            var team = blTeam.GetTeamById(teamId);
            var teamFullInfo = blTeam.GetTeamFullInfoById(teamId);
            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeam(teamId);

            return View("../Student/TeamMemberManagement",
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
                    ProjectTitle = team.ProjectName,
                });

        }

        [HttpPost]
        [ActionName("scl")]
        public PartialViewResult GetSafetyLog(int subSafetyItemId, int teamId, bool type)
        {
            var blTeamSafetyItemLog = new BLTeamSafetyItemLog();

            var teamSafetyItemLogList = blTeamSafetyItemLog.GetTeamSafetyItemLog(subSafetyItemId, teamId, type);

            return PartialView("_TeamSafetyItemLog", new VmTeamSafetyItemLogCollection
            {
                SafetyItemLogList = teamSafetyItemLogList
            });
        }

        [HttpGet]
        [ActionName("lupf")]
        [RoleBaseAuthorize(SystemRoles.Student)]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonByUserId(CurrentUserId);

            vmPerson.OnActionSuccess = "loadStudentPanel";

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

        #region ESP

        [ActionName("gclddl")]
        public ActionResult GetTaskDropDownList(int teamId, int safetyItemId)
        {
            var blSafetyItem = new BLSafetyItem();


            var safetyItemDetailList = blSafetyItem.GetSafetyItemDetailSelectListItem(teamId, safetyItemId);

            return Json(safetyItemDetailList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("gesp")]
        public ActionResult GetESP()
        {
            var blSafetyItem = new BLSafetyItem();
            var blTeamMember = new BLTeamMember();
            var blTeamSafetyItem = new BLTeamSafetyItem();
            var blReference = new BLReference();

            var teamId = blTeamMember.GetTeamMemberByUserId(CurrentUserId).TeamId;
            var vmTeamSafetyItemList = blTeamSafetyItem.GetTeamSafetyItemByTeamId(teamId);

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

        [HttpPost]
        [ActionName("ssi")]

        public ActionResult SaveSafetyItem(VmTeamSaveSafetyItem teamSaveSafetyItem)
        {
            var vmSafetyItem = new VmSafetyItem();
            var result = true;
            var blTeamSafetyItem = new BLTeamSafetyItem();

            string attachedFileUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    if (teamSaveSafetyItem.UploadedAttachedFile != null)
                    {
                        attachedFileUrl = UIHelper.UploadFile(teamSaveSafetyItem.UploadedAttachedFile, "/Resources/Uploaded/TeamSafetyItems/AttachedFile/" + CurrentUserId.Replace("-", ""));
                    }
                    else
                    {
                        attachedFileUrl = teamSaveSafetyItem.OldAttachedFileUrl;
                    }

                    blTeamSafetyItem.UpdateTeamSafetyItem(new VmTeamSafetyItem
                    {
                        TeamId = teamSaveSafetyItem.TeamId,
                        SafetyItemId = teamSaveSafetyItem.SafetyItemId,
                        SubSafetyItemId = teamSaveSafetyItem.SubSafetyItemId,
                        LastContent = teamSaveSafetyItem.DescriptionContent ?? "",
                        ItemStatus = 0,
                        AttachedFileUrl = attachedFileUrl,
                        Type = false,
                        UserId = CurrentUserId,
                        SafetyItemDetailIds = teamSaveSafetyItem.ChecklistIds,
                    });

                    var blSafetyItem = new BLSafetyItem();
                    vmSafetyItem = blSafetyItem.GeTeamParentSafetyItemStatus(teamSaveSafetyItem.TeamId, teamSaveSafetyItem.SafetyItemId);
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(attachedFileUrl))
            //{
            //    UIHelper.DeleteFile(teamSaveSafetyItem.OldAttachedFileUrl);
            //}

            var jsonData = new
            {
                attachedFileUrl,
                success = result,
                vmSafetyItem.SafetyItemStatus,
                safetyItemId = teamSaveSafetyItem.SafetyItemId,
                vmSafetyItem.Name,
                message = "Your attached file uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamSafetyItemEdit", model);
        }

        [ActionName("submit")]
        [HttpPost]
        [RoleBaseAuthorize(SystemRoles.Leader, SystemRoles.Advisor, SystemRoles.CoAdvisor)]
        public ActionResult SubmitSafetyItem(int teamId)
        {
            var result = true;

            var blTeamSafetyItem = new BLTeamSafetyItem();

            string attachedFileUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    result = blTeamSafetyItem.UpdateSubmitTeamSafetyItemStatus(teamId, 1);

                    if (result)
                    {
                        #region

                        var blTeamMember = new BLTeamMember();
                        var teamMemberList = blTeamMember.GetTeamMembersByRoles(teamId,
                            new string[]
                            {
                                SystemRoles.Advisor.ToString(),
                                SystemRoles.CoAdvisor.ToString(),
                                SystemRoles.Leader.ToString(),
                            });

                        var title = "ESP# WERC - 2021 – " + teamMemberList.First().TeamName;

                        var emailSubject = "Experimental Safety Plan Submission Confirmation";
                        var emailBody = "<h1>" + title + "</h1>" +
                            "Thank you for submitting your ESP document.It is now in review and you will be contacted in a few days." +
                            "<hr/>" +
                            "If you have questions about the WERC Design Contest Experimental Safety Plan, please call 575 - 646 - 1292 or email miljgh@nmsu.edu.";

                        emailHelper = new EmailHelper
                        {
                            SpecialEmail = specialEmail,
                            Subject = emailSubject,
                            Body = emailBody,
                            IsBodyHtml = true,

                        };

                        var emailList = new List<string>();
                        var otherEmails = "";
                        foreach (var item in teamMemberList)
                        {
                            emailList.Add(item.Email);
                            otherEmails += item.Email + ", ";
                        }

                        emailHelper.EmailList = emailList.ToArray();
                        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                        {
                            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                        }
                        emailHelper.CurrentUserId = CurrentUserId;
                        emailHelper.Send();


                        emailHelper = new EmailHelper
                        {
                            SpecialEmail = specialEmail,
                            Subject = emailSubject,
                            Body = otherEmails + "<br/>" + emailBody,
                            IsBodyHtml = true,
                            EmailList = new string[] { specialEmail },

                        };
                        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                        {
                            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                        }
                        emailHelper.CurrentUserId = CurrentUserId;
                        emailHelper.Send();

                        var blPerson = new BLPerson();
                        var personList = blPerson.GetUsersByRoleNames(new string[]
                        {
                            SystemRoles.Admin.ToString(),
                            SystemRoles.SafetyAdmin.ToString(),

                        });

                        emailList.Clear();
                        otherEmails = "";
                        foreach (var item in personList)
                        {
                            emailList.Add(item.Email);
                            otherEmails += item.Email + ", ";
                        }

                        title = "ESP# WERC - 2021 – " + teamMemberList.First().TeamName + " has been submitted";
                        emailSubject = title;
                        emailBody = title;

                        emailHelper = new EmailHelper
                        {
                            SpecialEmail = specialEmail,
                            Subject = emailSubject,
                            Body = emailBody,
                            IsBodyHtml = true,
                        };

                        emailHelper.EmailList = emailList.ToArray();
                        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                        {
                            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                        }
                        emailHelper.CurrentUserId = CurrentUserId;
                        emailHelper.Send();

                        emailHelper = new EmailHelper
                        {
                            SpecialEmail = specialEmail,
                            Subject = emailSubject,
                            Body = otherEmails + "<br/>" + emailBody,
                            IsBodyHtml = true,
                            EmailList = new string[] { specialEmail },

                        };
                        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                        {
                            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                        }
                        emailHelper.CurrentUserId = CurrentUserId;
                        emailHelper.Send();
                        #endregion

                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                attachedFileUrl,
                success = result,
                message = ""
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        [ActionName("checkSaved")]
        [HttpPost]
        [RoleBaseAuthorize(SystemRoles.Leader, SystemRoles.Advisor, SystemRoles.CoAdvisor)]
        public ActionResult CheckSavedSafetyItem(int teamId)
        {
            var result = true;
            var blTeamSafetyItem = new BLTeamSafetyItem();

            try
            {
                if (ModelState.IsValid)
                {
                    result = blTeamSafetyItem.CheckSavedTeamSafety(teamId);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                saved = result,
                message = ""
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        #endregion ESP

        #region Grading Report

        [HttpGet]
        [ActionName("fgrm")]
        public ActionResult LoadFinalGradesReport(int id = -1)
        {
            var blTeamMember = new BLTeamMember();
            var teamMember = blTeamMember.GetTeamMemberByUserId(CurrentUserId);
            var layout = "";
            var teamId = id;

            if (id == -1)
            {
                teamId = teamMember.TeamId;
            }

            if (CurrentUserRoles.Contains("Judge") == false)
            {
                if (CurrentUserRoles.Contains("Advisor"))
                {
                    layout = "~/Views/Shared/_LayoutAdvisor.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.Student.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutStudent.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.Leader.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutLeader.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.CoAdvisor.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutCoAdvisor.cshtml";
                }

                //var blTeam = new BLTeam();
                //var serveyIsComplete = blTeam.GetTeamById(teamId).Survey;

                //int inCompleteSurveyCount = 0;
                //var allMember = blTeamMember.GetTeamMembers(teamId);

                //inCompleteSurveyCount = allMember.Where(m => (m.RoleName != "Advisor" && m.RoleName != "CoAdvisor") && m.Survey == false).Count();

                //if (inCompleteSurveyCount > 0)


                var blTeam = new BLTeam();
                var viewFinalScore = blTeam.GetTeamById(teamId).ViewFinalScore;

                if (viewFinalScore == false)
                {
                    return View("Error", new VMHandleErrorInfo
                    {
                        CurrentUserId = CurrentUserId,
                        ErrorMessage = "Your scores are not yet available.",
                        ViewLayout = layout
                    });
                }
            }

            int gradeId = int.Parse(WebConfigurationManager.AppSettings["GradeId"]);

            var blGrade = new BLGrade();
            var gradegReportList = blGrade.GetStudentGradeReportListWithoutGradeId(CurrentUserId, gradeId, teamId);

            var otherTeamsGradeReportList = blGrade.GetStudentOtherTeamGradeReportListWithoutId(CurrentUserId, teamId, gradeId);

            var currentTeamContainer = otherTeamsGradeReportList.First().TeamGradeList.Where(t => t.TeamId == teamId);
            if (currentTeamContainer.Count() > 0)
            {
                var currentTeam = otherTeamsGradeReportList.First().TeamGradeList.Where(t => t.TeamId == teamId).First();

                otherTeamsGradeReportList.First().TeamGradeList.Remove(currentTeam);
            }

            ViewBag.BS_Title = "Final Score";

            return View("FinalGradeReportManagement", new VmFinalGradeReportManagement
            {
                CurrentUserId = CurrentUserId,
                GradeReportList = gradegReportList,
                OtherTeamsGradeReportList = otherTeamsGradeReportList,
                ViewLayout = layout,
                ExceldataList = blGrade.GetExcelData(teamId)
            });
        }

        [HttpGet]
        [ActionName("bsfgrm")]
        public ActionResult BenchScaleLoadFinalGradesReport(int id = -1)
        {
            var blTeamMember = new BLTeamMember();
            var teamMember = blTeamMember.GetTeamMemberByUserId(CurrentUserId);
            var layout = "";
            var teamId = id;

            if (id == -1)
            {
                teamId = teamMember.TeamId;
            }

            if (CurrentUserRoles.Contains("Judge") == false)
            {
                if (CurrentUserRoles.Contains("Advisor"))
                {
                    layout = "~/Views/Shared/_LayoutAdvisor.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.Student.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutStudent.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.Leader.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutLeader.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.CoAdvisor.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutCoAdvisor.cshtml";
                }

                //var blTeam = new BLTeam();
                //var serveyIsComplete = blTeam.GetTeamById(teamId).Survey;

                //int inCompleteSurveyCount = 0;
                //var allMember = blTeamMember.GetTeamMembers(teamId);

                //inCompleteSurveyCount = allMember.Where(m => (m.RoleName != "Advisor" && m.RoleName != "CoAdvisor") && m.Survey == false).Count();

                //if (inCompleteSurveyCount > 0)


                var blTeam = new BLTeam();
                var viewFinalScore = blTeam.GetTeamById(teamId).ViewFinalScore;

                if (viewFinalScore == false)
                {
                    return View("Error", new VMHandleErrorInfo
                    {
                        CurrentUserId = CurrentUserId,
                        ErrorMessage = "Your scores are not yet available.",
                        ViewLayout = layout
                    });
                }
            }

            int gradeId = int.Parse(WebConfigurationManager.AppSettings["GradeId"]);

            var blGrade = new BLGrade();
            var gradegReportList = blGrade.BenchScaleGetStudentGradeReportList(CurrentUserId, gradeId, teamId);

            var otherTeamsGradeReportList = blGrade.BenchScaleGetStudentOtherTeamGradeReportList(CurrentUserId, teamId, gradeId);

            var currentTeamContainer = otherTeamsGradeReportList.First().TeamGradeList.Where(t => t.TeamId == teamId);
            if (currentTeamContainer.Count() > 0)
            {
                var currentTeam = otherTeamsGradeReportList.First().TeamGradeList.Where(t => t.TeamId == teamId).First();

                otherTeamsGradeReportList.First().TeamGradeList.Remove(currentTeam);
            }

            ViewBag.BS_Title = "Bench Scale Quality Final Score";

            return View("FinalGradeReportManagement", new VmFinalGradeReportManagement
            {
                CurrentUserId = CurrentUserId,
                GradeReportList = gradegReportList,
                OtherTeamsGradeReportList = otherTeamsGradeReportList,
                ViewLayout = layout,
                ExceldataList = blGrade.GetExcelData(teamId)
            });
        }

        #endregion Grading Report


    }
}
