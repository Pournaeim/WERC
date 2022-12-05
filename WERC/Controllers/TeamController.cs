using BLL;

using ClosedXML.Excel;

using CyberneticCode.Web.Mvc.Helpers;

using Model.Base;
using Model.ViewModels.Invoice;
using Model.ViewModels.Team;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers
{
    [RoleBaseAuthorize(SystemRoles.Admin, SystemRoles.Advisor, SystemRoles.CoAdvisor, SystemRoles.Leader, SystemRoles.Student, SystemRoles.SafetyAdmin)]
    public class TeamController : BaseController
    {
        // GET: Team
        public void RefreshTeam()
        {
            //var blInvoice = new BLInvoice();

            //var invoiceIds = blInvoice.GetInvoiceIds(false);

            //if (invoiceIds != null)
            //{
            //    foreach (var item in invoiceIds.Ids)
            //    {


            //        int? lastOrderId = 0;

            //        var blShopCart = new BLShopCart();

            //        var lastOrderInfo = blShopCart.GetCheckoutStatus(CurrentUserId, item, out lastOrderId);

            //        if (lastOrderInfo != null)
            //        {
            //            blInvoice.UpdateInvoiceOrderStatus(lastOrderInfo, item, true, lastOrderId.Value, true, true);

            //            invoiceIds = blInvoice.GetInvoiceFullInfoByUserId(CurrentUserId, false);
            //        }
            //    }
            //}

            //var invoiceList = blInvoice.GetInvoiceFullInfoByUserId(CurrentUserId, false);

            //RedirectToAction("tfim", "admin");
        }
        public ActionResult TeamList()
        {
            var bsTeam = new BLTeam();

            return View(new VmTeamCollection() { TeamList = bsTeam.GetAdvisorTeams(CurrentUserId) });
        }

        [ActionName("etps")]
        [HttpPost]
        public ActionResult EditPayStatus(List<VmTeamSelection> teamSelectionList)
        {
            var result = true;
            var blTeam = new BLTeam();
            string checkoutURL = "";
            try
            {

                var blShopCart = new BLShopCart();
                checkoutURL = blShopCart.HandelCheckout(teamSelectionList, CurrentUserId);
            }
            catch (Exception ex)
            {
                result = false;
            }
            var jsonData = new
            {
                success = result,
                message = "Operation has been successful",
                redirectCheckoutURL = checkoutURL
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }


        [ActionName("etpsem")]
        [HttpPost]
        public ActionResult EditPayStatusExtraMember(int invoiceId)
        {
            var result = true;
            var blTeam = new BLTeam();
            string checkoutURL = "";
            try
            {

                var blShopCart = new BLShopCart();
                checkoutURL = blShopCart.HandelCheckoutExtraMember(invoiceId, CurrentUserId);

            }
            catch (Exception ex)
            {
                result = false;
            }
            var jsonData = new
            {
                success = result,
                message = "Operation has been successful",
                redirectCheckoutURL = checkoutURL

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("gtfibf")]
        [HttpPost]
        public JsonResult GetTeamFullInfoByFilter(VmTeamFullInfo filter = null)
        {
            var blTeam = new BLTeam();

            var teamFullInfoList = blTeam.GetTeamFullInfoByFilter(filter).ToList();

            teamFullInfoList.First().LabResultUrl = "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816004448favicon.png?CT=application_vnd.oasis.opendocument.spreadsheet.png," +
                "/Resources/Uploaded/Teams/6a1436ffcab9434aad8fed02f32f2411/20180810151905favicon.png?CT=application_pdf.png," +
                "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816005047favicon.png?CT=application_pkcs7_mime.png";

            #region Generate Excel

            var ds = new DataSet();

            var excelData = from e in teamFullInfoList
                            select new
                            {
                                e.Name,
                                e.ProjectName,
                                Payment = "$" + e.Amount + "-(" + e.Payment + "%)",
                                e.TaskName,
                                e.Advisor,
                                e.ExcelFileUrl,
                            };


            var dt = ExcelHandler.ToDataTable(excelData.ToList());

            var newTable = ExcelHandler.CreateExcelBaseDataTable(dt,

                "Name",
                "ProjectName",
                "Payment",
                "TaskName",
                "Advisor");

            ds.Tables.Add(newTable.Copy());

            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;
            var path = "/Resources/Uploaded/Excel/";

            var date = DateTime.Now;
            newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = newFilePrefix + "ExcelReport.xlsx";

            serverPath = Path.Combine(HttpContext.Server.MapPath(path), Path.GetFileName(fileName));

            fileUrl = Path.Combine(path, fileName.Replace("\\", "/"));

            var xlWorkbook = ExcelHandler.ExportDataSetToExcel(ds, serverPath);

            xlWorkbook.Worksheet(1).Columns("e").Width = 30;
            xlWorkbook.Worksheet(1).RowHeight = 27;

            foreach (IXLRow row in xlWorkbook.Worksheet(1).Rows())
            {

                foreach (IXLCell cell in row.Cells())
                {
                    row.Style.Alignment.SetShrinkToFit(true);
                    row.Style.Alignment.ShrinkToFit = true;
                }

            }

            xlWorkbook.SaveAs(serverPath);

            BLSubmissionRule blSubmissionRule = new BLSubmissionRule();

            var dueDate = blSubmissionRule.GetSubmissionRuleById(3).DueDate;

            var teamSubmissionRuleList = blSubmissionRule.GetTeamSubmissionRules();

            foreach (var item in teamFullInfoList)
            {
                item.ExcelFileUrl = fileUrl;

                DateTime? uploadDate = null;

                var teamSubmissionRule = teamSubmissionRuleList.Where(t => t.TeamId == item.Id && t.SubmissionRuleId == 3);

                if (teamSubmissionRule != null && teamSubmissionRule.Count() > 0)
                {
                    uploadDate = teamSubmissionRule.First().UploadDate;
                }
                if (uploadDate != null)
                {
                    item.WrittenReportLate = uploadDate > DateTime.Parse(dueDate);
                }
            }

            #endregion Generate Excel

            return Json(teamFullInfoList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("nagtfibf")]
        [HttpPost]
        public JsonResult NoneAdminGetTeamFullInfoByFilter(VmTeamFullInfo filter = null)
        {
            var blTeam = new BLTeam();

            var teamFullInfoList = blTeam.GetNoneAdminTeamFullInfoByFilter(CurrentUserId, filter, CurrentUserRoles.First()).ToList();

            teamFullInfoList.First().LabResultUrl = "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816004448favicon.png?CT=application_vnd.oasis.opendocument.spreadsheet.png," +
                "/Resources/Uploaded/Teams/6a1436ffcab9434aad8fed02f32f2411/20180810151905favicon.png?CT=application_pdf.png," +
                "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816005047favicon.png?CT=application_pkcs7_mime.png";

            return Json(teamFullInfoList, JsonRequestBehavior.AllowGet);
        }
        [ActionName("gta")]
        [HttpGet]
        public JsonResult GetTeamActivation(VmTeam filter = null)
        {

            var blTeam = new BLTeam();

            var teamFullInfoList = blTeam.GetTeamList();

            teamFullInfoList.First().LabResultUrl = "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816004448favicon.png?CT=application_vnd.oasis.opendocument.spreadsheet.png," +
                "/Resources/Uploaded/Teams/6a1436ffcab9434aad8fed02f32f2411/20180810151905favicon.png?CT=application_pdf.png," +
                "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816005047favicon.png?CT=application_pkcs7_mime.png";
            return Json(teamFullInfoList, JsonRequestBehavior.AllowGet);
        }


        [ActionName("gtebf")]
        [HttpPost]
        public JsonResult GetTeamEmailByFilter(VmTeamFullInfo filter = null)
        {

            var blTeam = new BLTeam();

            var teamFullInfoList = blTeam.GetTeamFullInfoByFilter(filter).ToList();

            teamFullInfoList.First().LabResultUrl = "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816004448favicon.png?CT=application_vnd.oasis.opendocument.spreadsheet.png," +
                "/Resources/Uploaded/Teams/6a1436ffcab9434aad8fed02f32f2411/20180810151905favicon.png?CT=application_pdf.png," +
                "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816005047favicon.png?CT=application_pkcs7_mime.png";
            return Json(teamFullInfoList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("gtfibfad")]
        [HttpGet]
        public JsonResult GetTeamFullInfoByFilterByAdvisor(VmTeamFullInfo filterItem = null)
        {
            var blTeam = new BLTeam();

            var teamFullInfoList = blTeam.GetTeamFullInfoByFilterByAdvisor(CurrentUserId, filterItem).ToList();
            teamFullInfoList.First().LabResultUrl = "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816004448favicon.png?CT=application_vnd.oasis.opendocument.spreadsheet.png," +
                "/Resources/Uploaded/Teams/6a1436ffcab9434aad8fed02f32f2411/20180810151905favicon.png?CT=application_pdf.png," +
                "/Resources/Uploaded/Teams/97fda227f835461d8b39a4f3bf1fa9fc/20180816005047favicon.png?CT=application_pkcs7_mime.png";
            return Json(teamFullInfoList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("lctf")]
        [HttpGet]
        [RoleBaseAuthorize(SystemRoles.Advisor)]
        public ActionResult LoadCreateTeamForm()
        {
            var blPerson = new BLPerson();
            var person = blPerson.GetPersonByUserId(CurrentUserId);

            var bsTeam = new BLTeam();
            var teamCount = bsTeam.GetAdvisorTeams(CurrentUserId).Count();



            return View("../Advisor/CreateTeam", new VmTeam
            {
                University = person.University,
                TeamCount = teamCount,
            });
        }

        [ActionName("ct")]
        [HttpPost]
        public ActionResult Create(VmTeam model)
        {
            var result = -1;
            var blTeam = new BLTeam();

            model.CurrentUserId = CurrentUserId;

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.UploadedDocument != null)
                    {
                        model.TeamImageUrl = UIHelper.UploadFile(model.UploadedDocument, "/Resources/Uploaded/Teams/" + CurrentUserId.Replace("-", ""));
                    }
                }

                result = blTeam.CreateTeam(model);
            }

            catch (Exception ex)
            {
                result = -1;
            }


            if (result != -1)
            {
                return RedirectToAction("tl", "Advisor", new { activeItemId = result });
            }

            model.ActionMessageHandler.Message = "Operation has been failed...\n";

            return View("../Advisor/CreateTeam", model);
        }

        [ActionName("et")]
        [HttpPost]
        public ActionResult EditTeam(VmTeam model)
        {
            model.CurrentUserId = CurrentUserId;

            var oldUrl = model.TeamImageUrl;
            var result = true;
            var blTeam = new BLTeam();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.UploadedDocument != null)
                    {
                        model.TeamImageUrl = UIHelper.UploadFile(model.UploadedDocument, "/Resources/Uploaded/Teams/" + CurrentUserId.Replace("-", ""));
                    }
                }
                result = blTeam.UpdateTeam(model);
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(model.TeamImageUrl))
            //{
            //    UIHelper.DeleteFile(oldUrl);
            //}

            if (result == false)
            {
                model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
            }

            var jsonData = new
            {
                teamTitle = model.Name,
                teamIconUrl = model.TeamImageUrl,
                teamId = model.Id,
                success = result,
                message = model.ActionMessageHandler.Message = "Operation has been successful"

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UploadPreliminaryReport(int teamId, string oldPreliminaryReportUrl, HttpPostedFileBase UploadedPreliminaryReport)
        {
            var result = true;
            var blTeam = new BLTeam();
            string preliminaryReportUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    preliminaryReportUrl = UIHelper.UploadFile(UploadedPreliminaryReport, "/Resources/Uploaded/Teams/PreliminaryReport/" + CurrentUserId.Replace("-", ""));

                    result = blTeam.UploadPreliminaryReport(teamId, preliminaryReportUrl);
                    if (string.IsNullOrWhiteSpace(preliminaryReportUrl) == true && result == true)
                    {
                        var blSubmissionRule = new BLSubmissionRule();
                        var submissionRuleList = blSubmissionRule.GetAllSubmissionRule();
                        var preliminaryReportDays = DateTime.Parse(submissionRuleList[2].DueDate).Subtract(DateTime.Now).TotalDays;
                        var days = preliminaryReportDays - Math.Truncate(preliminaryReportDays);
                        if (days > 0)
                        {
                            preliminaryReportDays++;
                        }

                        if (preliminaryReportDays > 0)
                        {
                            var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                            var adminPerson = new BLPerson().GetPersonByUserId(adminUserId);
                            var advisorPerson = new BLPerson().GetPersonByUserId(CurrentUserId);

                            var subject = "Confirm your WERC Environmental Design Contest 2023 account.";

                            var bodyForAdmin = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + adminPerson.FirstName + " " + adminPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";

                            var bodyForAdvisor = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + advisorPerson.FirstName + " " + advisorPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";


                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdmin,
                                IsBodyHtml = true,
                                EmailList = new string[] { specialEmail }
                            };

                            for (var i = 0; i < emailHelper.EmailList.Length; i++)
                            {
                                emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                            }

                            emailHelper.CurrentUserId = CurrentUserId;
                            emailHelper.Send();

                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdvisor,
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
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(preliminaryReportUrl))
            //{
            //    UIHelper.DeleteFile(oldPreliminaryReportUrl);
            //}

            var jsonData = new
            {
                preliminaryReportUrl = HttpUtility.HtmlDecode(preliminaryReportUrl),
                preliminaryReportFileName = UploadedPreliminaryReport.FileName,
                preliminaryReportUrlIcon = "/Resources/Images/Mimetypes128x128/" + preliminaryReportUrl.Split(new string[] { "?CT=" }, StringSplitOptions.RemoveEmptyEntries)[1],
                success = result,
                message = "Your preliminary report successfully has been uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }


        [ActionName("uftr")]
        [HttpPost]
        public ActionResult UploadFlashTalkReport(int teamId, string oldFlashTalkReportUrl, HttpPostedFileBase UploadedFlashTalkReport)
        {
            var result = true;
            var blTeam = new BLTeam();
            string falshTalkReportUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    falshTalkReportUrl = UIHelper.UploadFile(UploadedFlashTalkReport, "/Resources/Uploaded/Teams/FlashTalkReport/" + CurrentUserId.Replace("-", ""));

                    result = blTeam.UploadFlashTalkReport(teamId, falshTalkReportUrl);
                    if (string.IsNullOrWhiteSpace(falshTalkReportUrl) == true && result == true)
                    {
                        var blSubmissionRule = new BLSubmissionRule();
                        var submissionRuleList = blSubmissionRule.GetAllSubmissionRule();
                        var awardNominationDays = DateTime.Parse(submissionRuleList[2].DueDate).Subtract(DateTime.Now).TotalDays;
                        var days = awardNominationDays - Math.Truncate(awardNominationDays);
                        if (days > 0)
                        {
                            awardNominationDays++;
                        }

                        if (awardNominationDays > 0)
                        {
                            var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                            var adminPerson = new BLPerson().GetPersonByUserId(adminUserId);
                            var advisorPerson = new BLPerson().GetPersonByUserId(CurrentUserId);

                            var subject = "Confirm your WERC Environmental Design Contest 2023 account.";

                            var bodyForAdmin = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + adminPerson.FirstName + " " + adminPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";

                            var bodyForAdvisor = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + advisorPerson.FirstName + " " + advisorPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";


                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdmin,
                                IsBodyHtml = true,
                                EmailList = new string[] { specialEmail }
                            };

                            for (var i = 0; i < emailHelper.EmailList.Length; i++)
                            {
                                emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                            }

                            emailHelper.CurrentUserId = CurrentUserId;
                            emailHelper.Send();

                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdvisor,
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
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(awardNominationUrl))
            //{
            //    UIHelper.DeleteFile(oldFlashTalkReportUrl);
            //}

            var jsonData = new
            {
                flashTalkReportUrl = HttpUtility.HtmlDecode(falshTalkReportUrl),
                flashTalkReportFileName = UploadedFlashTalkReport.FileName,
                flashTalkReportUrlIcon = "/Resources/Images/Mimetypes128x128/" + falshTalkReportUrl.Split(new string[] { "?CT=" }, StringSplitOptions.RemoveEmptyEntries)[1],
                success = result,
                message = "Your flashTalk report successfully has been uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("u_b")]
        [HttpPost]
        public ActionResult UploadBrochure(int teamId, string oldBrochureUrl, HttpPostedFileBase UploadedBrochure)
        {
            var result = true;
            var blTeam = new BLTeam();
            string brochureUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    brochureUrl = UIHelper.UploadFile(UploadedBrochure, "/Resources/Uploaded/Teams/Brochure/" + CurrentUserId.Replace("-", ""));

                    result = blTeam.UploadBrochure(teamId, brochureUrl);
                    if (string.IsNullOrWhiteSpace(brochureUrl) == true && result == true)
                    {
                        var blSubmissionRule = new BLSubmissionRule();
                        var submissionRuleList = blSubmissionRule.GetAllSubmissionRule();
                        var brochureDays = DateTime.Parse(submissionRuleList[2].DueDate).Subtract(DateTime.Now).TotalDays;
                        var days = brochureDays - Math.Truncate(brochureDays);
                        if (days > 0)
                        {
                            brochureDays++;
                        }

                        if (brochureDays > 0)
                        {
                            var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                            var adminPerson = new BLPerson().GetPersonByUserId(adminUserId);
                            var advisorPerson = new BLPerson().GetPersonByUserId(CurrentUserId);

                            var subject = "Confirm your WERC Environmental Design Contest 2023 account.";

                            var bodyForAdmin = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + adminPerson.FirstName + " " + adminPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";

                            var bodyForAdvisor = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + advisorPerson.FirstName + " " + advisorPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";


                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdmin,
                                IsBodyHtml = true,
                                EmailList = new string[] { specialEmail }
                            };

                            for (var i = 0; i < emailHelper.EmailList.Length; i++)
                            {
                                emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                            }

                            emailHelper.CurrentUserId = CurrentUserId;
                            emailHelper.Send();

                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdvisor,
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
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(brochureUrl))
            //{
            //    UIHelper.DeleteFile(oldBrochureUrl);
            //}

            var jsonData = new
            {
                brochureUrl = HttpUtility.HtmlDecode(brochureUrl),
                brochureFileName = UploadedBrochure.FileName,
                brochureUrlIcon = "/Resources/Images/Mimetypes128x128/" + brochureUrl.Split(new string[] { "?CT=" }, StringSplitOptions.RemoveEmptyEntries)[1],
                success = result,
                message = "Your brochure successfully has been uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }


        [ActionName("u_a")]
        [HttpPost]
        public ActionResult UploadAwardNomination(int teamId, string oldAwardNominationUrl, HttpPostedFileBase UploadedAwardNomination)
        {
            var result = true;
            var blTeam = new BLTeam();
            string awardNominationUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    awardNominationUrl = UIHelper.UploadFile(UploadedAwardNomination, "/Resources/Uploaded/Teams/AwardNomination/" + CurrentUserId.Replace("-", ""));

                    result = blTeam.UploadAwardNomination(teamId, awardNominationUrl);
                    if (string.IsNullOrWhiteSpace(awardNominationUrl) == true && result == true)
                    {
                        var blSubmissionRule = new BLSubmissionRule();
                        var submissionRuleList = blSubmissionRule.GetAllSubmissionRule();
                        var awardNominationDays = DateTime.Parse(submissionRuleList[2].DueDate).Subtract(DateTime.Now).TotalDays;
                        var days = awardNominationDays - Math.Truncate(awardNominationDays);
                        if (days > 0)
                        {
                            awardNominationDays++;
                        }

                        if (awardNominationDays > 0)
                        {
                            var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                            var adminPerson = new BLPerson().GetPersonByUserId(adminUserId);
                            var advisorPerson = new BLPerson().GetPersonByUserId(CurrentUserId);

                            var subject = "Confirm your WERC Environmental Design Contest 2023 account.";

                            var bodyForAdmin = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + adminPerson.FirstName + " " + adminPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";

                            var bodyForAdvisor = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + advisorPerson.FirstName + " " + advisorPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";


                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdmin,
                                IsBodyHtml = true,
                                EmailList = new string[] { specialEmail }
                            };

                            for (var i = 0; i < emailHelper.EmailList.Length; i++)
                            {
                                emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                            }

                            emailHelper.CurrentUserId = CurrentUserId;
                            emailHelper.Send();

                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdvisor,
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
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(awardNominationUrl))
            //{
            //    UIHelper.DeleteFile(oldAwardNominationUrl);
            //}

            var jsonData = new
            {
                awardNominationUrl = HttpUtility.HtmlDecode(awardNominationUrl),
                awardNominationFileName = UploadedAwardNomination.FileName,
                awardNominationUrlIcon = "/Resources/Images/Mimetypes128x128/" + awardNominationUrl.Split(new string[] { "?CT=" }, StringSplitOptions.RemoveEmptyEntries)[1],
                success = result,
                message = "Your award nomination successfully has been uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }
        [ActionName("usr")]
        [HttpPost]
        public ActionResult UploadSubmissionRule(int teamId, int submissionRuleId, HttpPostedFileBase uploadedSubmissionRule)
        {
            var result = true;
            var blTeam = new BLTeam();

            var team = blTeam.GetTeamById(teamId);

            string submissionRuleUrl = string.Empty;

            try
            {
                var blSubmissionRule = new BLSubmissionRule();
                var submissionRule = blSubmissionRule.GetSubmissionRuleById(submissionRuleId);
                if (ModelState.IsValid)
                {
                    submissionRuleUrl = UIHelper.UploadFile(uploadedSubmissionRule, "/Resources/Uploaded/Teams/SubmissionRule/" + CurrentUserId.Replace("-", ""));

                    result = blTeam.UploadSubmissionRule(teamId, submissionRuleId, submissionRuleUrl);

                    #region
                    //if (string.IsNullOrWhiteSpace(submissionRuleUrl) == true && result == true)
                    //{

                    //    var submissionRuleLateDays = DateTime.Parse(submissionRule.DueDate).Subtract(DateTime.Now).TotalDays;
                    //    var days = submissionRuleLateDays - Math.Truncate(submissionRuleLateDays);

                    //    if (days > 0)
                    //    {
                    //        submissionRuleLateDays++;
                    //    }

                    //    if (submissionRuleLateDays > 0)
                    //    {
                    //        var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                    //        var adminPerson = new BLPerson().GetPersonByUserId(adminUserId);
                    //        var advisorPerson = new BLPerson().GetPersonByUserId(CurrentUserId);

                    //        var subject = "Confirm your WERC Environmental Design Contest 2023 account.";

                    //        var bodyForAdmin = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                    //             "<br/>" +
                    //             "Dear " + adminPerson.FirstName + " " + adminPerson.LastName + ", " +
                    //             "<br/>" +
                    //             "<br/>";

                    //        var bodyForAdvisor = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                    //             "<br/>" +
                    //             "Dear " + advisorPerson.FirstName + " " + advisorPerson.LastName + ", " +
                    //             "<br/>" +
                    //             "<br/>";


                    //        emailHelper = new EmailHelper
                    //        {
                    //            SpecialEmail = specialEmail,
                    //            Subject = subject,
                    //            Body = bodyForAdmin,
                    //            IsBodyHtml = true,
                    //            EmailList = new string[] { specialEmail }
                    //        };

                    //        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                    //        {
                    //            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                    //        }

                    //        emailHelper.CurrentUserId = CurrentUserId;
                    //        emailHelper.Send();

                    //        emailHelper = new EmailHelper
                    //        {
                    //            SpecialEmail = specialEmail,
                    //            Subject = subject,
                    //            Body = bodyForAdvisor,
                    //            IsBodyHtml = true,
                    //            EmailList = new string[] { specialEmail }
                    //        };

                    //        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                    //        {
                    //            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                    //        }

                    //        emailHelper.CurrentUserId = CurrentUserId;
                    //        emailHelper.Send();


                    //    }
                    //}
                    //else 
                    #endregion

                    if (string.IsNullOrWhiteSpace(submissionRuleUrl) == false && result == true)
                    {

                        var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                        var adminPerson = new BLPerson().GetPersonByUserId(adminUserId);
                        var uploaderPerson = new BLPerson().GetPersonByUserId(CurrentUserId);

                        var subject = "WERC Team Submission Ready for Review";

                        var body =
                               " <ul>" +
                               "     <li>Team " + team.Name + "-" + team.University + "-" + team.Task
                                                + " has submitted their " + team.Task + " " + submissionRule.Name + " for your review." +
                                    "</li>" +
                               "     <li>" +
                               "         Next steps:" +
                               "         <ul>" +
                               "             <li>" +
                               "                 <ul>" +
                               "                     <li>Sign in to werc.nmsu.edu</li>" +
                               "                     <li>Go to TEAMS tab</li>" +
                               "                     <li>Teams that have submitted the " + submissionRule.Name + " will have a “" + submissionRule.Name + "“ button with a green checkmark. </li>" +
                               "                     <li>Click " + submissionRule.Name + " button.</li>" +
                               "                     <li>You will be given the option to view or download the report.</li>" +
                               "                 </ul>" +
                               "             </li>" +
                               "         </ul>" +
                               "     </li>" +
                               " </ul>";

                        List<string> allEmails = new List<string>();

                        allEmails.Add(specialEmail);
                        allEmails.Add(adminPerson.Email);

                        emailHelper = new EmailHelper
                        {
                            AdminEmail = adminPerson.Email,
                            SpecialEmail = specialEmail,
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true,
                        };

                        if (submissionRule.ShowReport == true)
                        {
                            var blUserTask = new BLUserTask();
                            var userIds = blUserTask.GetUsersByTask(team.TaskId).ToArray();

                            var blPerson = new BLPerson();
                            var emails = blPerson.GetEmailsByUserIds(userIds);

                            allEmails.AddRange(emails);
                        }

                        emailHelper.EmailList = allEmails.ToArray();

                        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                        {
                            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                        }

                        emailHelper.CurrentUserId = CurrentUserId;
                        emailHelper.Send();
                    }
                }


            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(submissionRuleUrl))
            //{
            //    UIHelper.DeleteFile(oldSubmissionRuleUrl);
            //}

            var jsonData = new
            {
                submissionRuleUrl = HttpUtility.HtmlDecode(submissionRuleUrl),
                submissionRuleFileName = uploadedSubmissionRule.FileName,
                submissionRuleUrlIcon = "/Resources/Images/Mimetypes128x128/" + submissionRuleUrl.Split(new string[] { "?CT=" }, StringSplitOptions.RemoveEmptyEntries)[1],
                success = result,
                message = "Your submission successfully has been uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }
        [ActionName("urr")]
        [HttpPost]
        public ActionResult UploadWrittenReport(int teamId, string oldWrittenReportUrl, HttpPostedFileBase UploadedWrittenReport)
        {
            var result = true;
            var blTeam = new BLTeam();
            string writtenReportUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    writtenReportUrl = UIHelper.UploadFile(UploadedWrittenReport, "/Resources/Uploaded/Teams/WrittenReport/" + CurrentUserId.Replace("-", ""));

                    result = blTeam.UploadWrittenReport(teamId, writtenReportUrl);
                    if (string.IsNullOrWhiteSpace(writtenReportUrl) == true && result == true)
                    {
                        var blSubmissionRule = new BLSubmissionRule();
                        var submissionRuleList = blSubmissionRule.GetAllSubmissionRule();
                        var writtenReportDays = DateTime.Parse(submissionRuleList[2].DueDate).Subtract(DateTime.Now).TotalDays;
                        var days = writtenReportDays - Math.Truncate(writtenReportDays);
                        if (days > 0)
                        {
                            writtenReportDays++;
                        }

                        if (writtenReportDays > 0)
                        {
                            var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                            var adminPerson = new BLPerson().GetPersonByUserId(adminUserId);
                            var advisorPerson = new BLPerson().GetPersonByUserId(CurrentUserId);

                            var subject = "Confirm your WERC Environmental Design Contest 2023 account.";

                            var bodyForAdmin = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + adminPerson.FirstName + " " + adminPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";

                            var bodyForAdvisor = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                                 "<br/>" +
                                 "Dear " + advisorPerson.FirstName + " " + advisorPerson.LastName + ", " +
                                 "<br/>" +
                                 "<br/>";


                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdmin,
                                IsBodyHtml = true,
                                EmailList = new string[] { specialEmail }
                            };

                            for (var i = 0; i < emailHelper.EmailList.Length; i++)
                            {
                                emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                            }

                            emailHelper.CurrentUserId = CurrentUserId;
                            emailHelper.Send();

                            emailHelper = new EmailHelper
                            {
                                SpecialEmail = specialEmail,
                                Subject = subject,
                                Body = bodyForAdvisor,
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
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(writtenReportUrl))
            //{
            //    UIHelper.DeleteFile(oldWrittenReportUrl);
            //}

            var jsonData = new
            {
                writtenReportUrl = HttpUtility.HtmlDecode(writtenReportUrl),
                writtenReportFileName = UploadedWrittenReport.FileName,
                writtenReportUrlIcon = "/Resources/Images/Mimetypes128x128/" + writtenReportUrl.Split(new string[] { "?CT=" }, StringSplitOptions.RemoveEmptyEntries)[1],
                success = result,
                message = "Your written report successfully has been uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("upbc")]
        [HttpPost]
        public ActionResult UpdatePaidByCheque(int teamId)
        {
            var blTeam = new BLTeam();
            var lastStatus = blTeam.ReversePaidByCheque(teamId);
            var jsonData = new
            {
                lastStatus,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("uta")]
        [HttpPost]
        public ActionResult UpdateTeamActivation(int teamId)
        {
            var blTeam = new BLTeam();
            var lastStatus = blTeam.ReverseTeamActivation(teamId);
            var jsonData = new
            {
                lastStatus,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("ssa_ats")]
        [HttpPost]
        public ActionResult SetDeactiveAllTeams()
        {
            var blTeam = new BLTeam();
            blTeam.SetAllowSuppressScoringAllTeams(true);

            var jsonData = new
            {
                result = true,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("sa_ats")]
        [HttpPost]
        public ActionResult AllowAllTeamsToScring()
        {
            var blTeam = new BLTeam();
            blTeam.SetAllowSuppressScoringAllTeams(false);

            var jsonData = new
            {
                result = true,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("ssa_atsvs")]
        [HttpPost]
        public ActionResult SetDeactiveAllTeamsToViewScore()
        {
            var blTeam = new BLTeam();
            blTeam.SetAllowViewScoreAllTeams(true);

            var jsonData = new
            {
                result = true,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("sa_atsvs")]
        [HttpPost]
        public ActionResult AllowAllTeamsToViewScore()
        {
            var blTeam = new BLTeam();
            blTeam.SetAllowViewScoreAllTeams(false);

            var jsonData = new
            {
                result = true,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("utps")]
        [HttpPost]
        public ActionResult UpdateTeamPayStatus(int teamId)
        {
            var blTeam = new BLTeam();
            var lastStatus = blTeam.UpdateTeamPayStatus(teamId);
            var jsonData = new
            {
                lastStatus,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("utss")]
        [HttpPost]
        public ActionResult UpdateTeamSuppressScoring(int teamId)
        {
            var blTeam = new BLTeam();
            var lastStatus = blTeam.ReverseTeamSuppressScoring(teamId);
            var jsonData = new
            {
                lastStatus,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("dt")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = true;

            var blTeam = new BLTeam();

            var team = blTeam.GetTeamById(id);
            if (team.PayStatus == true)
            {
                var jsonDeleteResult = new
                {
                    success = false,
                    message = "You can not delete this team because the payment status of this team is PAID.",
                    teamId = id,
                };

                return Json(jsonDeleteResult, JsonRequestBehavior.AllowGet);
            }

            result = blTeam.DeleteTeam(id);

            string resultMessage = string.Empty;

            if (result == true)
            {
                resultMessage = new BaseViewModel()["Team Has been deleted successfuly."];
            }
            else
            {
                resultMessage = new BaseViewModel()["This team has members. You can't delete it..."];

            }

            var jsonResult = new
            {
                success = result,
                message = resultMessage,
                teamId = id,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [ActionName("utd")]
        [HttpPost]
        public ActionResult UpdateteamPayment(int teamId, decimal payment)
        {
            var blTeam = new BLTeam();
            var lastStatus = blTeam.UpdateteamPayment(teamId, payment);
            var jsonData = new
            {
                lastStatus,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("gpn")]
        [HttpPost]
        public ActionResult GetProjectName(int teamId)
        {
            var blTeam = new BLTeam();
            var projectName = blTeam.GetTeamById(teamId).ProjectName ?? "";
            var jsonData = new
            {
                projectName,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TeamEdit", model);
        }

        [ActionName("upn")]
        [HttpPost]
        public ActionResult UpdateProjectName(int teamId, string projectName)
        {
            var blTeam = new BLTeam();

            var jsonData = new
            {
                result = blTeam.UpdateProjectName(teamId, projectName)
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("ttoa")]
        [HttpPost]
        public ActionResult TransferTeamToAnotherAdvisor(int teamId, string advisorId)
        {
            var blTeam = new BLTeam();

            var jsonData = new
            {
                result = blTeam.TransferTeamToAnotherAdvisor(teamId, advisorId)
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("rtlos")]
        [HttpPost]
        public ActionResult RemoveTheLate(int teamId)
        {

            var blTeam = new BLTeam();

            var jsonData = new
            {
                result = blTeam.RemoveTheLate(teamId, 3)
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}