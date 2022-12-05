using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Model.ViewModels.Admin;
using Model.ViewModels.MealType;
using Model.ViewModels.Person;
using Model.ViewModels.Reference;
using Model.ViewModels.SafetyItem;
using Model.ViewModels.Team;
using Model.ViewModels.TeamSafetyItem;
using Model.ViewModels.TeamSafetyItemLog;

using Stimulsoft.Report;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers
{
    [RoleBaseAuthorize(SystemRoles.SafetyAdmin, SystemRoles.Admin)]
    public class SafetyAdminController : BaseController
    {

        static SafetyAdminController()
        {
            Stimulsoft.Base.StiLicense.Key =
                "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2AD0gPVknKsaW0un+3PuM6TTcPMUAWEURKXNso0e5OJN40hxJjK5JbrxU+NrJ3E0OUAve6MDSIxK3504G4vSTqZezogz9ehm+xS8zUyh3tFhCWSvIoPFEEuqZTyO744uk+ezyGDj7C5jJQQjndNuSYeM+UdsAZVREEuyNFHLm7gD9OuR2dWjf8ldIO6Goh3h52+uMZxbUNal/0uomgpx5NklQZwVfjTBOg0xKBLJqZTDKbdtUrnFeTZLQXPhrQA5D+hCvqsj+DE0n6uAvCB2kNOvqlDealr9mE3y978bJuoq1l4UNE3EzDk+UqlPo8KwL1XM+o1oxqZAZWsRmNv4Rr2EXqg/RNUQId47/4JO0ymIF5V4UMeQcPXs9DicCBJO2qz1Y+MIpmMDbSETtJWksDF5ns6+B0R7BsNPX+rw8nvVtKI1OTJ2GmcYBeRkIyCB7f8VefTSOkq5ZeZkI8loPcLsR4fC4TXjJu2loGgy4avJVXk32bt4FFp9ikWocI9OQ7CakMKyAF6Zx7dJF1nZw";
        }
        BLPerson blPerson = new BLPerson();
        VmPerson person = null;

        // GET: Admin
        public ActionResult Index()
        {
            var UserRoles = TempData["UserRoles"] as IEnumerable<string>;

            return View(new VmAdmin() { CurrentUserRoles = UserRoles });
        }

        #region admin menus

        [HttpGet]
        [ActionName("tfim")]
        public ActionResult TeamFullInfoManagement()
        {
            return View("TeamFullInfoManagement", new VmTeamFullInfoManagement());
        }

        [HttpGet]
        [ActionName("tem")]
        public ActionResult TeamEmailManagement()
        {
            var blTeamMember = new BLTeamMember();

            var memberUserIds = blTeamMember.GetAllTeamMembersUserIds();

            return View("TeamEmailManagement", new VmTeamFullInfoManagement()
            {
                MemberUserIds = memberUserIds
            });
        }

        [HttpGet]
        [ActionName("ruem")]
        public ActionResult RoleBaseUserEmailManagement()
        {
            return View("RoleBaseUserEmailManagement", new VmRoleBaseUserEmailManagement());
        }

        #endregion

        [HttpPost]
        [ActionName("scl")]
        public PartialViewResult GetSafetyLog(int safetyItemId, int teamId, bool type)
        {
            var blTeamSafetyItemLog = new BLTeamSafetyItemLog();

            var teamSafetyItemLogList = blTeamSafetyItemLog.GetTeamSafetyItemLog(safetyItemId, teamId, type);

            return PartialView("_TeamSafetyItemLog", new VmTeamSafetyItemLogCollection
            {
                SafetyItemLogList = teamSafetyItemLogList
            });
        }

        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonByUserId(CurrentUserId);
             
            vmPerson.OnActionSuccess = "loadSafetyAdminPanel";
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

        #region reference
        [HttpPost]
        [ActionName("urf")]
        public ActionResult UploadReferenceFile(System.Web.HttpPostedFileBase uploadedReferenceFile, string title)
        {
            var result = true;
            var blReference = new BLReference();
            var referenceFileUrl = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedReferenceFile != null)
                    {
                        referenceFileUrl = UIHelper.UploadFile(uploadedReferenceFile, "/Resources/Uploaded/TeamSafetyItems/ReferenceFile/" + CurrentUserId.Replace("-", ""));
                    }

                    blReference.CreateReference(
                            new VmReference
                            {
                                ReferenceFileUrl = referenceFileUrl,
                                Title = title,

                            });
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {

                success = result,
                message = "Your reference file uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamSafetyItemEdit", model);
        }

        [HttpGet]
        [ActionName("larf")]
        public ActionResult LoadAddReferenceForm()
        {
            var blReference = new BLReference();

            return View("ReferenceUpload", blReference.GetAllReference());
        }

        [HttpPost]
        [ActionName("drf")]
        public ActionResult DeleteReferenceFile(int id)
        {
            var blReference = new BLReference();

            var reference = blReference.GetReferenceById(id);

            //if (reference != null)
            //{
            //    UIHelper.DeleteFile(reference.ReferenceFileUrl);
            //}

            blReference.DeleteReference(id);

            return View("ReferenceUpload", blReference.GetAllReference());
        }
        #endregion reference


        [HttpGet]
        [ActionName("sa_tfim")]
        public ActionResult SafetyAdminTeamFullInfoManagement()
        {

            return View("SafetyAdminTeamFullInfoManagement", new VmTeamFullInfoManagement());
        }

        [HttpGet]
        [ActionName("sa_tfim_esp_r")]
        public ActionResult SafetyAdminTeamFullInfoESPReportManagement()
        {

            return View("SafetyAdminTeamFullInfoESPReportManagement", new VmTeamFullInfoManagement());
        }

        [HttpGet]
        [ActionName("gesp")]
        public ActionResult GetESP(int teamId)
        {
            var blSafetyItem = new BLSafetyItem();

            var blTeam = new BLTeam();

            var teamFullInfoList = blTeam.GetTeamFullInfoByFilter(new VmTeamFullInfo()).ToList();

            var teamName = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.Name).First();
            var taskName = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.TaskName).First();
            var facultyAdvisor = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.Advisor).First();
            var university = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.University).First();

            var blReference = new BLReference();

            var blTeamSafetyItem = new BLTeamSafetyItem();

            var vmTeamSafetyItemList = blTeamSafetyItem.GetTeamSafetyItemByTeamId(teamId);
            var safetyItemList = blSafetyItem.GetAllSafetyItems(teamId, safetyAdmin: true);
            //   var vmTeamSafetyItemList = blTeamSafetyItem.GetSafetyAdminTeamSafetyItemByTeamId(teamId);

            return View("SafetyAdminExperimentalSafetyPlan",
                new VmTeamSafetyItemCollection
                {
                    SafetyItemList = safetyItemList,
                    TeamSafetyItemList = vmTeamSafetyItemList,
                    ReferenceFiles = blReference.GetAllReference(),
                    TeamName = teamName,
                    TaskName = taskName,
                    Advisor = facultyAdvisor,
                    University = university,
                    TeamId = teamId
                });
        }

        [HttpGet]
        [ActionName("gesprp")]
        public ActionResult GetESPReportPage(int teamId)
        {

            var blTeam = new BLTeam();

            var teamFullInfoList = blTeam.GetTeamFullInfoByFilter(new VmTeamFullInfo()).ToList();

            var teamName = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.Name).First();
            var taskName = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.TaskName).First();
            var facultyAdvisor = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.Advisor).First();
            var university = teamFullInfoList.Where(t => t.Id == teamId).Select(t => t.University).First();

            var blTeamMember = new BLTeamMember();
            var teamMemberList = blTeamMember.GetTeamMembers(teamId);

            return View("SafetyAdminESPReportPage",
                new VmTeamSafetyItemCollection
                {
                    TeamMemberList = teamMemberList,
                    TeamName = teamName,
                    TaskName = taskName,
                    Advisor = facultyAdvisor,
                    University = university,

                });
        }

        [ActionName("gclddl")]
        public ActionResult GetTaskDropDownList(int teamId, int safetyItemId)
        {
            var blSafetyItem = new BLSafetyItem();

            var safetyItemDetailList = blSafetyItem.GetSafetyItemDetailSelectListItem(teamId, safetyItemId);

            return Json(safetyItemDetailList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("ssi")]
        [HttpPost]
        public ActionResult SaveSaftyItem(VmSaveSafetyItemAdmin saveSafetyItemAdmin)
        {
            var vmSafetyItem = new VmSafetyItem();
            var result = true;
            var blTeamSafetyItem = new BLTeamSafetyItem();

            try
            {
                if (ModelState.IsValid)
                {

                    result = blTeamSafetyItem.UpdateTeamSafetyItemStatusAndComment(new VmTeamSafetyItem
                    {
                        TeamId = saveSafetyItemAdmin.TeamId,
                        SafetyItemId = saveSafetyItemAdmin.SafetyItemId,
                        SubSafetyItemId = saveSafetyItemAdmin.SubSafetyItemId,
                        LastComment = saveSafetyItemAdmin.Comment ?? "",
                        LastContent = saveSafetyItemAdmin.LastContent ?? "",
                        ItemStatus = saveSafetyItemAdmin.ItemStatus,
                        Type = true,
                        UserId = CurrentUserId,
                        AttachedFileUrl = saveSafetyItemAdmin.AttachedFileUrl,

                    });
                    var blSafetyItem = new BLSafetyItem();
                    vmSafetyItem = blSafetyItem.GeTeamParentSafetyItemStatus(saveSafetyItemAdmin.TeamId, saveSafetyItemAdmin.SafetyItemId, true);

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                success = result,
                vmSafetyItem.SafetyItemStatus,
                safetyItemId = saveSafetyItemAdmin.SafetyItemId,
                vmSafetyItem.Name,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("espa")]
        [HttpPost]
        public ActionResult ESPApproveSaftyItem(int teamId, int itemStatus)
        {
            var result = true;
            var blTeamSafetyItem = new BLTeamSafetyItem();
            var allowInReview = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var blTeamMember = new BLTeamMember();
                    if (result)
                    {

                        #region

                        if (itemStatus == 2) // In review
                        {
                            var inReviewTeamSafetyItemList = blTeamSafetyItem.GetApproveAllTeamSafetyIteam(teamId);

                            foreach (var item in inReviewTeamSafetyItemList)
                            {
                                if (item.ItemStatus == 2)
                                {
                                    allowInReview = true;
                                }
                            }
                        }

                        if (itemStatus == 2 && allowInReview == false)
                        {
                            var jsonDataInreview = new
                            {
                                success = result,
                                allowInReview,
                            };

                            return Json(jsonDataInreview, JsonRequestBehavior.AllowGet);
                        }

                        var blTeam = new BLTeam();
                        if (itemStatus == 3) // approve
                        {
                            var approvalTeamSafetyItemList = blTeamSafetyItem.GetApproveAllTeamSafetyIteam(teamId);

                            foreach (var saveSafetyItemAdmin in approvalTeamSafetyItemList)
                            {
                                blTeamSafetyItem.UpdateTeamSafetyItemStatusAndComment(new VmTeamSafetyItem
                                {
                                    TeamId = saveSafetyItemAdmin.TeamId,
                                    SafetyItemId = saveSafetyItemAdmin.SafetyItemId,
                                    SubSafetyItemId = saveSafetyItemAdmin.SubSafetyItemId,
                                    LastComment = saveSafetyItemAdmin.LastComment ?? "",
                                    LastContent = saveSafetyItemAdmin.LastContent ?? "",
                                    ItemStatus = 3,
                                    Type = true,
                                    UserId = CurrentUserId,
                                    AttachedFileUrl = saveSafetyItemAdmin.AttachedFileUrl,

                                });
                            }
                        }

                        var teamMemberList = blTeamMember.GetTeamMembersByRoles(teamId,
                            new string[]
                            {
                                    SystemRoles.Advisor.ToString(),
                                    SystemRoles.CoAdvisor.ToString(),
                                    SystemRoles.Leader.ToString(),
                                    SystemRoles.Student.ToString(),
                            });

                        var title = "ESP# WERC - 2023 – " + teamMemberList.First().TeamName;

                        var emailSubject = "An in review Comment For Your WERC 2023 ESP";

                        var emailBody = "<h1>" + title + "</h1>" +
                            "You ESP has been reviewed in the WERC Design Contest System. It is included questions, comments and/or requests for changes regarding the safety of your experiment." +
                            "<br/>" +
                            "Please login to the WERC Design Contest System and respond to these in an understanding and timely manner." +
                            "<hr/>" +
                            "If you have questions about the WERC Design Contest Experimental Safety Plan, please call 575 - 646 - 1292 or email miljgh@nmsu.edu.";

                        if (itemStatus == 3)
                        {
                            emailSubject = "WERC Design Contest 2023 ESP Approval";
                            emailBody = "<h1>" + title + "</h1>" +
                               "Your ESP document has been approved. The final phase of approval will happen at the event when myself, or one of my safety team, " +
                               "will compare this document with your bench scale setup, including any chemicals and materials have on hand." +
                               "If you foresee any changes before you arrive at the event, please request a revision to your ESP document so it can be re-approved. " +
                               "After your bench scale setup is approved, you will be issued a run permit and be allowed to collect any starting water/samples needed for your work. " +
                               "Please remember that the bench scale area at this event is considered to be a laboratory area.  And, as such, everyone will be required to wear safety glasses, " +
                               "long pants or leg coverings, and close toe shoes at all times.  Thank you for your understanding with this process." +
                               "<hr/>" +
                               "If you have questions about the WERC Design Contest Experimental Safety Plan, please call 575 - 646 - 1292 or email miljgh@nmsu.edu.";

                        }

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
                            Body = otherEmails + "<br>" + emailBody,
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
                success = result,
                allowInReview,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("espr")]
        [HttpGet]
        public ActionResult ESPReport(int id)
        {

            string fileName = "ESPReport_" + id + ".pdf";
            string filePath = Server.MapPath("~/Resources/Uploaded/Reports/" + fileName);

            #region

            var blTeam = new BLTeam();
            var teamFullInfoList = blTeam.GetTeamFullInfoByFilter(new VmTeamFullInfo()).ToList();

            var teamName = teamFullInfoList.Where(t => t.Id == id).Select(t => t.Name).First();
            var taskName = teamFullInfoList.Where(t => t.Id == id).Select(t => t.TaskName).First();
            var facultyAdvisor = teamFullInfoList.Where(t => t.Id == id).Select(t => t.Advisor).First();
            var university = teamFullInfoList.Where(t => t.Id == id).Select(t => t.University).First();

            var blTeamMember = new BLTeamMember();

            var teamMemberList = blTeamMember.GetTeamMembers(id);
            var adivsorData = teamMemberList.Where(a => a.RoleId == "58c326dd-38ea-4d3c-92f9-3935e3763e68").First();

            List<VmTeamMember> reportDataList = new List<VmTeamMember>
            {
                new VmTeamMember
                {
                    RoleName = "Faculty Advisor",
                    MemberName = adivsorData.FirstName + " " + adivsorData.LastName,

                }
            };

            foreach (var item in teamMemberList)
            {
                if (item.RoleId != "58c326dd-38ea-4d3c-92f9-3935e3763e68")
                {

                    reportDataList.Add(new VmTeamMember
                    {
                        RoleName = "Researcher",
                        MemberName = item.FirstName + " " + item.LastName,

                    });
                }
            }

            reportDataList.Add(new VmTeamMember
            {
                RoleName = "EH & S(at request of COE Safety)",
                MemberName = "",

            });

            var blSafetyItem = new BLSafetyItem();

            var vmSafetyItemList = blSafetyItem.GetAllSafetyItems();

            var attachmentItems = string.Empty;

            int i = 0;

            foreach (var item in vmSafetyItemList)
            {
                attachmentItems += "Attachment " + ++i + ": " + item.Name + "\n";
            }

            #endregion


            var report = new StiReport();
            var path = Server.MapPath("~/Resources/Reports/ESPStimulReport.mrt");
            report.Load(path);

            var ds = new DataSet();
            var dt = ExcelHandler.ToDataTable(reportDataList);
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "ViewTeamMember";
            report.Dictionary.Databases.Clear();
            report.Dictionary.DataSources.Clear();

            report.RegData("ViewTeamMember", ds);
            report.Dictionary.Synchronize();
            report.Compile();

            report["ReportVariableTeamName"] = teamName + " ---- ";
            report["ReportVariableTaskName"] = taskName;
            report["ReportVariableUniversityName"] = university;
            report["ReportVariableAttachments"] = attachmentItems;

            report.Render(false);

            report.ExportDocument(StiExportFormat.Pdf, filePath);

            // return StiMvcReportResponse.ResponseAsPdf(report);

            var espTeamItemReportPDFFilePath = Server.MapPath(ESPReportTeamSafetyItemPDFUrl(id));

            var pdfFiles = new List<string>
            {
                filePath,
                espTeamItemReportPDFFilePath
            };

            #region Merg PDFs

            var blTeamSafetyItem = new BLTeamSafetyItem();
            var vmTeamSafetyItemList = blTeamSafetyItem.GetEspReportTeamSafetyItemByTeamId(id);

            foreach (var item in vmTeamSafetyItemList)
            {
                if (!string.IsNullOrWhiteSpace(item.AttachedFileUrl))
                {
                    var physicalPath = Server.MapPath(item.AttachedFileUrl.Split(new char[] { '?' })[0]);

                    if (System.IO.File.Exists(physicalPath) && pdfFiles.Contains(physicalPath) == false)
                    {
                        pdfFiles.Add(physicalPath);

                    }

                }
            }

            UIHelper.MergePDF(pdfFiles.ToArray(), Server.MapPath("~/Resources/EspReportMergedPDFs.pdf"));

            #endregion Merg PDFs

            var jsonData = new
            {
                downloadURL = $"{Request.Url.Scheme}{System.Uri.SchemeDelimiter}{Request.Url.Authority}/Resources/EspReportMergedPDFs.pdf",
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        public string ESPReportTeamSafetyItemPDFUrl(int id)
        {

            string fileName = "ESPTeamSafetyItemReport_" + id + ".pdf";
            string filePath = Server.MapPath("~/Resources/Uploaded/Reports/" + fileName);



            var report = new StiReport();
            var path = Server.MapPath("~/Resources/Reports/ESPTeamSafetyItemReport.mrt");
            report.Load(path);


            var blTeamSafetyItem = new BLTeamSafetyItem();
            var vmTeamSafetyItemList = blTeamSafetyItem.GetEspReportTeamSafetyItemByTeamId(id);

            var wercESPObject = (from t in vmTeamSafetyItemList
                                 select new VmWERCESPObject
                                 {
                                     BaseSafetyItemName = t.BaseSafetyItemName,
                                     BaseInstruction = t.BaseInstruction,
                                     BasePriority = t.BasePriority,
                                     LastContent = t.LastContent,
                                     LastComment = t.LastComment,
                                     SubSafetyItemName = t.SubSafetyItemName,
                                     Instruction = t.Instruction,
                                     Priority = t.Priority,
                                 }).ToList();


            var vmSafetyItemList = new BLSafetyItem().GetAllSafetyItems();

            foreach (var safetyItem in vmSafetyItemList)
            {
                var teamSafetyItemDetails = blTeamSafetyItem.GetTeamSafetyItemDetails(id, safetyItem.Id);

                var detailData = string.Empty;

                if (teamSafetyItemDetails != null && teamSafetyItemDetails.Count() > 0)
                {
                    foreach (var itemDetail in teamSafetyItemDetails)
                    {
                        detailData += itemDetail.Name + ": " + ((itemDetail.Value == null) ? false : true) + "<br/>";
                    }
                    wercESPObject.Where(e => e.BaseSafetyItemName == safetyItem.Name).First().TeamSafetyItemDetails = detailData;
                }

            }

            report.Dictionary.Databases.Clear();
            report.Dictionary.DataSources.Clear();

            report.RegData("MasterDetailDataSource", wercESPObject);
            report.Dictionary.Synchronize();
            report.Compile();

            report.Render(false);

            report.ExportDocument(StiExportFormat.Pdf, filePath);


            return $"~/Resources/Uploaded/Reports/{fileName}";
            //return $"{Request.Url.Scheme}{System.Uri.SchemeDelimiter}{Request.Url.Authority}/Resources/Uploaded/Reports/{fileName}";


        }

    }
}