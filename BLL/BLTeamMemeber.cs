using BLL.Base;
using Model;
using Model.ViewModels.Team;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLTeamMember : BLBase
    {
        public VmTeamMember GetTeamMemberById(int id)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMember = teamMemberRepository.GetTeamMemberById(id);
            var vmTeamMember = new VmTeamMember
            {
                Id = teamMember.Id,
                TeamId = teamMember.TeamId,
                TaskId = teamMember.TaskId,
                MemberUserId = teamMember.MemberUserId,
                TeamName = teamMember.TeamName,
                Task = teamMember.Task,
                TeamImageUrl = teamMember.TeamImageUrl,
                RegistrationStatus = teamMember.RegistrationStatus,
                Survey = teamMember.Survey,
                MemberName = teamMember.MemberName,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                PhoneNumber = teamMember.PhoneNumber,
                WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                Identifier = teamMember.Identifier,
                Sex = teamMember.Sex,
                BirthDate = teamMember.BirthDate,
                UserName = teamMember.UserName,
                Email = teamMember.Email,
                RegisterDate = teamMember.RegisterDate,
                RoleName = teamMember.RoleName,
                RoleId = teamMember.RoleId,
                UserDefiner = teamMember.UserDefiner,
                LastSignIn = teamMember.LastSignIn,
                UniversityId = teamMember.UniversityId,
                University = teamMember.University,
                JacketSizeId = teamMember.JacketSizeId,
                JacketSize = teamMember.JacketSize ?? "",
                DietTypeId = teamMember.DietTypeId,
                DietType = teamMember.DietType ?? "",
                StreetLine1 = teamMember.StreetLine1,
                StreetLine2 = teamMember.StreetLine2,
                City = teamMember.City,
                State = teamMember.State,
                ZipCode = teamMember.ZipCode,
                EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                EmgyPersonLastName = teamMember.EmgyPersonLastName,
                EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                ShortBio = teamMember.ShortBio,
                TShirtSize = teamMember.T_Shirt_Size,
                ProfilePictureUrl = teamMember.ProfilePictureUrl,
                UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                LabResultUrl = teamMember.LabResultUrl,

                PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                Shipping = "",
                ResumeUrl = teamMember.ResumeUrl,
                Date = teamMember.Date,
                EmailConfirmed = teamMember.EmailConfirmed,
                LockoutEnabled = teamMember.LockoutEnabled,
                IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                PreliminaryStatus = teamMember.PreliminaryStatus,
                OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
            };

            return vmTeamMember;
        }
        public IEnumerable<VmTeamMember> GetTeamMembers(int teamId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMemberList = teamMemberRepository.GetTeamMembers(teamId);
            var vmTeamMemberList = from teamMember in teamMemberList
                                   select new VmTeamMember
                                   {
                                       Id = teamMember.Id,
                                       TeamId = teamMember.TeamId,
                                       TaskId = teamMember.TaskId,
                                       MemberUserId = teamMember.MemberUserId,
                                       TeamName = teamMember.TeamName,
                                       Task = teamMember.Task,
                                       TeamImageUrl = teamMember.TeamImageUrl,
                                       RegistrationStatus = teamMember.RegistrationStatus,
                                       Survey = teamMember.Survey,
                                       MemberName = teamMember.MemberName,
                                       FirstName = teamMember.FirstName,
                                       LastName = teamMember.LastName,
                                       PhoneNumber = teamMember.PhoneNumber,
                                       WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                                       Identifier = teamMember.Identifier,
                                       Sex = teamMember.Sex,
                                       BirthDate = teamMember.BirthDate,
                                       UserName = teamMember.UserName,
                                       Email = teamMember.Email,
                                       RegisterDate = teamMember.RegisterDate,
                                       RoleName = teamMember.RoleName,
                                       RoleId = teamMember.RoleId,
                                       UserDefiner = teamMember.UserDefiner,
                                       LastSignIn = teamMember.LastSignIn,
                                       UniversityId = teamMember.UniversityId,
                                       University = teamMember.University,
                                       JacketSizeId = teamMember.JacketSizeId,
                                       JacketSize = teamMember.JacketSize ?? "",
                                       DietTypeId = teamMember.DietTypeId,
                                       DietType = teamMember.DietType ?? "",
                                       StreetLine1 = teamMember.StreetLine1,
                                       StreetLine2 = teamMember.StreetLine2,
                                       City = teamMember.City,
                                       State = teamMember.State,
                                       ZipCode = teamMember.ZipCode,
                                       EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                                       EmgyPersonLastName = teamMember.EmgyPersonLastName,
                                       EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                                       EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                                       ShortBio = teamMember.ShortBio,
                                       TShirtSize = teamMember.T_Shirt_Size,
                                       ProfilePictureUrl = teamMember.ProfilePictureUrl,
                                       UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                       LabResultUrl = teamMember.LabResultUrl,

                                       PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                                       OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                                       WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                                       Shipping = "",
                                       ResumeUrl = teamMember.ResumeUrl,
                                       Date = teamMember.Date,
                                       EmailConfirmed = teamMember.EmailConfirmed,
                                       LockoutEnabled = teamMember.LockoutEnabled,
                                       IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                                       IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                                       PreliminaryStatus = teamMember.PreliminaryStatus,
                                       OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
                                   };

            return vmTeamMemberList;
        }
        public IEnumerable<VmTeamMember> GetTeamMembersByRoles(int teamId, string[] roles)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMemberList = teamMemberRepository.GetTeamMembersByRoles(teamId, roles);
            var vmTeamMemberList = (from teamMember in teamMemberList
                                    select new VmTeamMember
                                    {
                                        Id = teamMember.Id,
                                        TeamId = teamMember.TeamId,
                                        TaskId = teamMember.TaskId,
                                        MemberUserId = teamMember.MemberUserId,
                                        TeamName = teamMember.TeamName,
                                        Task = teamMember.Task,
                                        TeamImageUrl = teamMember.TeamImageUrl,
                                        RegistrationStatus = teamMember.RegistrationStatus,
                                        Survey = teamMember.Survey,
                                        MemberName = teamMember.MemberName,
                                        FirstName = teamMember.FirstName,
                                        LastName = teamMember.LastName,
                                        PhoneNumber = teamMember.PhoneNumber,
                                        WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                                        Identifier = teamMember.Identifier,
                                        Sex = teamMember.Sex,
                                        BirthDate = teamMember.BirthDate,
                                        UserName = teamMember.UserName,
                                        Email = teamMember.Email,
                                        RegisterDate = teamMember.RegisterDate,
                                        RoleName = teamMember.RoleName,
                                        RoleId = teamMember.RoleId,
                                        UserDefiner = teamMember.UserDefiner,
                                        LastSignIn = teamMember.LastSignIn,
                                        UniversityId = teamMember.UniversityId,
                                        University = teamMember.University,
                                        JacketSizeId = teamMember.JacketSizeId,
                                        JacketSize = teamMember.JacketSize ?? "",
                                        DietTypeId = teamMember.DietTypeId,
                                        DietType = teamMember.DietType ?? "",
                                        StreetLine1 = teamMember.StreetLine1,
                                        StreetLine2 = teamMember.StreetLine2,
                                        City = teamMember.City,
                                        State = teamMember.State,
                                        ZipCode = teamMember.ZipCode,
                                        EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                                        EmgyPersonLastName = teamMember.EmgyPersonLastName,
                                        EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                                        EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                                        ShortBio = teamMember.ShortBio,
                                        TShirtSize = teamMember.T_Shirt_Size,
                                        ProfilePictureUrl = teamMember.ProfilePictureUrl,
                                        UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                        LabResultUrl = teamMember.LabResultUrl,

                                        PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                                        OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                                        WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                                        Shipping = "",
                                        ResumeUrl = teamMember.ResumeUrl,
                                        Date = teamMember.Date,
                                        EmailConfirmed = teamMember.EmailConfirmed,
                                        LockoutEnabled = teamMember.LockoutEnabled,
                                        IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                                        IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                                        PreliminaryStatus = teamMember.PreliminaryStatus,
                                        OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
                                    }).ToArray();

            return vmTeamMemberList;
        }
        public IEnumerable<VmTeamMember> GetTeamMembersByRoleName(int teamId, string roleName)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMemberList = teamMemberRepository.GetTeamMembersByRoleNameAndTeam(teamId, roleName);
            var vmTeamMemberList = (from teamMember in teamMemberList
                                    select new VmTeamMember
                                    {
                                        Id = teamMember.Id,
                                        TeamId = teamMember.TeamId,
                                        TaskId = teamMember.TaskId,
                                        MemberUserId = teamMember.MemberUserId,
                                        TeamName = teamMember.TeamName,
                                        Task = teamMember.Task,
                                        TeamImageUrl = teamMember.TeamImageUrl,
                                        RegistrationStatus = teamMember.RegistrationStatus,
                                        Survey = teamMember.Survey,
                                        MemberName = teamMember.MemberName,
                                        FirstName = teamMember.FirstName,
                                        LastName = teamMember.LastName,
                                        PhoneNumber = teamMember.PhoneNumber,
                                        WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                                        Identifier = teamMember.Identifier,
                                        Sex = teamMember.Sex,
                                        BirthDate = teamMember.BirthDate,
                                        UserName = teamMember.UserName,
                                        Email = teamMember.Email,
                                        RegisterDate = teamMember.RegisterDate,
                                        RoleName = teamMember.RoleName,
                                        RoleId = teamMember.RoleId,
                                        UserDefiner = teamMember.UserDefiner,
                                        LastSignIn = teamMember.LastSignIn,
                                        UniversityId = teamMember.UniversityId,
                                        University = teamMember.University,
                                        JacketSizeId = teamMember.JacketSizeId,
                                        JacketSize = teamMember.JacketSize ?? "",
                                        DietTypeId = teamMember.DietTypeId,
                                        DietType = teamMember.DietType ?? "",
                                        StreetLine1 = teamMember.StreetLine1,
                                        StreetLine2 = teamMember.StreetLine2,
                                        City = teamMember.City,
                                        State = teamMember.State,
                                        ZipCode = teamMember.ZipCode,
                                        EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                                        EmgyPersonLastName = teamMember.EmgyPersonLastName,
                                        EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                                        EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                                        ShortBio = teamMember.ShortBio,
                                        TShirtSize = teamMember.T_Shirt_Size,
                                        ProfilePictureUrl = teamMember.ProfilePictureUrl,
                                        UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                        LabResultUrl = teamMember.LabResultUrl,

                                        PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                                        OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                                        WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                                        Shipping = "",
                                        ResumeUrl = teamMember.ResumeUrl,
                                        Date = teamMember.Date,
                                        EmailConfirmed = teamMember.EmailConfirmed,
                                        LockoutEnabled = teamMember.LockoutEnabled,
                                        IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                                        IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                                        PreliminaryStatus = teamMember.PreliminaryStatus,
                                        OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
                                    }).ToArray();

            return vmTeamMemberList;
        }
        public string[] GetTeamMembersUserIds(int teamId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var userIds = teamMemberRepository.GetTeamMembersUserIds(teamId);

            return userIds;
        }
          public string[] GetTeamMembersEmails(int teamId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var emails = teamMemberRepository.GetTeamMembersEmails(teamId);

            return emails;
        }
        public IEnumerable<VmTeamMemberUserId> GetAllTeamMembersUserIds()
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var memberUserIdList = teamMemberRepository.GetAllTeamMembersUserIds();

            var vmTeamMemberUserIdList = from m in memberUserIdList
                                         group new { m.MemberUserId, m.TeamId } by new { m.TeamId } into g

                                         select new VmTeamMemberUserId
                                         {
                                             TeamId = g.Key.TeamId,
                                             UserIds = (from u in g select u.MemberUserId).ToArray()
                                         };

            return vmTeamMemberUserIdList;
        }
        public IEnumerable<VmTeamMember> GetAllTeamMembers()
        {
            
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var viewteamMemberList = teamMemberRepository.GetAllTeamMembers();
            var vmTeamMember = from teamMember in viewteamMemberList
                               select new VmTeamMember
                               {
                                   Id = teamMember.Id,
                                   TeamId = teamMember.TeamId,
                                   TaskId = teamMember.TaskId,
                                   MemberUserId = teamMember.MemberUserId,
                                   TeamName = teamMember.TeamName,
                                   Task = teamMember.Task,
                                   TeamImageUrl = teamMember.TeamImageUrl,
                                   RegistrationStatus = teamMember.RegistrationStatus,
                                   Survey = teamMember.Survey,
                                   MemberName = teamMember.MemberName,
                                   FirstName = teamMember.FirstName,
                                   LastName = teamMember.LastName,
                                   PhoneNumber = teamMember.PhoneNumber,
                                   WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                                   Identifier = teamMember.Identifier,
                                   Sex = teamMember.Sex,
                                   BirthDate = teamMember.BirthDate,
                                   UserName = teamMember.UserName,
                                   Email = teamMember.Email,
                                   RegisterDate = teamMember.RegisterDate,
                                   RoleName = teamMember.RoleName,
                                   RoleId = teamMember.RoleId,
                                   UserDefiner = teamMember.UserDefiner,
                                   LastSignIn = teamMember.LastSignIn,
                                   UniversityId = teamMember.UniversityId,
                                   University = teamMember.University,
                                   JacketSizeId = teamMember.JacketSizeId,
                                   JacketSize = teamMember.JacketSize ?? "",
                                   DietTypeId = teamMember.DietTypeId,
                                   DietType = teamMember.DietType ?? "",
                                   StreetLine1 = teamMember.StreetLine1,
                                   StreetLine2 = teamMember.StreetLine2,
                                   City = teamMember.City,
                                   State = teamMember.State,
                                   ZipCode = teamMember.ZipCode,
                                   EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                                   EmgyPersonLastName = teamMember.EmgyPersonLastName,
                                   EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                                   EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                                   ShortBio = teamMember.ShortBio,
                                   TShirtSize = teamMember.T_Shirt_Size,
                                   ProfilePictureUrl = teamMember.ProfilePictureUrl,
                                   UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                   LabResultUrl = teamMember.LabResultUrl,

                                   PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                                   OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                                   WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                                   Shipping = "",
                                   ResumeUrl = teamMember.ResumeUrl,
                                   Date = teamMember.Date,
                                   EmailConfirmed = teamMember.EmailConfirmed,
                                   LockoutEnabled = teamMember.LockoutEnabled,
                                   IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                                   IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                                   PreliminaryStatus = teamMember.PreliminaryStatus,
                                   OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
                               };

            return vmTeamMember;

        }
        public int GetTeamMembersCount(int teamId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            return teamMemberRepository.GetTeamMembersCount(teamId);

        }

        public VmTeamMember GetTeamMemberByUserId(string memberUserId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMember = teamMemberRepository.GetTeamMember(memberUserId);
            var vmTeamMember = new VmTeamMember
            {
                Id = teamMember.Id,
                TeamId = teamMember.TeamId,
                TaskId = teamMember.TaskId,
                MemberUserId = teamMember.MemberUserId,
                TeamName = teamMember.TeamName,
                Task = teamMember.Task,
                TeamImageUrl = teamMember.TeamImageUrl,
                RegistrationStatus = teamMember.RegistrationStatus,
                Survey = teamMember.Survey,
                MemberName = teamMember.MemberName,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                PhoneNumber = teamMember.PhoneNumber,
                WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                Identifier = teamMember.Identifier,
                Sex = teamMember.Sex,
                BirthDate = teamMember.BirthDate,
                UserName = teamMember.UserName,
                Email = teamMember.Email,
                RegisterDate = teamMember.RegisterDate,
                RoleName = teamMember.RoleName,
                RoleId = teamMember.RoleId,
                UserDefiner = teamMember.UserDefiner,
                LastSignIn = teamMember.LastSignIn,
                UniversityId = teamMember.UniversityId,
                University = teamMember.University,
                JacketSizeId = teamMember.JacketSizeId,
                JacketSize = teamMember.JacketSize ?? "",
                DietTypeId = teamMember.DietTypeId,
                DietType = teamMember.DietType ?? "",
                StreetLine1 = teamMember.StreetLine1,
                StreetLine2 = teamMember.StreetLine2,
                City = teamMember.City,
                State = teamMember.State,
                ZipCode = teamMember.ZipCode,
                EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                EmgyPersonLastName = teamMember.EmgyPersonLastName,
                EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                ShortBio = teamMember.ShortBio,
                TShirtSize = teamMember.T_Shirt_Size,
                ProfilePictureUrl = teamMember.ProfilePictureUrl,
                UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                LabResultUrl = teamMember.LabResultUrl,

                PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                Shipping = "",
                ResumeUrl = teamMember.ResumeUrl,
                Date = teamMember.Date,
                EmailConfirmed = teamMember.EmailConfirmed,
                LockoutEnabled = teamMember.LockoutEnabled,
                IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                PreliminaryStatus = teamMember.PreliminaryStatus,
                OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
            };

            return vmTeamMember;
        }
        public VmTeamMember GetTeamMemberByUserIdAndTeamId(string memberUserId, int teamId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMember = teamMemberRepository.GetTeamMemberByTeamId(memberUserId, teamId);
            var vmTeamMember = new VmTeamMember
            {
                Id = teamMember.Id,
                TeamId = teamMember.TeamId,
                TaskId = teamMember.TaskId,
                MemberUserId = teamMember.MemberUserId,
                TeamName = teamMember.TeamName,
                Task = teamMember.Task,
                TeamImageUrl = teamMember.TeamImageUrl,
                RegistrationStatus = teamMember.RegistrationStatus,
                Survey = teamMember.Survey,
                MemberName = teamMember.MemberName,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                PhoneNumber = teamMember.PhoneNumber,
                WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                Identifier = teamMember.Identifier,
                Sex = teamMember.Sex,
                BirthDate = teamMember.BirthDate,
                UserName = teamMember.UserName,
                Email = teamMember.Email,
                RegisterDate = teamMember.RegisterDate,
                RoleName = teamMember.RoleName,
                RoleId = teamMember.RoleId,
                UserDefiner = teamMember.UserDefiner,
                LastSignIn = teamMember.LastSignIn,
                UniversityId = teamMember.UniversityId,
                University = teamMember.University,
                JacketSizeId = teamMember.JacketSizeId,
                JacketSize = teamMember.JacketSize ?? "",
                DietTypeId = teamMember.DietTypeId,
                DietType = teamMember.DietType ?? "",
                StreetLine1 = teamMember.StreetLine1,
                StreetLine2 = teamMember.StreetLine2,
                City = teamMember.City,
                State = teamMember.State,
                ZipCode = teamMember.ZipCode,
                EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                EmgyPersonLastName = teamMember.EmgyPersonLastName,
                EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                ShortBio = teamMember.ShortBio,
                TShirtSize = teamMember.T_Shirt_Size,
                ProfilePictureUrl = teamMember.ProfilePictureUrl,
                UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                LabResultUrl = teamMember.LabResultUrl,

                PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                Shipping = "",
                ResumeUrl = teamMember.ResumeUrl,
                Date = teamMember.Date,
                EmailConfirmed = teamMember.EmailConfirmed,
                LockoutEnabled = teamMember.LockoutEnabled,
                IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                PreliminaryStatus = teamMember.PreliminaryStatus,
                OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
            };

            return vmTeamMember;
        }
        public VmTeamMember GetTeamMemberByUserId(string memberUserId, SystemRoles roleName, int taskId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMember = teamMemberRepository.GetTeamMemberByTask(memberUserId, taskId);
            var vmTeamMember = new VmTeamMember
            {
                Id = teamMember.Id,
                TeamId = teamMember.TeamId,
                TaskId = teamMember.TaskId,
                MemberUserId = teamMember.MemberUserId,
                TeamName = teamMember.TeamName,
                Task = teamMember.Task,
                TeamImageUrl = teamMember.TeamImageUrl,
                RegistrationStatus = teamMember.RegistrationStatus,
                Survey = teamMember.Survey,
                MemberName = teamMember.MemberName,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                PhoneNumber = teamMember.PhoneNumber,
                WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                Identifier = teamMember.Identifier,
                Sex = teamMember.Sex,
                BirthDate = teamMember.BirthDate,
                UserName = teamMember.UserName,
                Email = teamMember.Email,
                RegisterDate = teamMember.RegisterDate,
                RoleName = teamMember.RoleName,
                RoleId = teamMember.RoleId,
                UserDefiner = teamMember.UserDefiner,
                LastSignIn = teamMember.LastSignIn,
                UniversityId = teamMember.UniversityId,
                University = teamMember.University,
                JacketSizeId = teamMember.JacketSizeId,
                JacketSize = teamMember.JacketSize ?? "",
                DietTypeId = teamMember.DietTypeId,
                DietType = teamMember.DietType ?? "",
                StreetLine1 = teamMember.StreetLine1,
                StreetLine2 = teamMember.StreetLine2,
                City = teamMember.City,
                State = teamMember.State,
                ZipCode = teamMember.ZipCode,
                EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                EmgyPersonLastName = teamMember.EmgyPersonLastName,
                EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                ShortBio = teamMember.ShortBio,
                TShirtSize = teamMember.T_Shirt_Size,
                ProfilePictureUrl = teamMember.ProfilePictureUrl,
                UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                LabResultUrl = teamMember.LabResultUrl,

                PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                Shipping = "",
                ResumeUrl = teamMember.ResumeUrl,
                Date = teamMember.Date,
                EmailConfirmed = teamMember.EmailConfirmed,
                LockoutEnabled = teamMember.LockoutEnabled,
                IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                PreliminaryStatus = teamMember.PreliminaryStatus,
                OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
            };

            return vmTeamMember;
        }
        public VmTeamMember GetTeamMemberByUserAndTeamId(string memberUserId, int teamId)
        {
            var teamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var teamMember = teamMemberRepository.GetTeamMember(memberUserId, teamId);
            var vmTeamMember = new VmTeamMember
            {
                Id = teamMember.Id,
                TeamId = teamMember.TeamId,
                TaskId = teamMember.TaskId,
                MemberUserId = teamMember.MemberUserId,
                TeamName = teamMember.TeamName,
                Task = teamMember.Task,
                TeamImageUrl = teamMember.TeamImageUrl,
                RegistrationStatus = teamMember.RegistrationStatus,
                Survey = teamMember.Survey,
                MemberName = teamMember.MemberName,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                PhoneNumber = teamMember.PhoneNumber,
                WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                Identifier = teamMember.Identifier,
                Sex = teamMember.Sex,
                BirthDate = teamMember.BirthDate,
                UserName = teamMember.UserName,
                Email = teamMember.Email,
                RegisterDate = teamMember.RegisterDate,
                RoleName = teamMember.RoleName,
                RoleId = teamMember.RoleId,
                UserDefiner = teamMember.UserDefiner,
                LastSignIn = teamMember.LastSignIn,
                UniversityId = teamMember.UniversityId,
                University = teamMember.University,
                JacketSizeId = teamMember.JacketSizeId,
                JacketSize = teamMember.JacketSize ?? "",
                DietTypeId = teamMember.DietTypeId,
                DietType = teamMember.DietType ?? "",
                StreetLine1 = teamMember.StreetLine1,
                StreetLine2 = teamMember.StreetLine2,
                City = teamMember.City,
                State = teamMember.State,
                ZipCode = teamMember.ZipCode,
                EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                EmgyPersonLastName = teamMember.EmgyPersonLastName,
                EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                ShortBio = teamMember.ShortBio,
                TShirtSize = teamMember.T_Shirt_Size,
                ProfilePictureUrl = teamMember.ProfilePictureUrl,
                UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                LabResultUrl = teamMember.LabResultUrl,

                PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                Shipping = "",
                ResumeUrl = teamMember.ResumeUrl,
                Date = teamMember.Date,
                EmailConfirmed = teamMember.EmailConfirmed,
                LockoutEnabled = teamMember.LockoutEnabled,
                IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                PreliminaryStatus = teamMember.PreliminaryStatus,
                OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
            };

            return vmTeamMember;
        }
        public IEnumerable<VmTeamMember> GetTeamMembersByFilter(VmTeamMember filterItem, int teamId)
        {
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            var viewFilterItem = new ViewTeamMember
            {
                Id = filterItem.Id,
                MemberName = filterItem.MemberName,
                Email = filterItem.Email,
                Survey = filterItem.Survey,
                RoleName = (filterItem.IsTeamLeader) ? "Leader" : "Student",
                RegistrationStatus = filterItem.RegistrationStatus,
            };

            viewFilterItem.RoleName = (filterItem.IsCoAdvisor) ? "CoAdvisor" : "";


            var viewteamMemberList = viewTeamMemberRepository.Select(viewFilterItem, 0, int.MaxValue, teamId);

            var vmTeamMemberList = from teamMember in viewteamMemberList
                                   select new VmTeamMember
                                   {
                                       Id = teamMember.Id,
                                       TeamId = teamMember.TeamId,
                                       TaskId = teamMember.TaskId,
                                       MemberUserId = teamMember.MemberUserId,
                                       TeamName = teamMember.TeamName,
                                       Task = teamMember.Task,
                                       TeamImageUrl = teamMember.TeamImageUrl,
                                       RegistrationStatus = teamMember.RegistrationStatus,
                                       Survey = teamMember.Survey,
                                       MemberName = teamMember.MemberName,
                                       FirstName = teamMember.FirstName,
                                       LastName = teamMember.LastName,
                                       PhoneNumber = teamMember.PhoneNumber,
                                       WorkPhoneNumber = teamMember.WorkPhoneNumber ?? "",
                                       Identifier = teamMember.Identifier,
                                       Sex = teamMember.Sex,
                                       BirthDate = teamMember.BirthDate,
                                       UserName = teamMember.UserName,
                                       Email = teamMember.Email,
                                       RegisterDate = teamMember.RegisterDate,
                                       RoleName = teamMember.RoleName,
                                       RoleId = teamMember.RoleId,
                                       UserDefiner = teamMember.UserDefiner,
                                       LastSignIn = teamMember.LastSignIn,
                                       UniversityId = teamMember.UniversityId,
                                       University = teamMember.University,
                                       JacketSizeId = teamMember.JacketSizeId,
                                       JacketSize = teamMember.JacketSize ?? "",
                                       DietTypeId = teamMember.DietTypeId,
                                       DietType = teamMember.DietType ?? "",
                                       StreetLine1 = teamMember.StreetLine1,
                                       StreetLine2 = teamMember.StreetLine2,
                                       City = teamMember.City,
                                       State = teamMember.State,
                                       ZipCode = teamMember.ZipCode,
                                       EmgyPersonFirstName = teamMember.EmgyPersonFirstName,
                                       EmgyPersonLastName = teamMember.EmgyPersonLastName,
                                       EmgyPersonRelationship = teamMember.EmgyPersonRelationship,
                                       EmgyPersonPhoneNumber = teamMember.EmgyPersonPhoneNumber,
                                       ShortBio = teamMember.ShortBio,
                                       TShirtSize = teamMember.T_Shirt_Size,
                                       ProfilePictureUrl = teamMember.ProfilePictureUrl,
                                       UniversityPictureUrl = teamMember.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                       LabResultUrl = teamMember.LabResultUrl,

                                       PreliminaryReportUrl = teamMember.PreliminaryReportUrl ?? "?CT=Stylish_not_ok.png'",
                                       OpenTaskTestPlanUrl = teamMember.OpenTaskTestPlanUrl ?? "?CT=Stylish_not_ok.png'",
                                       WrittenReportUrl = teamMember.WrittenReportUrl ?? "?CT=Stylish_not_ok.png'",

                                       Shipping = "",
                                       ResumeUrl = teamMember.ResumeUrl,
                                       Date = teamMember.Date,
                                       EmailConfirmed = teamMember.EmailConfirmed,
                                       LockoutEnabled = teamMember.LockoutEnabled,
                                       IsTeamLeader = (teamMember.RoleName == "Leader") ? true : false,
                                       IsCoAdvisor = (teamMember.RoleName == "CoAdvisor") ? true : false,
                                       PreliminaryStatus = teamMember.PreliminaryStatus,
                                       OpenTaskTestPlanStatus = teamMember.PreliminaryStatus,
                                   };
            return vmTeamMemberList;
        }

        public bool CheckOtherTeamLeaderIsExist(int teamId, string teamMemberUserId)
        {
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            return viewTeamMemberRepository.CheckOtherTeamLeaderIsExist(teamId, teamMemberUserId);
        }
        public bool CheckOtherTeamLeaderIsExist(int teamId)
        {
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();

            return viewTeamMemberRepository.CheckOtherTeamLeaderIsExist(teamId);
        }

        public int CreateTeamMember(VmTeamMember vmTeamMember)
        {
            var result = -1;
            try
            {
                var TeamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();

                var newTeamMember = new TeamMember
                {
                    Id = vmTeamMember.Id,
                    TeamId = vmTeamMember.TeamId,
                    MemberUserId = vmTeamMember.MemberUserId,
                    RegistrationStatus = vmTeamMember.RegistrationStatus,
                    Survey = vmTeamMember.Survey,
                };

                var personRepository = UnitOfWork.GetRepository<PersonRepository>();

                personRepository.CreatePerson(
                    new Person
                    {
                        UserId = vmTeamMember.MemberUserId,
                        FirstName = vmTeamMember.FirstName,
                        LastName = vmTeamMember.LastName,
                        UniversityId = vmTeamMember.UniversityId,
                        JacketSizeId = vmTeamMember.JacketSizeId,
                        DietTypeId = vmTeamMember.DietTypeId,
                    });

                TeamMemberRepository.CreateTeamMember(newTeamMember);

                UnitOfWork.Commit();

                result = newTeamMember.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateTeamMember(VmTeamMember vmTeamMember)
        {
            try
            {
                var teamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();

                var updateableTeamMember = new TeamMember
                {
                    Id = vmTeamMember.Id,
                    TeamId = vmTeamMember.TeamId,
                    MemberUserId = vmTeamMember.MemberUserId,
                    RegistrationStatus = vmTeamMember.RegistrationStatus,
                    Survey = vmTeamMember.Survey,
                };

                var personRepository = UnitOfWork.GetRepository<PersonRepository>();

                personRepository.UpdatePersonNameByUserId(new Person
                {
                    UserId = vmTeamMember.MemberUserId,
                    FirstName = vmTeamMember.FirstName,
                    LastName = vmTeamMember.LastName,
                });

                teamMemberRepository.UpdateTeamMember(updateableTeamMember);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool UpdateTeamMemberSurvey(string memberUserId, bool survey)
        {
            try
            {
                var teamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();

                teamMemberRepository.UpdateTeamMemberSurveyByUserId(memberUserId, survey);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteTeamMember(int teamMemberId)
        {
            try
            {
                var TeamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();


                TeamMemberRepository.DeleteTeamMember(teamMemberId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
    }
}
