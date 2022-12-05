using BLL.Base;

using Model;
using Model.ViewModels.Invoice;
using Model.ViewModels.SubmissionRule;
using Model.ViewModels.Team;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLTeam : BLBase
    {
        public VmTeam GetTeamById(int id)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();

            var team = teamRepository.GetTeamById(id);
            var vmTeam = new VmTeam
            {
                Id = team.Id,
                Payment = team.Payment,
                TaskId = team.TaskId,
                ProjectName = team.ProjectName,
                Name = team.Name,
                TeamNumber = team.TeamNumber,
                Task = team.Task,
                TeamState = team.TeamState.Value,
                TeamImageUrl = team.TeamImageUrl,
                TeamStateDescription = GetTeamStateDescription(team.TeamState),
                RegistrationStatus = team.RegistrationStatus,
                Survey = team.Survey,
                MemberName = team.MemberName,
                PhoneNumber = team.PhoneNumber,
                WorkPhoneNumber = team.WorkPhoneNumber ?? "",
                Identifier = team.Identifier,
                Sex = team.Sex,
                BirthDate = team.BirthDate,
                UserName = team.UserName,
                Email = team.Email,
                RegisterDate = team.RegisterDate,
                RoleName = team.RoleName,
                RoleId = team.RoleId,
                UserDefiner = team.UserDefiner,
                LastSignIn = team.LastSignIn,
                UniversityId = team.UniversityId,
                University = team.University,
                JacketSizeId = team.JacketSizeId,
                JacketSize = team.JacketSize ?? "",
                DietTypeId = team.DietTypeId,
                DietType = team.DietType ?? "",
                StreetLine1 = team.StreetLine1,
                StreetLine2 = team.StreetLine2,
                City = team.City,
                State = team.State,
                ZipCode = team.ZipCode,
                EmgyPersonRelationship = team.EmgyPersonRelationship,
                EmgyPersonPhoneNumber = team.EmgyPersonPhoneNumber,
                EmgyPersonLastName = team.EmgyPersonLastName,
                EmgyPersonFirstName = team.EmgyPersonFirstName,
                ShortBio = team.ShortBio,
                T_Shirt_Size = team.T_Shirt_Size,
                ProfilePictureUrl = team.ProfilePictureUrl,
                UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                LabResultUrl = team.LabResultUrl,

                PreliminaryReportUrl = (team.RoleName.ToLower().Contains("advisor") || team.RoleName.ToLower().Contains("leader"))
                    ?
                    string.IsNullOrWhiteSpace(team.PreliminaryReportUrl) ? "-?CT=application_pdf.png" : team.PreliminaryReportUrl
                    :
                    string.IsNullOrWhiteSpace(team.PreliminaryReportUrl) ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",

                PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                PreliminaryReportDate = team.PreliminaryReportDate,

                FlashTalkReportUrl = (team.RoleName.ToLower().Contains("advisor") || team.RoleName.ToLower().Contains("leader"))
                    ?
                    string.IsNullOrWhiteSpace(team.FlashTalkReportUrl) ? "-?CT=application_pdf.png" : team.FlashTalkReportUrl
                    :
                    string.IsNullOrWhiteSpace(team.FlashTalkReportUrl) ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",

                FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                FlashTalkReportDate = team.FlashTalkReportDate,

                BrochureUrl = (team.RoleName.ToLower().Contains("advisor") || team.RoleName.ToLower().Contains("leader"))
                    ?
                    string.IsNullOrWhiteSpace(team.BrochureUrl) ? "-?CT=application_pdf.png" : team.BrochureUrl
                    :
                    string.IsNullOrWhiteSpace(team.BrochureUrl) ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",

                BrochureUrlForMember = team.BrochureUrl,
                BrochureDate = team.BrochureDate,


                AwardNominationUrl = (team.RoleName.ToLower().Contains("advisor") || team.RoleName.ToLower().Contains("leader"))
                    ?
                    string.IsNullOrWhiteSpace(team.AwardNominationUrl) ? "-?CT=application_pdf.png" : team.AwardNominationUrl
                    :
                    string.IsNullOrWhiteSpace(team.AwardNominationUrl) ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",

                AwardNominationUrlForMember = team.AwardNominationUrl,
                AwardNominationDate = team.AwardNominationDate,

                OpenTaskTestPlanUrl = (team.RoleName.ToLower().Contains("advisor") || team.RoleName.ToLower().Contains("leader"))
                    ?
                    string.IsNullOrWhiteSpace(team.OpenTaskTestPlanUrl) ? "-?CT=application_pdf.png" : team.OpenTaskTestPlanUrl
                    :
                    string.IsNullOrWhiteSpace(team.OpenTaskTestPlanUrl) ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",

                OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                WrittenReportUrl = (team.RoleName.ToLower().Contains("advisor") || team.RoleName.ToLower().Contains("leader"))
                    ?
                    string.IsNullOrWhiteSpace(team.WrittenReportUrl) ? "-?CT=application_pdf.png" : team.WrittenReportUrl
                    :
                    string.IsNullOrWhiteSpace(team.WrittenReportUrl) ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",

                WrittenReportUrlForMember = team.WrittenReportUrl,

                WrittenReportDate = team.WrittenReportDate,

                Preliminary = team.Preliminary,
                Scored = team.Scored,
                OpenTaskTestPlan = team.OpenTaskTestPlan,

                Shipping = "",
                ResumeUrl = team.ResumeUrl,
                Date = team.Date,
                EmailConfirmed = team.EmailConfirmed,
                LockoutEnabled = team.LockoutEnabled,
                PayStatus = team.PayStatus,
                PaidByCheque = team.PaidByCheque,
                PaidByChequePercent = team.PaidByChequePercent,
                SuppressScoring = team.SuppressScoring,
                Deactivate = team.Deactivate,
                ViewFinalScore = team.ViewFinalScore,
                PaymentTypeId = team.PaymentTypeId,
                PaymentType = team.PaymentType,
                PaymentTypeDescription = team.PaymentTypeDescription,
            };


            return vmTeam;
        }

        public IEnumerable<VmTeam> GetLabTeams(string labUserId, string teamName)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeams(labUserId, teamName);
            var vmTeamList = from team in teamList
                             select new VmTeam
                             {
                                 Id = team.TeamId,
                                 TaskId = team.TaskId,
                                 ProjectName = team.ProjectName,
                                 Name = team.TeamName,
                                 Task = team.TaskName,
                                 TeamState = team.TeamState.Value,
                                 TeamImageUrl = team.TeamImageUrl,
                                 TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                 RegistrationStatus = team.RegistrationStatus,
                                 Survey = team.Survey,
                                 University = team.University,
                                 UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                 Date = team.Date,
                                 TaskImageUrl = team.TaskImageUrl,

                                 PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                 PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                 PreliminaryReportDate = team.PreliminaryReportDate,

                                 FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                 FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                 FlashTalkReportDate = team.FlashTalkReportDate,

                                 BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                 BrochureUrlForMember = team.BrochureUrl,
                                 BrochureDate = team.BrochureDate,

                                 AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                 AwardNominationUrlForMember = team.AwardNominationUrl,
                                 AwardNominationDate = team.AwardNominationDate,

                                 OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                 OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                 OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                 WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                 WrittenReportUrlForMember = team.WrittenReportUrl,
                                 WrittenReportDate = team.WrittenReportDate,

                                 Preliminary = team.Preliminary,
                                 Scored = team.Scored,
                                 OpenTaskTestPlan = team.OpenTaskTestPlan,

                                 PayStatus = team.PayStatus,
                                 PaidByCheque = team.PaidByCheque,
                                 PaidByChequePercent = team.PaidByChequePercent,



                             };

            return vmTeamList;
        }

        public IEnumerable<VmTeam> GetLabTeams(string labUserId)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeams(labUserId);

            var vmTeamList = from team in teamList
                             select new VmTeam
                             {
                                 Id = team.TeamId,
                                 TaskId = team.TaskId,
                                 ProjectName = team.ProjectName,
                                 Name = team.TeamName,
                                 MemberName = team.MemberName,
                                 Task = team.TaskName,
                                 TeamState = team.TeamState.Value,
                                 TeamImageUrl = team.TeamImageUrl,
                                 TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                 RegistrationStatus = team.RegistrationStatus,
                                 Survey = team.Survey,
                                 University = team.University,
                                 UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                 Date = team.Date,
                                 TaskImageUrl = team.TaskImageUrl,

                                 PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                 PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                 PreliminaryReportDate = team.PreliminaryReportDate,

                                 FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                 FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                 FlashTalkReportDate = team.FlashTalkReportDate,

                                 BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                 BrochureUrlForMember = team.BrochureUrl,
                                 BrochureDate = team.BrochureDate,

                                 AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                 AwardNominationUrlForMember = team.AwardNominationUrl,
                                 AwardNominationDate = team.AwardNominationDate,

                                 OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                 OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                 OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                 WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                 WrittenReportUrlForMember = team.WrittenReportUrl,
                                 WrittenReportDate = team.WrittenReportDate,

                                 Preliminary = team.Preliminary,
                                 Scored = team.Scored,
                                 OpenTaskTestPlan = team.OpenTaskTestPlan,

                                 PayStatus = team.PayStatus,
                                 PaidByCheque = team.PaidByCheque,
                                 PaidByChequePercent = team.PaidByChequePercent,
                             };

            return vmTeamList;
        }

        public List<VmTeam> GetAdvisorTeams(string advisorId)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeams(advisorId);
            var vmTeamList = (from team in teamList
                              select new VmTeam
                              {
                                  Id = team.Id,
                                  Payment = team.Payment,
                                  TaskId = team.TaskId,
                                  ProjectName = team.ProjectName,
                                  Name = team.Name,
                                  TeamNumber = team.TeamNumber,
                                  Task = team.Task,
                                  TeamState = team.TeamState.Value,
                                  TeamImageUrl = team.TeamImageUrl,
                                  TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                  RegistrationStatus = team.RegistrationStatus,
                                  Survey = team.Survey,
                                  MemberName = team.MemberName,
                                  PhoneNumber = team.PhoneNumber,
                                  WorkPhoneNumber = team.WorkPhoneNumber ?? "",
                                  Identifier = team.Identifier,
                                  Sex = team.Sex,
                                  BirthDate = team.BirthDate,
                                  UserName = team.UserName,
                                  Email = team.Email,
                                  RegisterDate = team.RegisterDate,
                                  RoleName = team.RoleName,
                                  RoleId = team.RoleId,
                                  UserDefiner = team.UserDefiner,
                                  LastSignIn = team.LastSignIn,
                                  UniversityId = team.UniversityId,
                                  University = team.University,
                                  JacketSizeId = team.JacketSizeId,
                                  JacketSize = team.JacketSize ?? "",
                                  DietTypeId = team.DietTypeId,
                                  DietType = team.DietType ?? "",
                                  StreetLine1 = team.StreetLine1,
                                  StreetLine2 = team.StreetLine2,
                                  City = team.City,
                                  State = team.State,
                                  ZipCode = team.ZipCode,
                                  EmgyPersonRelationship = team.EmgyPersonRelationship,
                                  EmgyPersonPhoneNumber = team.EmgyPersonPhoneNumber,
                                  EmgyPersonLastName = team.EmgyPersonLastName,
                                  EmgyPersonFirstName = team.EmgyPersonFirstName,
                                  ShortBio = team.ShortBio,
                                  T_Shirt_Size = team.T_Shirt_Size,
                                  ProfilePictureUrl = team.ProfilePictureUrl,
                                  UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                  LabResultUrl = team.LabResultUrl,

                                  PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                  PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                  PreliminaryReportDate = team.PreliminaryReportDate,

                                  FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                  FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                  FlashTalkReportDate = team.FlashTalkReportDate,

                                  BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                  BrochureUrlForMember = team.BrochureUrl,
                                  BrochureDate = team.BrochureDate,

                                  AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                  AwardNominationUrlForMember = team.AwardNominationUrl,
                                  AwardNominationDate = team.AwardNominationDate,

                                  OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                  OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                  OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                  WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                  WrittenReportUrlForMember = team.WrittenReportUrl,
                                  WrittenReportDate = team.WrittenReportDate,

                                  Shipping = "",
                                  ResumeUrl = team.ResumeUrl,
                                  Date = team.Date,
                                  EmailConfirmed = team.EmailConfirmed,
                                  LockoutEnabled = team.LockoutEnabled,
                                  PayStatus = team.PayStatus,
                                  PaidByCheque = team.PaidByCheque,
                                  PaidByChequePercent = team.PaidByChequePercent,
                                  SuppressScoring = team.SuppressScoring,
                                  Deactivate = team.Deactivate,
                                  Preliminary = team.Preliminary,
                                  Scored = team.Scored,
                                  OpenTaskTestPlan = team.OpenTaskTestPlan,
                                  PaymentTypeId = team.PaymentTypeId,
                                  PaymentType = team.PaymentType,
                                  PaymentTypeDescription = team.PaymentTypeDescription,
                                  RegisterForFlashTalk = team.RegisterForFlashTalk,
                                  TeamRegisterForFlashTalk = team.TeamRegisterForFlashTalk,
                              }).ToList();
            var teamIds = vmTeamList.Select(t => t.Id).ToArray();

            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);

            foreach (var team in vmTeamList)
            {
                team.SubmissionRuleList = (from s in submissionRuleList
                                           where s.TeamId == team.Id
                                           select new VmSubmissionRule
                                           {
                                               Id = s.Id,
                                               TeamId = s.TeamId,
                                               UploadDate = s.UploadDate,
                                               DueDate = s.DueDate,
                                               DueDateOrder = DateTime.Parse(s.DueDate),
                                               Description = s.Description,
                                               Name = s.Name,
                                               ShowLate = s.ShowLate,
                                               ShowReport = s.ShowReport,
                                               TeamName = s.TeamName,
                                               PayStatus = s.PayStatus,
                                               RegistrationStatus = s.RegistrationStatus,
                                               TeamPayStatus = s.TeamPayStatus,
                                               TeamRegistrationStatus = s.TeamRegistrationStatus,
                                               SubmissionRuleUrl = s.SubmissionRuleUrl,

                                           }).OrderBy(p => p.DueDateOrder).ToList();

            }
            return vmTeamList;
        }
        public List<VmTeam> GetTeamsByLeader(string leaderId)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamId = viewTeamMemberRepository.GetTeamMember(leaderId).TeamId;

            var advisorId = teamRepository.GetTeamById(teamId).MemberUserId;

            var teamList = teamRepository.GetMemberUserTeams(advisorId);
            var vmTeamList = (from team in teamList
                              select new VmTeam
                              {
                                  Id = team.Id,
                                  Payment = team.Payment,
                                  TaskId = team.TaskId,
                                  ProjectName = team.ProjectName,
                                  Name = team.Name,
                                  TeamNumber = team.TeamNumber,
                                  Task = team.Task,
                                  TeamState = team.TeamState.Value,
                                  TeamImageUrl = team.TeamImageUrl,
                                  TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                  RegistrationStatus = team.RegistrationStatus,
                                  Survey = team.Survey,
                                  MemberName = team.MemberName,
                                  PhoneNumber = team.PhoneNumber,
                                  WorkPhoneNumber = team.WorkPhoneNumber ?? "",
                                  Identifier = team.Identifier,
                                  Sex = team.Sex,
                                  BirthDate = team.BirthDate,
                                  UserName = team.UserName,
                                  Email = team.Email,
                                  RegisterDate = team.RegisterDate,
                                  RoleName = team.RoleName,
                                  RoleId = team.RoleId,
                                  UserDefiner = team.UserDefiner,
                                  LastSignIn = team.LastSignIn,
                                  UniversityId = team.UniversityId,
                                  University = team.University,
                                  JacketSizeId = team.JacketSizeId,
                                  JacketSize = team.JacketSize ?? "",
                                  DietTypeId = team.DietTypeId,
                                  DietType = team.DietType ?? "",
                                  StreetLine1 = team.StreetLine1,
                                  StreetLine2 = team.StreetLine2,
                                  City = team.City,
                                  State = team.State,
                                  ZipCode = team.ZipCode,
                                  EmgyPersonRelationship = team.EmgyPersonRelationship,
                                  EmgyPersonPhoneNumber = team.EmgyPersonPhoneNumber,
                                  EmgyPersonLastName = team.EmgyPersonLastName,
                                  EmgyPersonFirstName = team.EmgyPersonFirstName,
                                  ShortBio = team.ShortBio,
                                  T_Shirt_Size = team.T_Shirt_Size,
                                  ProfilePictureUrl = team.ProfilePictureUrl,
                                  UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                  LabResultUrl = team.LabResultUrl,

                                  PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                  PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                  PreliminaryReportDate = team.PreliminaryReportDate,

                                  FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                  FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                  FlashTalkReportDate = team.FlashTalkReportDate,

                                  BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                  BrochureUrlForMember = team.BrochureUrl,
                                  BrochureDate = team.BrochureDate,

                                  AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                  AwardNominationUrlForMember = team.AwardNominationUrl,
                                  AwardNominationDate = team.AwardNominationDate,

                                  OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                  OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                  OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                  WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                  WrittenReportUrlForMember = team.WrittenReportUrl,
                                  WrittenReportDate = team.WrittenReportDate,

                                  Shipping = "",
                                  ResumeUrl = team.ResumeUrl,
                                  Date = team.Date,
                                  EmailConfirmed = team.EmailConfirmed,
                                  LockoutEnabled = team.LockoutEnabled,
                                  PayStatus = team.PayStatus,
                                  PaidByCheque = team.PaidByCheque,
                                  PaidByChequePercent = team.PaidByChequePercent,
                                  SuppressScoring = team.SuppressScoring,
                                  Deactivate = team.Deactivate,
                                  Preliminary = team.Preliminary,
                                  Scored = team.Scored,
                                  OpenTaskTestPlan = team.OpenTaskTestPlan,
                              }).ToList();
            var teamIds = vmTeamList.Select(t => t.Id).ToArray();

            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);

            foreach (var team in vmTeamList)
            {
                team.SubmissionRuleList = (from s in submissionRuleList
                                           where s.TeamId == team.Id
                                           select new VmSubmissionRule
                                           {
                                               Id = s.Id,
                                               TeamId = s.TeamId,
                                               UploadDate = s.UploadDate,
                                               DueDate = s.DueDate,
                                               DueDateOrder = DateTime.Parse(s.DueDate),
                                               Description = s.Description,
                                               Name = s.Name,
                                               ShowLate = s.ShowLate,
                                               ShowReport = s.ShowReport,
                                               TeamName = s.TeamName,
                                               PayStatus = s.PayStatus,
                                               RegistrationStatus = s.RegistrationStatus,
                                               TeamPayStatus = s.TeamPayStatus,
                                               TeamRegistrationStatus = s.TeamRegistrationStatus,
                                               SubmissionRuleUrl = s.SubmissionRuleUrl,

                                           }).OrderBy(p => p.DueDateOrder).ToList();

            }
            return vmTeamList;
        }
        public List<VmTeam> GetTeamsByCoAdvisor(string coAdvisorId)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamId = viewTeamMemberRepository.GetTeamMember(coAdvisorId).TeamId;

            var advisorId = teamRepository.GetTeamById(teamId).MemberUserId;

            var teamList = teamRepository.GetMemberUserTeams(advisorId);
            var vmTeamList = (from team in teamList
                              select new VmTeam
                              {
                                  Id = team.Id,
                                  Payment = team.Payment,
                                  TaskId = team.TaskId,
                                  ProjectName = team.ProjectName,
                                  Name = team.Name,
                                  TeamNumber = team.TeamNumber,
                                  Task = team.Task,
                                  TeamState = team.TeamState.Value,
                                  TeamImageUrl = team.TeamImageUrl,
                                  TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                  RegistrationStatus = team.RegistrationStatus,
                                  Survey = team.Survey,
                                  MemberName = team.MemberName,
                                  PhoneNumber = team.PhoneNumber,
                                  WorkPhoneNumber = team.WorkPhoneNumber ?? "",
                                  Identifier = team.Identifier,
                                  Sex = team.Sex,
                                  BirthDate = team.BirthDate,
                                  UserName = team.UserName,
                                  Email = team.Email,
                                  RegisterDate = team.RegisterDate,
                                  RoleName = team.RoleName,
                                  RoleId = team.RoleId,
                                  UserDefiner = team.UserDefiner,
                                  LastSignIn = team.LastSignIn,
                                  UniversityId = team.UniversityId,
                                  University = team.University,
                                  JacketSizeId = team.JacketSizeId,
                                  JacketSize = team.JacketSize ?? "",
                                  DietTypeId = team.DietTypeId,
                                  DietType = team.DietType ?? "",
                                  StreetLine1 = team.StreetLine1,
                                  StreetLine2 = team.StreetLine2,
                                  City = team.City,
                                  State = team.State,
                                  ZipCode = team.ZipCode,
                                  EmgyPersonRelationship = team.EmgyPersonRelationship,
                                  EmgyPersonPhoneNumber = team.EmgyPersonPhoneNumber,
                                  EmgyPersonLastName = team.EmgyPersonLastName,
                                  EmgyPersonFirstName = team.EmgyPersonFirstName,
                                  ShortBio = team.ShortBio,
                                  T_Shirt_Size = team.T_Shirt_Size,
                                  ProfilePictureUrl = team.ProfilePictureUrl,
                                  UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                  LabResultUrl = team.LabResultUrl,

                                  PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                  PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                  PreliminaryReportDate = team.PreliminaryReportDate,

                                  FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                  FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                  FlashTalkReportDate = team.FlashTalkReportDate,

                                  BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                  BrochureUrlForMember = team.BrochureUrl,
                                  BrochureDate = team.BrochureDate,

                                  AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                  AwardNominationUrlForMember = team.AwardNominationUrl,
                                  AwardNominationDate = team.AwardNominationDate,

                                  OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                  OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                  OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                  WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                  WrittenReportUrlForMember = team.WrittenReportUrl,
                                  WrittenReportDate = team.WrittenReportDate,

                                  Shipping = "",
                                  ResumeUrl = team.ResumeUrl,
                                  Date = team.Date,
                                  EmailConfirmed = team.EmailConfirmed,
                                  LockoutEnabled = team.LockoutEnabled,
                                  PayStatus = team.PayStatus,
                                  PaidByCheque = team.PaidByCheque,
                                  PaidByChequePercent = team.PaidByChequePercent,
                                  SuppressScoring = team.SuppressScoring,
                                  Deactivate = team.Deactivate,
                                  Preliminary = team.Preliminary,
                                  Scored = team.Scored,
                                  OpenTaskTestPlan = team.OpenTaskTestPlan,
                              }).ToList();
            var teamIds = vmTeamList.Select(t => t.Id).ToArray();

            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);

            foreach (var team in vmTeamList)
            {
                team.SubmissionRuleList = (from s in submissionRuleList
                                           where s.TeamId == team.Id
                                           select new VmSubmissionRule
                                           {
                                               Id = s.Id,
                                               TeamId = s.TeamId,
                                               UploadDate = s.UploadDate,
                                               DueDate = s.DueDate,
                                               DueDateOrder = DateTime.Parse(s.DueDate),
                                               Description = s.Description,
                                               Name = s.Name,
                                               ShowLate = s.ShowLate,
                                               ShowReport = s.ShowReport,
                                               TeamName = s.TeamName,
                                               PayStatus = s.PayStatus,
                                               RegistrationStatus = s.RegistrationStatus,
                                               TeamPayStatus = s.TeamPayStatus,
                                               TeamRegistrationStatus = s.TeamRegistrationStatus,
                                               SubmissionRuleUrl = s.SubmissionRuleUrl,

                                           }).OrderBy(p => p.DueDateOrder).ToList();

            }
            return vmTeamList;
        }

        public List<VmTeam> GetAdvisorTeams(string advisorId, string name = "")
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeams(advisorId, name);
            var vmTeamList = (from team in teamList
                              select new VmTeam
                              {
                                  Id = team.Id,
                                  Payment = team.Payment,
                                  TaskId = team.TaskId,
                                  ProjectName = team.ProjectName,
                                  Name = team.Name,
                                  TeamNumber = team.TeamNumber,
                                  Task = team.Task,
                                  TeamState = team.TeamState.Value,
                                  TeamImageUrl = team.TeamImageUrl,
                                  TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                  RegistrationStatus = team.RegistrationStatus,
                                  Survey = team.Survey,
                                  MemberName = team.MemberName,
                                  PhoneNumber = team.PhoneNumber,
                                  WorkPhoneNumber = team.WorkPhoneNumber ?? "",
                                  Identifier = team.Identifier,
                                  Sex = team.Sex,
                                  BirthDate = team.BirthDate,
                                  UserName = team.UserName,
                                  Email = team.Email,
                                  RegisterDate = team.RegisterDate,
                                  RoleName = team.RoleName,
                                  RoleId = team.RoleId,
                                  UserDefiner = team.UserDefiner,
                                  LastSignIn = team.LastSignIn,
                                  UniversityId = team.UniversityId,
                                  University = team.University,
                                  JacketSizeId = team.JacketSizeId,
                                  JacketSize = team.JacketSize ?? "",
                                  DietTypeId = team.DietTypeId,
                                  DietType = team.DietType ?? "",
                                  StreetLine1 = team.StreetLine1,
                                  StreetLine2 = team.StreetLine2,
                                  City = team.City,
                                  State = team.State,
                                  ZipCode = team.ZipCode,
                                  EmgyPersonRelationship = team.EmgyPersonRelationship,
                                  EmgyPersonPhoneNumber = team.EmgyPersonPhoneNumber,
                                  EmgyPersonLastName = team.EmgyPersonLastName,
                                  EmgyPersonFirstName = team.EmgyPersonFirstName,
                                  ShortBio = team.ShortBio,
                                  T_Shirt_Size = team.T_Shirt_Size,
                                  ProfilePictureUrl = team.ProfilePictureUrl,
                                  UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                  LabResultUrl = team.LabResultUrl,

                                  PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                  PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                  PreliminaryReportDate = team.PreliminaryReportDate,

                                  FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                  FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                  FlashTalkReportDate = team.FlashTalkReportDate,

                                  BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                  BrochureUrlForMember = team.BrochureUrl,
                                  BrochureDate = team.BrochureDate,

                                  AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                  AwardNominationUrlForMember = team.AwardNominationUrl,
                                  AwardNominationDate = team.AwardNominationDate,

                                  OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                  OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                  OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                  WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                  WrittenReportUrlForMember = team.WrittenReportUrl,
                                  WrittenReportDate = team.WrittenReportDate,

                                  Shipping = "",//team.Shipping
                                  ResumeUrl = team.ResumeUrl,
                                  Date = team.Date,
                                  EmailConfirmed = team.EmailConfirmed,
                                  LockoutEnabled = team.LockoutEnabled,
                                  PayStatus = team.PayStatus,
                                  PaidByCheque = team.PaidByCheque,
                                  PaidByChequePercent = team.PaidByChequePercent,
                                  SuppressScoring = team.SuppressScoring,
                                  Deactivate = team.Deactivate,
                                  Preliminary = team.Preliminary,
                                  Scored = team.Scored,
                                  OpenTaskTestPlan = team.OpenTaskTestPlan,
                              }).ToList();


            var teamIds = vmTeamList.Select(t => t.Id).ToArray();

            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);

            foreach (var team in vmTeamList)
            {
                team.SubmissionRuleList = (from s in submissionRuleList
                                           where s.TeamId == team.Id
                                           select new VmSubmissionRule
                                           {
                                               Id = s.Id,
                                               TeamId = s.TeamId,
                                               UploadDate = s.UploadDate,
                                               DueDate = s.DueDate,
                                               DueDateOrder = DateTime.Parse(s.DueDate),
                                               Description = s.Description,
                                               Name = s.Name,
                                               ShowLate = s.ShowLate,
                                               ShowReport = s.ShowReport,
                                               TeamName = s.TeamName,
                                               PayStatus = s.PayStatus,
                                               RegistrationStatus = s.RegistrationStatus,
                                               TeamPayStatus = s.TeamPayStatus,
                                               TeamRegistrationStatus = s.TeamRegistrationStatus,
                                               SubmissionRuleUrl = s.SubmissionRuleUrl,

                                           }).OrderBy(p => p.DueDateOrder).ToList();

            }


            return vmTeamList;
        }
        public List<VmTeam> GetJudgeTeams(string judgeId)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeams(judgeId);

            var vmTeamList = (from team in teamList
                              select new VmTeam
                              {
                                  Id = team.TeamId,
                                  TaskId = team.TaskId,
                                  ProjectName = team.ProjectName,
                                  Name = team.TeamName,
                                  MemberName = team.MemberName,
                                  Task = team.TaskName,
                                  TeamState = team.TeamState.Value,
                                  TeamImageUrl = team.TeamImageUrl,
                                  TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                  RegistrationStatus = team.RegistrationStatus,
                                  Survey = team.Survey,
                                  University = team.University,
                                  UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                  Date = team.Date,
                                  TaskImageUrl = team.TaskImageUrl,

                                  PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                  PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                  PreliminaryReportDate = team.PreliminaryReportDate,

                                  FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                  FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                  FlashTalkReportDate = team.FlashTalkReportDate,

                                  BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                  BrochureUrlForMember = team.BrochureUrl,
                                  BrochureDate = team.BrochureDate,

                                  AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                  AwardNominationUrlForMember = team.AwardNominationUrl,
                                  AwardNominationDate = team.AwardNominationDate,

                                  OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                  OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                  OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                  WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                  WrittenReportUrlForMember = team.WrittenReportUrl,
                                  WrittenReportDate = team.WrittenReportDate,

                                  Preliminary = team.Preliminary,
                                  Scored = team.Scored,
                                  OpenTaskTestPlan = team.OpenTaskTestPlan,

                                  PayStatus = team.PayStatus,
                                  PaidByCheque = team.PaidByCheque,
                                  PaidByChequePercent = team.PaidByChequePercent,
                                  SuppressScoring = team.SuppressScoring,
                              }).ToList();


            var teamIds = vmTeamList.Select(t => t.Id).ToArray();

            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);

            foreach (var team in vmTeamList)
            {
                team.SubmissionRuleList = (from s in submissionRuleList
                                           where s.TeamId == team.Id
                                           select new VmSubmissionRule
                                           {
                                               Id = s.Id,
                                               TeamId = s.TeamId,
                                               UploadDate = s.UploadDate,
                                               DueDate = s.DueDate,
                                               DueDateOrder = DateTime.Parse(s.DueDate),
                                               Description = s.Description,
                                               Name = s.Name,
                                               ShowLate = s.ShowLate,
                                               ShowReport = s.ShowReport,
                                               TeamName = s.TeamName,
                                               PayStatus = s.PayStatus,
                                               RegistrationStatus = s.RegistrationStatus,
                                               TeamPayStatus = s.TeamPayStatus,
                                               TeamRegistrationStatus = s.TeamRegistrationStatus,
                                               SubmissionRuleUrl = s.SubmissionRuleUrl,
                                               SendEmail = s.SendEmail,

                                           }).OrderBy(p => p.DueDateOrder).ToList();

            }
            return vmTeamList;
        }
        public IEnumerable<VmTeam> GetMemberUserTeamsByTaskConfirm(string judgeId, bool confirm)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeamsByTaskConfirm(judgeId, confirm);

            var vmTeamList = from team in teamList
                             select new VmTeam
                             {
                                 Id = team.TeamId,
                                 TaskId = team.TaskId,
                                 ProjectName = team.ProjectName,
                                 Name = team.TeamName,
                                 MemberName = team.MemberName,
                                 Task = team.TaskName,
                                 TeamState = team.TeamState.Value,
                                 TeamImageUrl = team.TeamImageUrl,
                                 TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                 RegistrationStatus = team.RegistrationStatus,
                                 Survey = team.Survey,
                                 University = team.University,
                                 UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                 Date = team.Date,
                                 TaskImageUrl = team.TaskImageUrl,

                                 PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                 PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                 PreliminaryReportDate = team.PreliminaryReportDate,

                                 FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                 FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                 FlashTalkReportDate = team.FlashTalkReportDate,

                                 BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                 BrochureUrlForMember = team.BrochureUrl,
                                 BrochureDate = team.BrochureDate,

                                 AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                 AwardNominationUrlForMember = team.AwardNominationUrl,
                                 AwardNominationDate = team.AwardNominationDate,

                                 OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                 OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                 OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                 WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                 WrittenReportUrlForMember = team.WrittenReportUrl,
                                 WrittenReportDate = team.WrittenReportDate,

                                 Preliminary = team.Preliminary,
                                 Scored = team.Scored,
                                 OpenTaskTestPlan = team.OpenTaskTestPlan,

                                 PayStatus = team.PayStatus,
                                 PaidByCheque = team.PaidByCheque,
                                 PaidByChequePercent = team.PaidByChequePercent,
                                 SuppressScoring = team.SuppressScoring,
                             };

            return vmTeamList;
        }
        public List<VmTeam> GetJudgeTeams(string judgeId, string name = "")
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeams(judgeId, name);
            var vmTeamList = (from team in teamList
                              select new VmTeam
                              {
                                  Id = team.TeamId,
                                  TaskId = team.TaskId,
                                  ProjectName = team.ProjectName,
                                  Name = team.TeamName,
                                  Task = team.TaskName,
                                  TeamState = team.TeamState.Value,
                                  TeamImageUrl = team.TeamImageUrl,
                                  TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                  RegistrationStatus = team.RegistrationStatus,
                                  Survey = team.Survey,
                                  University = team.University,
                                  UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                  Date = team.Date,
                                  TaskImageUrl = team.TaskImageUrl,

                                  PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                  PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                  PreliminaryReportDate = team.PreliminaryReportDate,

                                  FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                  FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                  FlashTalkReportDate = team.FlashTalkReportDate,

                                  BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                  BrochureUrlForMember = team.BrochureUrl,
                                  BrochureDate = team.BrochureDate,

                                  AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                  AwardNominationUrlForMember = team.AwardNominationUrl,
                                  AwardNominationDate = team.AwardNominationDate,

                                  OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                  OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                  OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                  WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                  WrittenReportUrlForMember = team.WrittenReportUrl,
                                  WrittenReportDate = team.WrittenReportDate,

                                  Preliminary = team.Preliminary,
                                  Scored = team.Scored,
                                  OpenTaskTestPlan = team.OpenTaskTestPlan,

                                  PayStatus = team.PayStatus,
                                  PaidByCheque = team.PaidByCheque,
                                  PaidByChequePercent = team.PaidByChequePercent,
                              }).ToList();
            var teamIds = vmTeamList.Select(t => t.Id).ToArray();

            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);

            foreach (var team in vmTeamList)
            {
                team.SubmissionRuleList = (from s in submissionRuleList
                                           where s.TeamId == team.Id
                                           select new VmSubmissionRule
                                           {
                                               Id = s.Id,
                                               TeamId = s.TeamId,
                                               UploadDate = s.UploadDate,
                                               DueDate = s.DueDate,
                                               DueDateOrder = DateTime.Parse(s.DueDate),
                                               Description = s.Description,
                                               Name = s.Name,
                                               ShowLate = s.ShowLate,
                                               ShowReport = s.ShowReport,
                                               TeamName = s.TeamName,
                                               PayStatus = s.PayStatus,
                                               RegistrationStatus = s.RegistrationStatus,
                                               TeamPayStatus = s.TeamPayStatus,
                                               TeamRegistrationStatus = s.TeamRegistrationStatus,
                                               SubmissionRuleUrl = s.SubmissionRuleUrl,

                                           }).OrderBy(p => p.DueDateOrder).ToList();

            }
            return vmTeamList;
        }
        public IEnumerable<VmTeam> GetMemberUserTeamsByTaskConfirm(string judgeId, bool confirm, string name = "")
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            var teamList = teamRepository.GetMemberUserTeamsByTaskConfirm(judgeId, confirm, name);
            var vmTeamList = from team in teamList
                             select new VmTeam
                             {
                                 Id = team.TeamId,
                                 TaskId = team.TaskId,
                                 ProjectName = team.ProjectName,
                                 Name = team.TeamName,
                                 Task = team.TaskName,
                                 TeamState = team.TeamState.Value,
                                 TeamImageUrl = team.TeamImageUrl,
                                 TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                 RegistrationStatus = team.RegistrationStatus,
                                 Survey = team.Survey,
                                 University = team.University,
                                 UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                 Date = team.Date,
                                 TaskImageUrl = team.TaskImageUrl,

                                 PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                 PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                 PreliminaryReportDate = team.PreliminaryReportDate,

                                 FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                 FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                 FlashTalkReportDate = team.FlashTalkReportDate,

                                 BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                 BrochureUrlForMember = team.BrochureUrl,
                                 BrochureDate = team.BrochureDate,

                                 AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                 AwardNominationUrlForMember = team.AwardNominationUrl,
                                 AwardNominationDate = team.AwardNominationDate,

                                 OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                 OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                 OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                 WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                 WrittenReportUrlForMember = team.WrittenReportUrl,
                                 WrittenReportDate = team.WrittenReportDate,

                                 Preliminary = team.Preliminary,
                                 Scored = team.Scored,
                                 OpenTaskTestPlan = team.OpenTaskTestPlan,

                                 PayStatus = team.PayStatus,
                                 PaidByCheque = team.PaidByCheque,
                                 PaidByChequePercent = team.PaidByChequePercent,
                             };

            return vmTeamList;
        }
        public IEnumerable<VmTeam> GetTeamList(string name = "")
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();

            var teamList = teamRepository.GetTeams(name);
            var vmTeamList = from team in teamList
                             select new VmTeam
                             {
                                 Id = team.Id,
                                 Payment = team.Payment,
                                 TaskId = team.TaskId,
                                 ProjectName = team.ProjectName,
                                 Name = team.Name,
                                 TeamNumber = team.TeamNumber,
                                 Task = team.Task,
                                 TeamState = team.TeamState.Value,
                                 TeamImageUrl = team.TeamImageUrl,
                                 TeamStateDescription = GetTeamStateDescription(team.TeamState),
                                 RegistrationStatus = team.RegistrationStatus,
                                 Survey = team.Survey,
                                 MemberName = team.MemberName,
                                 PhoneNumber = team.PhoneNumber,
                                 WorkPhoneNumber = team.WorkPhoneNumber ?? "",
                                 Identifier = team.Identifier,
                                 Sex = team.Sex,
                                 BirthDate = team.BirthDate,
                                 UserName = team.UserName,
                                 Email = team.Email,
                                 RegisterDate = team.RegisterDate,
                                 RoleName = team.RoleName,
                                 RoleId = team.RoleId,
                                 UserDefiner = team.UserDefiner,
                                 LastSignIn = team.LastSignIn,
                                 UniversityId = team.UniversityId,
                                 University = team.University,
                                 JacketSizeId = team.JacketSizeId,
                                 JacketSize = team.JacketSize ?? "",
                                 DietTypeId = team.DietTypeId,
                                 DietType = team.DietType ?? "",
                                 StreetLine1 = team.StreetLine1,
                                 StreetLine2 = team.StreetLine2,
                                 City = team.City,
                                 State = team.State,
                                 ZipCode = team.ZipCode,
                                 EmgyPersonRelationship = team.EmgyPersonRelationship,
                                 EmgyPersonPhoneNumber = team.EmgyPersonPhoneNumber,
                                 EmgyPersonLastName = team.EmgyPersonLastName,
                                 EmgyPersonFirstName = team.EmgyPersonFirstName,
                                 ShortBio = team.ShortBio,
                                 T_Shirt_Size = team.T_Shirt_Size,
                                 ProfilePictureUrl = team.ProfilePictureUrl,
                                 UniversityPictureUrl = team.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                 LabResultUrl = team.LabResultUrl,

                                 PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                 PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                 PreliminaryReportDate = team.PreliminaryReportDate,

                                 FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                 FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                 FlashTalkReportDate = team.FlashTalkReportDate,

                                 BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                 BrochureUrlForMember = team.BrochureUrl,
                                 BrochureDate = team.BrochureDate,

                                 AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                 AwardNominationUrlForMember = team.AwardNominationUrl,
                                 AwardNominationDate = team.AwardNominationDate,

                                 OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                 OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                 OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                 WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                 WrittenReportUrlForMember = team.WrittenReportUrl,
                                 WrittenReportDate = team.WrittenReportDate,

                                 Preliminary = team.Preliminary,
                                 Scored = team.Scored,
                                 OpenTaskTestPlan = team.OpenTaskTestPlan,

                                 Shipping = "",
                                 ResumeUrl = team.ResumeUrl,
                                 Date = team.Date,
                                 EmailConfirmed = team.EmailConfirmed,
                                 LockoutEnabled = team.LockoutEnabled,
                                 PayStatus = team.PayStatus,
                                 PaidByCheque = team.PaidByCheque,
                                 PaidByChequePercent = team.PaidByChequePercent,
                                 SuppressScoring = team.SuppressScoring,
                                 Deactivate = team.Deactivate,
                             };

            return vmTeamList;
        }
        public IEnumerable<VmTeam> GetTeamList()
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var teamList = teamRepository.GetAllTeam();
            var vmTeamList = from team in teamList
                             select new VmTeam
                             {
                                 Id = team.Id,
                                 Payment = team.Payment,
                                 TaskId = team.TaskId,
                                 ProjectName = team.ProjectName,
                                 Name = team.Name,
                                 TeamNumber = team.TeamNumber,
                                 LabResultUrl = team.LabResultUrl,

                                 PreliminaryReportUrl = team.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png",
                                 PreliminaryReportUrlForMember = team.PreliminaryReportUrl,
                                 PreliminaryReportDate = team.PreliminaryReportDate,

                                 FlashTalkReportUrl = team.FlashTalkReportUrl ?? "?CT=Stylish_not_ok.png",
                                 FlashTalkReportUrlForMember = team.FlashTalkReportUrl,
                                 FlashTalkReportDate = team.FlashTalkReportDate,

                                 BrochureUrl = team.BrochureUrl ?? "?CT=Stylish_not_ok.png",
                                 BrochureUrlForMember = team.BrochureUrl,
                                 BrochureDate = team.BrochureDate,

                                 AwardNominationUrl = team.AwardNominationUrl ?? "?CT=Stylish_not_ok.png",
                                 AwardNominationUrlForMember = team.AwardNominationUrl,
                                 AwardNominationDate = team.AwardNominationDate,

                                 OpenTaskTestPlanUrl = team.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png",
                                 OpenTaskTestPlanUrlForMember = team.OpenTaskTestPlanUrl,
                                 OpenTaskTestPlanDate = team.OpenTaskTestPlanDate,

                                 WrittenReportUrl = team.WrittenReportUrl ?? "?CT=Stylish_not_ok.png",
                                 WrittenReportUrlForMember = team.WrittenReportUrl,
                                 WrittenReportDate = team.WrittenReportDate,

                                 Shipping = "",
                                 Date = team.Date,
                                 PayStatus = team.PayStatus,
                                 PaidByCheque = team.PaidByCheque,
                                 PaidByChequePercent = team.PaidByChequePercent,
                                 SuppressScoring = team.SuppressScoring,
                                 Deactivate = team.Deactivate,
                                 AdminSuppressScoring = team.AdminSuppressScoring ?? false,
                                 ViewFinalScore = team.ViewFinalScore,
                             };

            return vmTeamList;
        }
        public IEnumerable<int> GetTeamIdsByTask(int taskId)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var teamList = teamRepository.GetTeamsByTask(taskId);

            var teamIds = from team in teamList select team.Id;

            return teamIds.ToArray();
        }

        public int GetLeaderTeam(string leaderId)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            return viewTeamMemberRepository.GetTeamMember(leaderId).TeamId;
        }
        public int GetCoAdvisorTeam(string coAdvisorId)
        {
            var teamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            return viewTeamMemberRepository.GetTeamMember(coAdvisorId).TeamId;
        }

        public int CreateTeam(VmTeam vmTeam)
        {
            var result = -1;
            try
            {
                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

                var currentDate = DateTime.Now;

                var teamMembers = new List<TeamMember>()
                {
                    new TeamMember
                    {
                         MemberUserId = vmTeam.CurrentUserId,
                         RegistrationStatus = true,
                    }
                };

                var teamNumber = teamRepository.FindFirstEmptyTeamNumber();

                if (teamNumber == 100)
                {
                    return -1;
                }

                var universityWords = vmTeam.University.Split(' ');
                var abbreviation = "";

                var person = new BLPerson().GetPersonByUserId(vmTeam.CurrentUserId);

                abbreviation = person.Abbreviation;

                //var count = 0;
                //for (int i = 0; i < universityWords.Length; i++)
                //{
                //    if (universityWords[i].Length > 2)
                //    {
                //        abbreviation += universityWords[i][0];
                //        count++;
                //    }

                //}

                var teamName = teamNumber.ToString("d2") + "-" + abbreviation + "-" + vmTeam.Task.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];

                var newTeam = new Team
                {
                    Id = vmTeam.Id,
                    TaskId = vmTeam.TaskId,
                    Date = DateTime.Now,
                    Name = teamName,
                    State = 0,
                    PayStatus = false,
                    PaidByCheque = false,
                    ImageUrl = vmTeam.TeamImageUrl,
                    TeamMembers = teamMembers,
                    TeamNumber = teamNumber,
                    Deactivate = false,
                    AdminSuppressScoring = false,
                    ProjectName = vmTeam.ProjectName,

                };


                teamRepository.CreateTeam(newTeam);

                UnitOfWork.Commit();

                result = newTeam.Id;


            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public void UpdateTeamName(int teamId, string name)
        {
            var TeamRepository = UnitOfWork.GetRepository<TeamRepository>();

            TeamRepository.UpdateTeamName(teamId, name);
            UnitOfWork.Commit();
        }
        public bool UpdateProjectName(int teamId, string projectName)
        {
            var TeamRepository = UnitOfWork.GetRepository<TeamRepository>();

            TeamRepository.UpdateProjectName(teamId, projectName);
            return UnitOfWork.Commit();
        }
        public bool TransferTeamToAnotherAdvisor(int teamId, string advisorId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();

            teamMemberRepository.TransferTeamToAnotherAdvisor(teamId, advisorId);
            return UnitOfWork.Commit();
        }
        public bool RemoveTheLate(int teamId, int submissionRuleId)
        {
            var teamSubmissionRuleRepository = UnitOfWork.GetRepository<TeamSubmissionRuleRepository>();

            teamSubmissionRuleRepository.RemoveTheLate(teamId, submissionRuleId);
            return UnitOfWork.Commit();
        }
        public bool UploadOpenTaskTestPlan(int teamId, string openTaskTestPlanUrl)
        {
            try
            {

                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
                teamRepository.UpdateOpenTaskTestPlan(teamId, openTaskTestPlanUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UploadSubmissionRule(int teamId, int submissionRuleId, string submissionRuleUrl)
        {
            try
            {
                var teamRepository = UnitOfWork.GetRepository<TeamSubmissionRuleRepository>();

                if (teamRepository.UpdateTeamSubmissionRule(teamId, submissionRuleId, submissionRuleUrl) == false)
                {
                    teamRepository.CreateTeamSubmissionRule(
                        new TeamSubmissionRule
                        {
                            TeamId = teamId,
                            SubmissionRuleUrl = submissionRuleUrl,
                            SubmissionRuleId = submissionRuleId,
                            UploadDate = DateTime.Now,
                        }
                        );
                }

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UploadWrittenReport(int teamId, string writtenReportUrl)
        {
            try
            {

                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
                teamRepository.UpdateWrittenReport(teamId, writtenReportUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UploadPreliminaryReport(int teamId, string preliminaryReportUrl)
        {
            try
            {

                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
                teamRepository.UpdatePreliminaryReport(teamId, preliminaryReportUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UploadFlashTalkReport(int teamId, string FlashTalkReportUrl)
        {
            try
            {

                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
                teamRepository.UpdateFlashTalkReport(teamId, FlashTalkReportUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UploadAwardNomination(int teamId, string AwardNominationUrl)
        {
            try
            {

                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
                teamRepository.UpdateAwardNomination(teamId, AwardNominationUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UploadBrochure(int teamId, string BrochureUrl)
        {
            try
            {

                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
                teamRepository.UpdateBrochure(teamId, BrochureUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateTeam(VmTeam vmTeam)
        {
            try
            {
                var TeamRepository = UnitOfWork.GetRepository<TeamRepository>();


                var universityWords = vmTeam.University.Split(' ');
                var abbreviation = "";

                var person = new BLPerson().GetPersonByUserId(vmTeam.CurrentUserId);

                abbreviation = person.Abbreviation;

                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();


                var oldTeamName = vmTeam.Name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0] + "-" + abbreviation + "-";

                var teamName = oldTeamName + vmTeam.Task.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];

                var updateableTeam = new Team
                {
                    Id = vmTeam.Id,
                    TaskId = vmTeam.TaskId,
                    Name = teamName,
                    State = vmTeam.TeamState,
                    ImageUrl = vmTeam.TeamImageUrl,
                    Deactivate = vmTeam.Deactivate,
                    ProjectName = vmTeam.ProjectName,
                };

                TeamRepository.UpdateTeam(updateableTeam);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

        public bool UpdatePayStatus(int teamId, bool payStatus)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.UpdatePayStatus(teamId, payStatus);
            return UnitOfWork.Commit();
        }
        public bool UpdatePaidByCheque(int teamId, bool paidByCheque)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.UpdatePaidByCheque(teamId, paidByCheque);
            return UnitOfWork.Commit();
        }
        public bool UpdateTeamRegisterForFlashTalk(int teamId, bool teamRegisterForFlashTalk)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.UpdateTeamRegisterForFlashTalk(teamId, teamRegisterForFlashTalk);
            return UnitOfWork.Commit();
        }

        public bool ReversePaidByCheque(int teamId)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var lastStatus = teamRepository.ReversePaidByCheque(teamId);
            UnitOfWork.Commit();
            return lastStatus;
        }


        public bool UpdateteamPayment(int teamId, decimal payment)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            if (payment == 0)
            {
                teamRepository.UpdateTeamPayStatus(teamId, true);
            }

            teamRepository.UpdateteamPayment(teamId, payment);
            var lastStatus = UnitOfWork.Commit();
            return lastStatus;
        }
        public bool UpdateTeamActivation(int teamId, bool deactivation)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.UpdateTeamActivation(teamId, deactivation);
            return UnitOfWork.Commit();
        }
        public bool ReverseTeamActivation(int teamId)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var lastStatus = teamRepository.ReverseTeamActivation(teamId);
            UnitOfWork.Commit();
            return lastStatus;
        }

        public void SetAllowSuppressScoringAllTeams(bool deactive)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.SetAllowSuppressScoringAllTeams(deactive);
            UnitOfWork.Commit();
        }
        public void SetAllowViewScoreAllTeams(bool deactive)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.SetAllowViewScoringAllTeams(deactive);
            UnitOfWork.Commit();
        }
        public void SetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring()
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.SetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring();
            UnitOfWork.Commit();
        }
        public void UnSetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring()
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            teamRepository.UnSetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring();
            UnitOfWork.Commit();
        }
        public bool UpdateTeamPayStatus(int teamId)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var lastStatus = teamRepository.UpdateTeamPayStatus(teamId);
            UnitOfWork.Commit();
            return lastStatus;
        }
        public bool UpdateTeamScored(int teamId)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var lastStatus = teamRepository.UpdateTeamScored(teamId);
            UnitOfWork.Commit();
            return lastStatus;
        }
        public bool ReverseTeamSuppressScoring(int teamId)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var lastStatus = teamRepository.ReverseTeamSuppressScoring(teamId);
            UnitOfWork.Commit();
            return lastStatus;
        }
        public bool UpdatePayStatus(List<VmTeamSelection> teamSelectionList, bool payStatus)
        {
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var teamIds = teamSelectionList.Where(t => t.Checked == true).Select(t => t.TeamId).ToArray();

            teamRepository.UpdatePayStatus(teamIds, payStatus);

            return UnitOfWork.Commit();
        }
        public bool DeleteTeam(int teamId)
        {
            try
            {
                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

                if (teamRepository.DeleteTeam(teamId) == true)
                {
                    return UnitOfWork.Commit();
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        public bool UpdateTeamState(int teamId, TeamState teamState)
        {
            try
            {
                var TeamRepository = UnitOfWork.GetRepository<TeamRepository>();

                TeamRepository.UpdateTeamState(teamId, teamState);

                UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public IEnumerable<VmTeamFullInfo> GetTeamFullInfoByFilter(VmTeamFullInfo filterItem)
        {
            var viewTeamFullInfoRepository = UnitOfWork.GetRepository<ViewTeamFullInfoRepository>();

            var viewFilterItem = new VmTeamFullInfo
            {
                Name = filterItem.Name,
                Leader = filterItem.Leader,
                Advisor = filterItem.Advisor,
                TaskName = filterItem.TaskName,
                Judges = filterItem.Judges,
                PayStatus = filterItem.PayStatus,
                Deactivate = filterItem.Deactivate,
                RegistrationStatus = filterItem.RegistrationStatus,
                //SafetyStatus = filterItem.SafetyStatus,
                //SafetyStatus = filterItem.WrittenReportStatus,
                //Shipping = filterItem.Shipping,
                //Survey = filterItem.Survey,
                PreliminaryReportStatus = filterItem.PreliminaryReportStatus,
                FlashTalkReportStatus = filterItem.FlashTalkReportStatus,
                OpenTaskTestPlanStatus = filterItem.OpenTaskTestPlanStatus,
            };

            var viewteamFullInfoList = viewTeamFullInfoRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmTeamFullInfoList = (from teamFullInfo in viewteamFullInfoList
                                      select new VmTeamFullInfo
                                      {
                                          Id = teamFullInfo.Id,
                                          Payment = teamFullInfo.Payment,
                                          TaskId = teamFullInfo.TaskId,
                                          ProjectName = teamFullInfo.ProjectName,
                                          Name = teamFullInfo.Name,
                                          TaskName = teamFullInfo.TaskName,
                                          TeamImageUrl = teamFullInfo.TeamImageUrl,
                                          TeamState = teamFullInfo.TeamState.Value,
                                          TeamStateDescription = teamFullInfo.TeamState.ToString(),
                                          SafetyStatus = /*teamFullInfo.SafetyStatus*/ false,
                                          PayStatus = teamFullInfo.PayStatus,
                                          PaidByCheque = teamFullInfo.PaidByCheque,
                                          PaidByChequePercent = teamFullInfo.PaidByChequePercent,
                                          PayStatusDescription = (teamFullInfo.PayStatus == true) ? PayStatus.Payed.ToString() : PayStatus.NotPayed.ToString(),
                                          RegistrationStatus = teamFullInfo.RegistrationStatus,
                                          Survey = teamFullInfo.Survey,
                                          Advisor = teamFullInfo.Advisor,
                                          Leader = teamFullInfo.Leader,
                                          Judges = teamFullInfo.Judges,
                                          PhoneNumber = teamFullInfo.PhoneNumber,
                                          WorkPhoneNumber = teamFullInfo.WorkPhoneNumber ?? "",
                                          Identifier = teamFullInfo.Identifier,
                                          Sex = teamFullInfo.Sex,
                                          BirthDate = teamFullInfo.BirthDate,
                                          UserName = teamFullInfo.UserName,
                                          Email = teamFullInfo.Email,
                                          RegisterDate = teamFullInfo.RegisterDate,
                                          RoleName = teamFullInfo.RoleName,
                                          RoleId = teamFullInfo.RoleId,
                                          UserDefiner = teamFullInfo.UserDefiner,
                                          LastSignIn = teamFullInfo.LastSignIn,
                                          UniversityId = teamFullInfo.UniversityId,
                                          University = teamFullInfo.University,
                                          JacketSizeId = teamFullInfo.JacketSizeId,
                                          JacketSize = teamFullInfo.JacketSize ?? "",
                                          DietTypeId = teamFullInfo.DietTypeId,
                                          DietType = teamFullInfo.DietType ?? "",
                                          StreetLine1 = teamFullInfo.StreetLine1,
                                          StreetLine2 = teamFullInfo.StreetLine2,
                                          City = teamFullInfo.City,
                                          State = teamFullInfo.State,
                                          ZipCode = teamFullInfo.ZipCode,
                                          EmgyPersonRelationship = teamFullInfo.EmgyPersonRelationship,
                                          EmgyPersonPhoneNumber = teamFullInfo.EmgyPersonPhoneNumber,
                                          EmgyPersonLastName = teamFullInfo.EmgyPersonLastName,
                                          EmgyPersonFirstName = teamFullInfo.EmgyPersonFirstName,
                                          ShortBio = teamFullInfo.ShortBio,
                                          T_Shirt_Size = teamFullInfo.T_Shirt_Size,
                                          ProfilePictureUrl = teamFullInfo.ProfilePictureUrl,
                                          UniversityPictureUrl = teamFullInfo.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                          LabResultUrl = teamFullInfo.LabResultUrl,

                                          PreliminaryReportStatus = (teamFullInfo.Preliminary == false || teamFullInfo.Preliminary == null) ? "" :
                                          !string.IsNullOrWhiteSpace(teamFullInfo.PreliminaryReportUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                                          PreliminaryReportUrlForMember = teamFullInfo.PreliminaryReportUrl,
                                          PreliminaryReportUrl = teamFullInfo.PreliminaryReportUrl,
                                          PreliminaryReportDate = teamFullInfo.PreliminaryReportDate,

                                          OpenTaskTestPlanStatus = (teamFullInfo.OpenTaskTestPlan == false || teamFullInfo.OpenTaskTestPlan == null) ? "" :
                                          !string.IsNullOrWhiteSpace(teamFullInfo.OpenTaskTestPlanUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                                          OpenTaskTestPlanUrlForMember = teamFullInfo.OpenTaskTestPlanUrl,
                                          OpenTaskTestPlanUrl = teamFullInfo.OpenTaskTestPlanUrl,
                                          OpenTaskTestPlanDate = teamFullInfo.OpenTaskTestPlanDate,

                                          WrittenReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.WrittenReportUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          WrittenReportUrlForMember = teamFullInfo.WrittenReportUrl,
                                          WrittenReportDate = teamFullInfo.WrittenReportDate,
                                          WrittenReportDateString = (teamFullInfo.WrittenReportDate != null) ? teamFullInfo.WrittenReportDate.ToString() : null,


                                          FlashTalkReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.FlashTalkReportUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          FlashTalkReportUrlForMember = teamFullInfo.FlashTalkReportUrl,
                                          FlashTalkReportDate = teamFullInfo.FlashTalkReportDate,

                                          BrochureUrl = string.IsNullOrWhiteSpace(teamFullInfo.BrochureUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          BrochureUrlForMember = teamFullInfo.BrochureUrl,
                                          BrochureDate = teamFullInfo.BrochureDate,
                                          BrochureDateString = (teamFullInfo.BrochureDate != null) ? teamFullInfo.BrochureDate.ToString() : null,

                                          AwardNominationUrl = string.IsNullOrWhiteSpace(teamFullInfo.AwardNominationUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          AwardNominationUrlForMember = teamFullInfo.AwardNominationUrl,
                                          AwardNominationDate = teamFullInfo.AwardNominationDate,


                                          Shipping = "",
                                          ResumeUrl = teamFullInfo.ResumeUrl,
                                          Date = teamFullInfo.Date,
                                          EmailConfirmed = teamFullInfo.EmailConfirmed,
                                          LockoutEnabled = teamFullInfo.LockoutEnabled,
                                          Deactivate = teamFullInfo.Deactivate,
                                          SubmitStatus = teamFullInfo.SubmitStatus,
                                          Status = teamFullInfo.Status,
                                          Approved = teamFullInfo.Approved,

                                          PreliminaryReport = teamFullInfo.Preliminary,
                                          Scored = teamFullInfo.Scored,
                                          OpenTaskTestPlan = teamFullInfo.OpenTaskTestPlan,
                                          MemberUserId = teamFullInfo.MemberUserId,

                                      }).ToList();

            var blInvoice = new BLInvoice();

            var teamIds = vmTeamFullInfoList.Select(t => t.Id).ToArray();

            var invoiceDetails = blInvoice.GetInvoiceDetailByTeams(teamIds);

            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);
            var downloadableSubmissionRuleList = blSubmissionRule.GetDownloadTeamsSubmissionRuleByTeamId(teamIds);

            foreach (var team in vmTeamFullInfoList)
            {

                #region Generate downloaded submission links
                var downloadedSubmissionList = (from s in downloadableSubmissionRuleList
                                                where s.TeamId == team.Id
                                                select s).ToArray();
                var link = "";

                foreach (var item in downloadedSubmissionList)
                {
                    link += "<a href='" + item.SubmissionRuleUrl + "'>" + item.Name + "</a>,";
                }

                if (link.Length > 0)
                {
                    link = link.Substring(0, link.Length - 1);
                    team.Submissions = link;
                }

                #endregion Generate downloaded submission links

                var writtenReportRule = (from s in submissionRuleList
                                         where s.Id == 3 && s.TeamId == team.Id
                                         select s).FirstOrDefault();

                if (writtenReportRule != null)
                {
                    team.WrittenReportUrl = string.IsNullOrWhiteSpace(writtenReportRule.SubmissionRuleUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png";
                    team.WrittenReportUrlForMember = writtenReportRule.SubmissionRuleUrl;
                    team.WrittenReportDate = writtenReportRule.UploadDate;
                    team.WrittenReportDateString = writtenReportRule.UploadDate.ToString();

                }
                else
                {
                    team.WrittenReportUrl = "-?CT=Stylish_not_ok.png";
                    team.WrittenReportUrlForMember = "";
                    team.WrittenReportDate = null;
                    team.WrittenReportDateString = "";
                }

                var invoiceDetail = invoiceDetails.FirstOrDefault(i => i.TeamId == team.Id);

                if (invoiceDetail != null)
                {
                    // if (team.PaidByCheque == true)
                    {
                        team.Amount = invoiceDetail.Amount;
                    }
                    //else
                    //{
                    //    team.Amount = invoiceDetail.Amount + invoiceDetail.Amount * 2.5m / 100;
                    //}
                }
            }

            return vmTeamFullInfoList;
        }
        public IEnumerable<VmTeamFullInfo> GetNoneAdminTeamFullInfoByFilter(string noneAdminUserId, VmTeamFullInfo filterItem, string roleName)
        {
            var viewTeamFullInfoRepository = UnitOfWork.GetRepository<ViewTeamFullInfoRepository>();

            var viewFilterItem = new VmTeamFullInfo
            {
                Name = filterItem.Name,
                Leader = filterItem.Leader,
                Advisor = filterItem.Advisor,
                TaskName = filterItem.TaskName,
                Judges = filterItem.Judges,
                PayStatus = filterItem.PayStatus,
                Deactivate = filterItem.Deactivate,
                RegistrationStatus = filterItem.RegistrationStatus,
                //SafetyStatus = filterItem.SafetyStatus,
                //SafetyStatus = filterItem.WrittenReportStatus,
                //Shipping = filterItem.Shipping,
                //Survey = filterItem.Survey,
                PreliminaryReportStatus = filterItem.PreliminaryReportStatus,
                FlashTalkReportStatus = filterItem.FlashTalkReportStatus,
                OpenTaskTestPlanStatus = filterItem.OpenTaskTestPlanStatus,
            };

            var teamId = new BLTeamMember().GetTeamMemberByUserId(noneAdminUserId).TeamId;
            var advisorUserId = new BLTeamMember().GetTeamMembersByRoleName(teamId, "Advisor").First().MemberUserId;

            var viewteamFullInfoList = viewTeamFullInfoRepository.SelectByAdvisor(advisorUserId, viewFilterItem, 0, int.MaxValue);

            var vmTeamFullInfoList = (from teamFullInfo in viewteamFullInfoList
                                      select new VmTeamFullInfo
                                      {
                                          Id = teamFullInfo.Id,
                                          Payment = teamFullInfo.Payment,
                                          TaskId = teamFullInfo.TaskId,
                                          ProjectName = teamFullInfo.ProjectName,
                                          Name = teamFullInfo.Name,
                                          TaskName = teamFullInfo.TaskName,
                                          TeamImageUrl = teamFullInfo.TeamImageUrl,
                                          TeamState = teamFullInfo.TeamState.Value,
                                          TeamStateDescription = teamFullInfo.TeamState.ToString(),
                                          SafetyStatus = /*teamFullInfo.SafetyStatus*/ false,
                                          PayStatus = teamFullInfo.PayStatus,
                                          PaidByCheque = teamFullInfo.PaidByCheque,
                                          PaidByChequePercent = teamFullInfo.PaidByChequePercent,
                                          PayStatusDescription = (teamFullInfo.PayStatus == true) ? PayStatus.Payed.ToString() : PayStatus.NotPayed.ToString(),
                                          RegistrationStatus = teamFullInfo.RegistrationStatus,
                                          Survey = teamFullInfo.Survey,
                                          Advisor = teamFullInfo.Advisor,
                                          Leader = teamFullInfo.Leader,
                                          Judges = teamFullInfo.Judges,
                                          PhoneNumber = teamFullInfo.PhoneNumber,
                                          WorkPhoneNumber = teamFullInfo.WorkPhoneNumber ?? "",
                                          Identifier = teamFullInfo.Identifier,
                                          Sex = teamFullInfo.Sex,
                                          BirthDate = teamFullInfo.BirthDate,
                                          UserName = teamFullInfo.UserName,
                                          Email = teamFullInfo.Email,
                                          RegisterDate = teamFullInfo.RegisterDate,
                                          RoleName = teamFullInfo.RoleName,
                                          RoleId = teamFullInfo.RoleId,
                                          UserDefiner = teamFullInfo.UserDefiner,
                                          LastSignIn = teamFullInfo.LastSignIn,
                                          UniversityId = teamFullInfo.UniversityId,
                                          University = teamFullInfo.University,
                                          JacketSizeId = teamFullInfo.JacketSizeId,
                                          JacketSize = teamFullInfo.JacketSize ?? "",
                                          DietTypeId = teamFullInfo.DietTypeId,
                                          DietType = teamFullInfo.DietType ?? "",
                                          StreetLine1 = teamFullInfo.StreetLine1,
                                          StreetLine2 = teamFullInfo.StreetLine2,
                                          City = teamFullInfo.City,
                                          State = teamFullInfo.State,
                                          ZipCode = teamFullInfo.ZipCode,
                                          EmgyPersonRelationship = teamFullInfo.EmgyPersonRelationship,
                                          EmgyPersonPhoneNumber = teamFullInfo.EmgyPersonPhoneNumber,
                                          EmgyPersonLastName = teamFullInfo.EmgyPersonLastName,
                                          EmgyPersonFirstName = teamFullInfo.EmgyPersonFirstName,
                                          ShortBio = teamFullInfo.ShortBio,
                                          T_Shirt_Size = teamFullInfo.T_Shirt_Size,
                                          ProfilePictureUrl = teamFullInfo.ProfilePictureUrl,
                                          UniversityPictureUrl = teamFullInfo.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                          LabResultUrl = teamFullInfo.LabResultUrl,

                                          PreliminaryReportStatus = (teamFullInfo.Preliminary == false || teamFullInfo.Preliminary == null) ? "" :
                                          !string.IsNullOrWhiteSpace(teamFullInfo.PreliminaryReportUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                                          PreliminaryReportUrlForMember = teamFullInfo.PreliminaryReportUrl,
                                          PreliminaryReportUrl = teamFullInfo.PreliminaryReportUrl,
                                          PreliminaryReportDate = teamFullInfo.PreliminaryReportDate,

                                          OpenTaskTestPlanStatus = (teamFullInfo.OpenTaskTestPlan == false || teamFullInfo.OpenTaskTestPlan == null) ? "" :
                                          !string.IsNullOrWhiteSpace(teamFullInfo.OpenTaskTestPlanUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                                          OpenTaskTestPlanUrlForMember = teamFullInfo.OpenTaskTestPlanUrl,
                                          OpenTaskTestPlanUrl = teamFullInfo.OpenTaskTestPlanUrl,
                                          OpenTaskTestPlanDate = teamFullInfo.OpenTaskTestPlanDate,

                                          WrittenReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.WrittenReportUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          WrittenReportUrlForMember = teamFullInfo.WrittenReportUrl,
                                          WrittenReportDate = teamFullInfo.WrittenReportDate,
                                          WrittenReportDateString = (teamFullInfo.WrittenReportDate != null) ? teamFullInfo.WrittenReportDate.ToString() : null,

                                          FlashTalkReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.FlashTalkReportUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          FlashTalkReportUrlForMember = teamFullInfo.FlashTalkReportUrl,
                                          FlashTalkReportDate = teamFullInfo.FlashTalkReportDate,

                                          BrochureUrl = string.IsNullOrWhiteSpace(teamFullInfo.BrochureUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          BrochureUrlForMember = teamFullInfo.BrochureUrl,
                                          BrochureDate = teamFullInfo.BrochureDate,
                                          BrochureDateString = (teamFullInfo.BrochureDate != null) ? teamFullInfo.BrochureDate.ToString() : null,

                                          AwardNominationUrl = string.IsNullOrWhiteSpace(teamFullInfo.AwardNominationUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                          AwardNominationUrlForMember = teamFullInfo.AwardNominationUrl,
                                          AwardNominationDate = teamFullInfo.AwardNominationDate,


                                          Shipping = "",
                                          ResumeUrl = teamFullInfo.ResumeUrl,
                                          Date = teamFullInfo.Date,
                                          EmailConfirmed = teamFullInfo.EmailConfirmed,
                                          LockoutEnabled = teamFullInfo.LockoutEnabled,
                                          Deactivate = teamFullInfo.Deactivate,
                                          SubmitStatus = teamFullInfo.SubmitStatus,
                                          Status = teamFullInfo.Status,
                                          Approved = teamFullInfo.Approved,

                                          PreliminaryReport = teamFullInfo.Preliminary,
                                          Scored = teamFullInfo.Scored,
                                          OpenTaskTestPlan = teamFullInfo.OpenTaskTestPlan,
                                          MemberUserId = teamFullInfo.MemberUserId,

                                      }).ToList();

            var blInvoice = new BLInvoice();

            var teamIds = vmTeamFullInfoList.Select(t => t.Id).ToArray();

            var invoiceDetails = blInvoice.GetInvoiceDetailByTeams(teamIds);


            var blSubmissionRule = new BLSubmissionRule();
            var submissionRuleList = blSubmissionRule.GetSubmissionRuleByTeams(teamIds);

            foreach (var team in vmTeamFullInfoList)
            {
                var writtenReportRule = (from s in submissionRuleList
                                         where s.Id == 3 && s.TeamId == team.Id
                                         select s).FirstOrDefault();

                if (writtenReportRule != null)
                {
                    team.WrittenReportUrl = string.IsNullOrWhiteSpace(writtenReportRule.SubmissionRuleUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png";
                    team.WrittenReportUrlForMember = writtenReportRule.SubmissionRuleUrl;
                    team.WrittenReportDate = DateTime.Parse(writtenReportRule.DueDate);
                    team.WrittenReportDateString = writtenReportRule.DueDate;

                }
                var invoiceDetail = invoiceDetails.FirstOrDefault(i => i.TeamId == team.Id);

                if (invoiceDetail != null)
                {
                    // if (team.PaidByCheque == true)
                    {
                        team.Amount = invoiceDetail.Amount;
                    }
                    //else
                    //{
                    //    team.Amount = invoiceDetail.Amount + invoiceDetail.Amount * 2.75m / 100;
                    //}
                }
            }

            return vmTeamFullInfoList;
        }
        public IEnumerable<VmTeamFullInfo> GetTeamFullInfoByFilterByAdvisor(string advisorUserId, VmTeamFullInfo filterItem)
        {
            var viewTeamFullInfoRepository = UnitOfWork.GetRepository<ViewTeamFullInfoRepository>();

            var viewFilterItem = new VmTeamFullInfo
            {
                Name = filterItem.Name,
                Leader = filterItem.Leader,
                Advisor = filterItem.Advisor,
                TaskName = filterItem.TaskName,
                Judges = filterItem.Judges,
                PayStatus = filterItem.PayStatus,
                PreliminaryReportStatus = filterItem.PreliminaryReportStatus,
                FlashTalkReportStatus = filterItem.FlashTalkReportStatus,
                OpenTaskTestPlanStatus = filterItem.OpenTaskTestPlanStatus,
            };

            var viewteamFullInfoList = viewTeamFullInfoRepository.SelectByAdvisor(advisorUserId, viewFilterItem, 0, int.MaxValue);

            var vmTeamFullInfoList = from teamFullInfo in viewteamFullInfoList
                                     select new VmTeamFullInfo
                                     {
                                         Id = teamFullInfo.Id,
                                         Payment = teamFullInfo.Payment,
                                         TaskId = teamFullInfo.TaskId,
                                         ProjectName = teamFullInfo.ProjectName,
                                         Name = teamFullInfo.Name,
                                         TaskName = teamFullInfo.TaskName,
                                         TeamImageUrl = teamFullInfo.TeamImageUrl,
                                         TeamState = teamFullInfo.TeamState.Value,
                                         TeamStateDescription = teamFullInfo.TeamState.ToString(),
                                         SafetyStatus = /*teamFullInfo.SafetyStatus*/ false,
                                         PayStatus = teamFullInfo.PayStatus,
                                         PaidByCheque = teamFullInfo.PaidByCheque,
                                         PaidByChequePercent = teamFullInfo.PaidByChequePercent,
                                         PayStatusDescription = (teamFullInfo.PayStatus == true) ? PayStatus.Payed.ToString() : PayStatus.NotPayed.ToString(),
                                         RegistrationStatus = teamFullInfo.RegistrationStatus,
                                         Survey = teamFullInfo.Survey,
                                         Advisor = teamFullInfo.Advisor,
                                         Leader = teamFullInfo.Leader,
                                         Judges = teamFullInfo.Judges,
                                         PhoneNumber = teamFullInfo.PhoneNumber,
                                         WorkPhoneNumber = teamFullInfo.WorkPhoneNumber ?? "",
                                         Identifier = teamFullInfo.Identifier,
                                         Sex = teamFullInfo.Sex,
                                         BirthDate = teamFullInfo.BirthDate,
                                         UserName = teamFullInfo.UserName,
                                         Email = teamFullInfo.Email,
                                         RegisterDate = teamFullInfo.RegisterDate,
                                         RoleName = teamFullInfo.RoleName,
                                         RoleId = teamFullInfo.RoleId,
                                         UserDefiner = teamFullInfo.UserDefiner,
                                         LastSignIn = teamFullInfo.LastSignIn,
                                         UniversityId = teamFullInfo.UniversityId,
                                         University = teamFullInfo.University,
                                         JacketSizeId = teamFullInfo.JacketSizeId,
                                         JacketSize = teamFullInfo.JacketSize ?? "",
                                         DietTypeId = teamFullInfo.DietTypeId,
                                         DietType = teamFullInfo.DietType ?? "",
                                         StreetLine1 = teamFullInfo.StreetLine1,
                                         StreetLine2 = teamFullInfo.StreetLine2,
                                         City = teamFullInfo.City,
                                         State = teamFullInfo.State,
                                         ZipCode = teamFullInfo.ZipCode,
                                         EmgyPersonRelationship = teamFullInfo.EmgyPersonRelationship,
                                         EmgyPersonPhoneNumber = teamFullInfo.EmgyPersonPhoneNumber,
                                         EmgyPersonLastName = teamFullInfo.EmgyPersonLastName,
                                         EmgyPersonFirstName = teamFullInfo.EmgyPersonFirstName,
                                         ShortBio = teamFullInfo.ShortBio,
                                         T_Shirt_Size = teamFullInfo.T_Shirt_Size,
                                         ProfilePictureUrl = teamFullInfo.ProfilePictureUrl,
                                         UniversityPictureUrl = teamFullInfo.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                         LabResultUrl = teamFullInfo.LabResultUrl,

                                         PreliminaryReportStatus = (teamFullInfo.Preliminary == false || teamFullInfo.Preliminary == null) ? "" :
                                         !string.IsNullOrWhiteSpace(teamFullInfo.PreliminaryReportUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                                         PreliminaryReportUrlForMember = teamFullInfo.PreliminaryReportUrl,
                                         PreliminaryReportUrl = teamFullInfo.PreliminaryReportUrl,
                                         PreliminaryReportDate = teamFullInfo.PreliminaryReportDate,


                                         OpenTaskTestPlanStatus = (teamFullInfo.OpenTaskTestPlan == false || teamFullInfo.OpenTaskTestPlan == null) ? "" :
                                         !string.IsNullOrWhiteSpace(teamFullInfo.OpenTaskTestPlanUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                                         OpenTaskTestPlanUrlForMember = teamFullInfo.OpenTaskTestPlanUrl,
                                         OpenTaskTestPlanUrl = teamFullInfo.OpenTaskTestPlanUrl,
                                         OpenTaskTestPlanDate = teamFullInfo.OpenTaskTestPlanDate,

                                         WrittenReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.WrittenReportUrl)
                                                ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                         WrittenReportUrlForMember = teamFullInfo.WrittenReportUrl,
                                         WrittenReportDate = teamFullInfo.WrittenReportDate,

                                         FlashTalkReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.FlashTalkReportUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                         FlashTalkReportUrlForMember = teamFullInfo.FlashTalkReportUrl,
                                         FlashTalkReportDate = teamFullInfo.FlashTalkReportDate,

                                         BrochureUrl = string.IsNullOrWhiteSpace(teamFullInfo.BrochureUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                         BrochureUrlForMember = teamFullInfo.BrochureUrl,
                                         BrochureDate = teamFullInfo.BrochureDate,

                                         AwardNominationUrl = string.IsNullOrWhiteSpace(teamFullInfo.AwardNominationUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                                         AwardNominationUrlForMember = teamFullInfo.AwardNominationUrl,
                                         AwardNominationDate = teamFullInfo.AwardNominationDate,

                                         Shipping = "",
                                         ResumeUrl = teamFullInfo.ResumeUrl,
                                         Date = teamFullInfo.Date,
                                         EmailConfirmed = teamFullInfo.EmailConfirmed,
                                         LockoutEnabled = teamFullInfo.LockoutEnabled,
                                         Deactivate = teamFullInfo.Deactivate,
                                         SubmitStatus = teamFullInfo.SubmitStatus,
                                         Status = teamFullInfo.Status,
                                         Approved = teamFullInfo.Approved,
                                         PreliminaryReport = teamFullInfo.Preliminary,
                                         Scored = teamFullInfo.Scored,
                                         OpenTaskTestPlan = teamFullInfo.OpenTaskTestPlan,
                                         MemberUserId = teamFullInfo.MemberUserId,
                                     };
            return vmTeamFullInfoList;
        }
        public VmTeamFullInfo GetTeamFullInfoById(int teamId)
        {
            var viewTeamFullInfoRepository = UnitOfWork.GetRepository<ViewTeamFullInfoRepository>();



            var teamFullInfo = viewTeamFullInfoRepository.GetTeamFullInfoById(teamId);

            var vmTeamFullInfoList = new VmTeamFullInfo
            {
                Id = teamFullInfo.Id,
                Payment = teamFullInfo.Payment,
                TaskId = teamFullInfo.TaskId,
                ProjectName = teamFullInfo.ProjectName,
                Name = teamFullInfo.Name,
                TaskName = teamFullInfo.TaskName,
                TeamImageUrl = teamFullInfo.TeamImageUrl,
                TeamState = teamFullInfo.TeamState.Value,
                TeamStateDescription = teamFullInfo.TeamState.ToString(),
                SafetyStatus = /*teamFullInfo.SafetyStatus*/ false,
                PayStatus = teamFullInfo.PayStatus,
                PaidByCheque = teamFullInfo.PaidByCheque,
                PaidByChequePercent = teamFullInfo.PaidByChequePercent,
                PayStatusDescription = (teamFullInfo.PayStatus == true) ? PayStatus.Payed.ToString() : PayStatus.NotPayed.ToString(),
                RegistrationStatus = teamFullInfo.RegistrationStatus,
                Survey = teamFullInfo.Survey,
                Advisor = teamFullInfo.Advisor,
                Leader = teamFullInfo.Leader,
                Judges = teamFullInfo.Judges,
                PhoneNumber = teamFullInfo.PhoneNumber,
                WorkPhoneNumber = teamFullInfo.WorkPhoneNumber ?? "",
                Identifier = teamFullInfo.Identifier,
                Sex = teamFullInfo.Sex,
                BirthDate = teamFullInfo.BirthDate,
                UserName = teamFullInfo.UserName,
                Email = teamFullInfo.Email,
                RegisterDate = teamFullInfo.RegisterDate,
                RoleName = teamFullInfo.RoleName,
                RoleId = teamFullInfo.RoleId,
                UserDefiner = teamFullInfo.UserDefiner,
                LastSignIn = teamFullInfo.LastSignIn,
                UniversityId = teamFullInfo.UniversityId,
                University = teamFullInfo.University,
                JacketSizeId = teamFullInfo.JacketSizeId,
                JacketSize = teamFullInfo.JacketSize ?? "",
                DietTypeId = teamFullInfo.DietTypeId,
                DietType = teamFullInfo.DietType ?? "",
                StreetLine1 = teamFullInfo.StreetLine1,
                StreetLine2 = teamFullInfo.StreetLine2,
                City = teamFullInfo.City,
                State = teamFullInfo.State,
                ZipCode = teamFullInfo.ZipCode,
                EmgyPersonRelationship = teamFullInfo.EmgyPersonRelationship,
                EmgyPersonPhoneNumber = teamFullInfo.EmgyPersonPhoneNumber,
                EmgyPersonLastName = teamFullInfo.EmgyPersonLastName,
                EmgyPersonFirstName = teamFullInfo.EmgyPersonFirstName,
                ShortBio = teamFullInfo.ShortBio,
                T_Shirt_Size = teamFullInfo.T_Shirt_Size,
                ProfilePictureUrl = teamFullInfo.ProfilePictureUrl,
                UniversityPictureUrl = teamFullInfo.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                LabResultUrl = teamFullInfo.LabResultUrl,

                PreliminaryReportStatus = (teamFullInfo.Preliminary == false || teamFullInfo.Preliminary == null) ? "" :
                                         !string.IsNullOrWhiteSpace(teamFullInfo.PreliminaryReportUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                PreliminaryReportUrlForMember = teamFullInfo.PreliminaryReportUrl,
                PreliminaryReportUrl = teamFullInfo.PreliminaryReportUrl,
                PreliminaryReportDate = teamFullInfo.PreliminaryReportDate,


                OpenTaskTestPlanStatus = (teamFullInfo.OpenTaskTestPlan == false || teamFullInfo.OpenTaskTestPlan == null) ? "" :
                                         !string.IsNullOrWhiteSpace(teamFullInfo.OpenTaskTestPlanUrl) ? "/Resources/Images/Stylish_ok.png" : "/Resources/Images/Stylish_not_ok.png",

                OpenTaskTestPlanUrlForMember = teamFullInfo.OpenTaskTestPlanUrl,
                OpenTaskTestPlanUrl = teamFullInfo.OpenTaskTestPlanUrl,
                OpenTaskTestPlanDate = teamFullInfo.OpenTaskTestPlanDate,

                WrittenReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.WrittenReportUrl)
                                                ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                WrittenReportUrlForMember = teamFullInfo.WrittenReportUrl,
                WrittenReportDate = teamFullInfo.WrittenReportDate,

                FlashTalkReportUrl = string.IsNullOrWhiteSpace(teamFullInfo.FlashTalkReportUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                FlashTalkReportUrlForMember = teamFullInfo.FlashTalkReportUrl,
                FlashTalkReportDate = teamFullInfo.FlashTalkReportDate,

                BrochureUrl = string.IsNullOrWhiteSpace(teamFullInfo.BrochureUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                BrochureUrlForMember = teamFullInfo.BrochureUrl,
                BrochureDate = teamFullInfo.BrochureDate,

                AwardNominationUrl = string.IsNullOrWhiteSpace(teamFullInfo.AwardNominationUrl)
                                                 ? "-?CT=Stylish_not_ok.png" : "-?CT=Stylish_ok.png",
                AwardNominationUrlForMember = teamFullInfo.AwardNominationUrl,
                AwardNominationDate = teamFullInfo.AwardNominationDate,

                Shipping = "",
                ResumeUrl = teamFullInfo.ResumeUrl,
                Date = teamFullInfo.Date,
                EmailConfirmed = teamFullInfo.EmailConfirmed,
                LockoutEnabled = teamFullInfo.LockoutEnabled,
                Deactivate = teamFullInfo.Deactivate,
                SubmitStatus = teamFullInfo.SubmitStatus,
                Status = teamFullInfo.Status,
                Approved = teamFullInfo.Approved,
                PreliminaryReport = teamFullInfo.Preliminary,
                Scored = teamFullInfo.Scored,
                OpenTaskTestPlan = teamFullInfo.OpenTaskTestPlan,
                MemberUserId = teamFullInfo.MemberUserId,
            };
            return vmTeamFullInfoList;
        }

    }

}