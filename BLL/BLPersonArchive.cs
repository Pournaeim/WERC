using BLL.Base;

using Model;
using Model.ViewModels.Admin;
using Model.ViewModels.Judge;
using Model.ViewModels.Person;
using Model.ViewModels.Task;
using Model.ViewModels.User;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLPersonArchive : BLBase
    {
        public bool CreateSiteInfo(string url)
        {
            try
            {

                var SiteInfoRepository = UnitOfWork.GetRepository<SiteInfoRepository>();

                SiteInfoRepository.CreateSiteInfo(
                        new SiteInfo
                        {
                            SiteName = url + " - " + DateTime.Now
                        });

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CreatePerson(VmPerson vmPerson)
        {
            try
            {
                if (PersonIsExistByUserId(vmPerson.UserId) == false)
                {
                    var personRepository = UnitOfWork.GetRepository<PersonArchiveRepository>();

                    personRepository.CreatePerson(
                        new PersonArchive
                        {
                            UserId = vmPerson.UserId,
                            Sex = vmPerson.Sex,
                            FirstName = vmPerson.FirstName,
                            LastName = vmPerson.LastName,
                            UniversityId = vmPerson.UniversityId == 0 ? null : vmPerson.UniversityId,
                            JacketSizeId = vmPerson.JacketSizeId,
                            DietTypeId = vmPerson.DietTypeId,
                            Allergies = vmPerson.Allergies,

                            WelcomeDinner = vmPerson.WelcomeDinner,
                            LunchOnMonday = vmPerson.LunchOnMonday,
                            LunchOnTuesday = vmPerson.LunchOnTuesday,
                            ReceptionNetworkOnTuesday = vmPerson.ReceptionNetworkOnTuesday,
                            AwardBanquet = vmPerson.AwardBanquet,
                            NoneOfTheAbove = vmPerson.NoneOfTheAbove,
                            SecondaryEmail = vmPerson.SecondaryEmail,
                            Agreement = vmPerson.Agreement,
                        });
                    UnitOfWork.Commit(1);
                }
                return true;
            }
            catch (Exception ex)
            {
                var blSiteInfo = new BLPerson();
                blSiteInfo.CreateSiteInfo("Person Err: " + ex.Message + " \n " + (ex.InnerException) ?? "");

                return false;
            }
        }
        public bool PersonIsExistByUserId(string userId)
        {
            try
            {
                var personRepository = UnitOfWork.GetRepository<PersonArchiveRepository>();

                return personRepository.PersonIsExistByUserId(userId);

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public VmPerson GetPersonByUserId(string userId)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();
                var userTaskRepository = UnitOfWork.GetRepository<UserTaskRepository>();
                var personMealTypeRepository = UnitOfWork.GetRepository<PersonMealTypeRepository>();

                var person = personInRoleRepository.GetUsersById(userId);

                var taskIds = userTaskRepository.GetUserTaskIds(userId);
                var mealTypeIds = personMealTypeRepository.GetPersonMealTypeIds(person.Id);

                var vwPerson = new VmPerson
                {
                    Id = person.Id,
                    RoleId = person.RoleId,
                    SizeId = person.SizeId,
                    Sex = person.Sex,
                    UniversityId = person.UniversityId,
                    University = person.University ?? "",
                    Affiliation = person.Affiliation ?? "",
                    Abbreviation = person.Abbreviation ?? "",
                    JacketSizeId = person.JacketSizeId,
                    JacketSize = person.JacketSize ?? "",
                    DietTypeId = person.DietTypeId,
                    DietType = person.DietType ?? "",
                    Allergies = person.Allergies ?? "",
                    UserId = person.UserId,
                    ProfilePictureUrl = person.ProfilePictureUrl ?? "",
                    UniversityPictureUrl = person.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                    ResumeUrl = person.ResumeUrl ?? "",
                    FirstName = person.FirstName ?? "",
                    LastName = person.LastName ?? "",
                    RoleName = person.RoleName ?? "",
                    Email = person.Email,
                    SecondaryEmail = person.SecondaryEmail,
                    T_Shirt_Size = person.T_Shirt_Size ?? "",
                    PhoneNumber = person.PhoneNumber ?? "",
                    WorkPhoneNumber = person.WorkPhoneNumber ?? "",
                    StreetLine1 = person.StreetLine1 ?? "",
                    StreetLine2 = person.StreetLine2 ?? "",
                    City = person.City ?? "",
                    State = person.State ?? "",
                    ZipCode = person.ZipCode ?? "",
                    EmgyPersonFirstName = person.EmgyPersonFirstName ?? "",
                    EmgyPersonLastName = person.EmgyPersonLastName ?? "",
                    EmgyPersonPhoneNumber = person.EmgyPersonPhoneNumber ?? "",
                    EmgyPersonRelationship = person.EmgyPersonRelationship ?? "",
                    ShortBio = person.ShortBio ?? "",

                    WelcomeDinner = person.WelcomeDinner,
                    LunchOnMonday = person.LunchOnMonday,
                    LunchOnTuesday = person.LunchOnTuesday,
                    ReceptionNetworkOnTuesday = person.ReceptionNetworkOnTuesday,
                    AwardBanquet = person.AwardBanquet,
                    NoneOfTheAbove = person.NoneOfTheAbove,
                    Agreement = person.Agreement ?? false,
                    TaskIds = taskIds,
                    ClientTaskIds = string.Join(",", taskIds),
                    ClientMealTypeIds = string.Join(",", mealTypeIds),
                    Major = person.Major,
                    Minor = person.Minor,

                    YearClassificationId = person.YearClassificationId,
                    YearClassification = person.YearClassification ?? "",


                };

                if (person.GoalsAfterGraduationIds != null && person.GoalsAfterGraduationIds.Count() > 0)
                {
                    vwPerson.GoalsAfterGraduationIds = person.GoalsAfterGraduationIds.Split(',').Select(int.Parse).ToArray();
                    vwPerson.ClientGoalsAfterGraduationIds = person.GoalsAfterGraduationIds;
                }

                if (person.EthnicityIds != null && person.EthnicityIds.Count() > 0)
                {
                    vwPerson.EthnicityIds = person.EthnicityIds.Split(',').Select(int.Parse).ToArray();
                    vwPerson.ClientEthnicityIds = person.EthnicityIds;
                }
                return vwPerson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public VmPerson GetPersonByEmail(string email)
        {
            try
            {
                var aspNetUsersArchiveRepository = UnitOfWork.GetRepository<AspNetUsersArchiveRepository>();
                var userTaskRepository = UnitOfWork.GetRepository<UserTaskRepository>();
                var personMealTypeRepository = UnitOfWork.GetRepository<PersonMealTypeRepository>();
                var personArchiveRepository = UnitOfWork.GetRepository<PersonArchiveRepository>();

                var user = aspNetUsersArchiveRepository.GetAspNetUsersArchiveByEmail(email);

                if (user == null)
                {
                    return null;
                }
                var person = personArchiveRepository.GetPersonByUserId(user.Id);
                var taskIds = userTaskRepository.GetUserTaskIds(person.UserId);
                var mealTypeIds = personMealTypeRepository.GetPersonMealTypeIds(person.Id);

                var vwPerson = new VmPerson
                {
                    Id = person.Id,
                    SizeId = person.SizeId,
                    Sex = person.Sex,
                    UniversityId = person.UniversityId,
                    Affiliation = person.Affiliation ?? "",
                    Abbreviation = person.Abbreviation ?? "",
                    JacketSizeId = person.JacketSizeId,
                    DietTypeId = person.DietTypeId,
                    Allergies = person.Allergies ?? "",
                    UserId = person.UserId,
                    ProfilePictureUrl = person.ProfilePictureUrl ?? "",
                    ResumeUrl = person.ResumeUrl ?? "",
                    FirstName = person.FirstName ?? "",
                    LastName = person.LastName ?? "",
                    Email = user.Email,
                    SecondaryEmail = person.SecondaryEmail,
                    PhoneNumber = user.PhoneNumber ?? "",
                    WorkPhoneNumber = user.WorkPhoneNumber ?? "",
                    StreetLine1 = person.StreetLine1 ?? "",
                    StreetLine2 = person.StreetLine2 ?? "",
                    City = person.City ?? "",
                    State = person.State ?? "",
                    ZipCode = person.ZipCode ?? "",
                    EmgyPersonFirstName = person.EmgyPersonFirstName ?? "",
                    EmgyPersonLastName = person.EmgyPersonLastName ?? "",
                    EmgyPersonPhoneNumber = person.EmgyPersonPhoneNumber ?? "",
                    EmgyPersonRelationship = person.EmgyPersonRelationship ?? "",
                    ShortBio = person.ShortBio ?? "",

                    WelcomeDinner = person.WelcomeDinner,
                    LunchOnMonday = person.LunchOnMonday,
                    LunchOnTuesday = person.LunchOnTuesday,
                    ReceptionNetworkOnTuesday = person.ReceptionNetworkOnTuesday,
                    AwardBanquet = person.AwardBanquet,
                    NoneOfTheAbove = person.NoneOfTheAbove,
                    Agreement = person.Agreement ?? false,
                    TaskIds = taskIds,
                    ClientTaskIds = string.Join(",", taskIds),
                    ClientMealTypeIds = string.Join(",", mealTypeIds),
                    Major = person.Major,
                    Minor = person.Minor,

                    YearClassificationId = person.YearClassificationId,


                };

                if (person.GoalsAfterGraduationIds != null && person.GoalsAfterGraduationIds.Count() > 0)
                {
                    vwPerson.GoalsAfterGraduationIds = person.GoalsAfterGraduationIds.Split(',').Select(int.Parse).ToArray();
                    vwPerson.ClientGoalsAfterGraduationIds = person.GoalsAfterGraduationIds;
                }

                if (person.EthnicityIds != null && person.EthnicityIds.Count() > 0)
                {
                    vwPerson.EthnicityIds = person.EthnicityIds.Split(',').Select(int.Parse).ToArray();
                    vwPerson.ClientEthnicityIds = person.EthnicityIds;
                }
                return vwPerson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VmPerson GetPersonProfileByUserId(string userId)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();
                var tempUserTaskRepository = UnitOfWork.GetRepository<TempUserTaskRepository>();
                var personMealTypeRepository = UnitOfWork.GetRepository<PersonMealTypeRepository>();

                var person = personInRoleRepository.GetUsersById(userId);

                var taskIds = tempUserTaskRepository.GetTempUserTaskIds(userId);
                var mealTypeIds = personMealTypeRepository.GetPersonMealTypeIds(person.Id);

                var vwPerson = new VmPerson
                {
                    Id = person.Id,
                    RoleId = person.RoleId,
                    SizeId = person.SizeId,
                    Sex = person.Sex,
                    UniversityId = person.UniversityId,
                    University = person.University ?? "",
                    Affiliation = person.Affiliation ?? "",
                    Abbreviation = person.Abbreviation ?? "",
                    JacketSizeId = person.JacketSizeId,
                    JacketSize = person.JacketSize ?? "",
                    DietTypeId = person.DietTypeId,
                    DietType = person.DietType ?? "",
                    Allergies = person.Allergies ?? "",
                    UserId = person.UserId,
                    ProfilePictureUrl = person.ProfilePictureUrl ?? "",
                    UniversityPictureUrl = person.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                    ResumeUrl = person.ResumeUrl ?? "",
                    FirstName = person.FirstName ?? "",
                    LastName = person.LastName ?? "",
                    RoleName = person.RoleName ?? "",
                    Email = person.Email,
                    SecondaryEmail = person.SecondaryEmail,
                    T_Shirt_Size = person.T_Shirt_Size ?? "",
                    PhoneNumber = person.PhoneNumber ?? "",
                    WorkPhoneNumber = person.WorkPhoneNumber ?? "",
                    StreetLine1 = person.StreetLine1 ?? "",
                    StreetLine2 = person.StreetLine2 ?? "",
                    City = person.City ?? "",
                    State = person.State ?? "",
                    ZipCode = person.ZipCode ?? "",
                    EmgyPersonFirstName = person.EmgyPersonFirstName ?? "",
                    EmgyPersonLastName = person.EmgyPersonLastName ?? "",
                    EmgyPersonPhoneNumber = person.EmgyPersonPhoneNumber ?? "",
                    EmgyPersonRelationship = person.EmgyPersonRelationship ?? "",
                    ShortBio = person.ShortBio ?? "",

                    WelcomeDinner = person.WelcomeDinner,
                    LunchOnMonday = person.LunchOnMonday,
                    LunchOnTuesday = person.LunchOnTuesday,
                    ReceptionNetworkOnTuesday = person.ReceptionNetworkOnTuesday,
                    AwardBanquet = person.AwardBanquet,
                    NoneOfTheAbove = person.NoneOfTheAbove,
                    Agreement = person.Agreement ?? false,
                    TaskIds = taskIds,
                    ClientMealTypeIds = string.Join(",", mealTypeIds),
                    MealTypeIds = mealTypeIds,
                    ClientTaskIds = string.Join(",", taskIds),
                    Major = person.Major,
                    Minor = person.Minor,

                    YearClassificationId = person.YearClassificationId,
                    YearClassification = person.YearClassification ?? "",


                };

                if (person.GoalsAfterGraduationIds != null && person.GoalsAfterGraduationIds.Count() > 0)
                {
                    vwPerson.GoalsAfterGraduationIds = person.GoalsAfterGraduationIds.Split(',').Select(int.Parse).ToArray();
                    vwPerson.ClientGoalsAfterGraduationIds = person.GoalsAfterGraduationIds;
                }

                if (person.EthnicityIds != null && person.EthnicityIds.Count() > 0)
                {
                    vwPerson.EthnicityIds = person.EthnicityIds.Split(',').Select(int.Parse).ToArray();
                    vwPerson.ClientEthnicityIds = person.EthnicityIds;
                }
                return vwPerson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string[] GetEmailsByUserIds(string[] userIds)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

                return personInRoleRepository.GetEmailsByUserIds(userIds);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<ViewPersonInRole> GetPersonsByUserIds(string[] userIds)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

                return personInRoleRepository.GetPersonsByUserIds(userIds);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<ViewPersonInRole> GetPersonsByUserIdsAndRole(string[] userIds, SystemRoles roleName)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

                return personInRoleRepository.GetPersonsByUserIdsAndRole(userIds, roleName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool UpdatePerson(VmPerson vmPerson)
        {
            try
            {
                var personRepository = UnitOfWork.GetRepository<PersonArchiveRepository>();
                var dietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();

                var person = new PersonArchive
                {
                    Id = vmPerson.Id,
                    SizeId = vmPerson.SizeId,
                    Sex = vmPerson.Sex,
                    UniversityId = vmPerson.UniversityId,
                    JacketSizeId = vmPerson.JacketSizeId,
                    UserId = vmPerson.UserId,
                    ResumeUrl = vmPerson.ResumeUrl,
                    FirstName = vmPerson.FirstName,
                    LastName = vmPerson.LastName,
                    StreetLine1 = vmPerson.StreetLine1,
                    StreetLine2 = vmPerson.StreetLine2,
                    City = vmPerson.City,
                    State = vmPerson.State,
                    ZipCode = vmPerson.ZipCode,
                    EmgyPersonFirstName = vmPerson.EmgyPersonFirstName,
                    EmgyPersonLastName = vmPerson.EmgyPersonLastName,
                    EmgyPersonPhoneNumber = vmPerson.EmgyPersonPhoneNumber,
                    EmgyPersonRelationship = vmPerson.EmgyPersonRelationship,
                    ShortBio = vmPerson.ShortBio,

                    WelcomeDinner = vmPerson.WelcomeDinner,
                    LunchOnMonday = vmPerson.LunchOnMonday,
                    LunchOnTuesday = vmPerson.LunchOnTuesday,
                    ReceptionNetworkOnTuesday = vmPerson.ReceptionNetworkOnTuesday,
                    AwardBanquet = vmPerson.AwardBanquet,
                    NoneOfTheAbove = vmPerson.NoneOfTheAbove,
                    SecondaryEmail = vmPerson.SecondaryEmail,
                    Agreement = vmPerson.Agreement,
                    Abbreviation = vmPerson.Abbreviation,
                    Affiliation = vmPerson.Affiliation,
                    Major = vmPerson.Major,
                    Minor = vmPerson.Minor,
                    YearClassificationId = vmPerson.YearClassificationId,
                };

                if (vmPerson.DietTypeId == 5)
                {
                    var dietType = new DietType
                    {
                        Id = dietTypeRepository.GetDietTypeNewId(),
                        Name = vmPerson.DietType,
                        Display = true,
                    };

                    person.DietTypeId = dietType.Id;

                    dietTypeRepository.CreateDietType(dietType);
                }
                else
                {
                    person.DietTypeId = vmPerson.DietTypeId;
                }

                person.Allergies = vmPerson.Allergies;

                var oldPerson = GetPersonByUserId(vmPerson.CurrentUserId);

                if (vmPerson.RoleId == "f3b628a1-ab7d-48dc-811d-d509e645d7f0" || vmPerson.RoleId == "291d6069-44a3-4960-86d3-b91bda430e71") // Student or Leader
                {
                    person.GoalsAfterGraduationIds = string.Join(",", vmPerson.GoalsAfterGraduationIds);
                    person.EthnicityIds = string.Join(",", vmPerson.EthnicityIds);
                }

                personRepository.UpdatePerson(person);

                //Do not delete its for next year
                if (vmPerson.RoleId == "4d7951d8-8eda-4452-8ff1-dfc9076d8bbe") //Judge
                {
                    var tempUserTaskRepository = UnitOfWork.GetRepository<TempUserTaskRepository>();

                    tempUserTaskRepository.DeleteTempUserTasks(vmPerson.UserId);
                    tempUserTaskRepository.CreateTasksUser(vmPerson.UserId, vmPerson.TaskIds);
                }
                if (vmPerson.MealTypeIds != null)
                {
                    var personMealTypeRepository = UnitOfWork.GetRepository<PersonMealTypeRepository>();

                    personMealTypeRepository.DeletePersonMealTypes(vmPerson.Id);
                    personMealTypeRepository.CreatePersonMealTypes(vmPerson.Id, vmPerson.MealTypeIds);

                }

                var teamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();

                var blTeam = new BLTeam();
                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

                var updateableTeamMember = new TeamMember
                {

                    MemberUserId = vmPerson.UserId,
                    RegistrationStatus = true,
                };

                teamMemberRepository.UpdateTeamMemberRegistrationStatusByUserId(updateableTeamMember);

                if (!string.IsNullOrWhiteSpace(oldPerson.Abbreviation) && oldPerson.Abbreviation != vmPerson.Abbreviation)
                {
                    var teams = blTeam.GetAdvisorTeams(vmPerson.CurrentUserId);
                    foreach (var item in teams)
                    {
                        item.Name = item.Name.Replace(oldPerson.Abbreviation, vmPerson.Abbreviation);
                        teamRepository.UpdateTeamName(item.Id, item.Name);
                    }

                }

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public VmUser GetUserInfoFromOldDatabases(string eMail)
        {
            VmUser oldUser = new VmUser();

            return oldUser;
        }

        public bool ReplaceAdvisor(string userId, string firstName, string lastName, string email)
        {
            try
            {
                var personRepository = UnitOfWork.GetRepository<PersonArchiveRepository>();
                var userRepository = UnitOfWork.GetRepository<UserRepository>();

                userRepository.UpdateEmailAndUserName(userId, email);
                personRepository.ReplaceAdvisor(userId, firstName, lastName, email);

                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UploadProfileImage(string userId, string profilePictureUrl)
        {
            try
            {

                var personRepository = UnitOfWork.GetRepository<PersonArchiveRepository>();
                personRepository.UpdateProfileImage(userId, profilePictureUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UploadResume(string userId, string resumeUrl)
        {
            try
            {

                var personRepository = UnitOfWork.GetRepository<PersonArchiveRepository>();
                personRepository.UpdateResume(userId, resumeUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<VmApprovalReject> GetAllPersons()
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.GetAllpersons();

            var vmApprovalRejectList = from user in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = user.UserId,
                                           Sex = user.Sex,
                                           University = user.University,
                                           Abbreviation = user.Abbreviation,
                                           UniversityPictureUrl = user.UniversityPictureUrl,
                                           FirstName = user.FirstName,
                                           LastName = user.LastName,
                                           Name = user.Name,
                                           RoleName = user.RoleName,
                                           Email = user.Email,
                                           EmailConfirmed = user.EmailConfirmed,
                                           PhoneNumber = user.PhoneNumber,
                                           UserName = user.UserName,
                                           LockoutEnabled = user.LockoutEnabled,
                                           ProfilePictureUrl = user.ProfilePictureUrl,
                                           Approval = (user.EmailConfirmed == false && user.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (user.EmailConfirmed == false && user.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = user.WelcomeDinner,
                                           LunchOnMonday = user.LunchOnMonday,
                                           LunchOnTuesday = user.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = user.ReceptionNetworkOnTuesday,
                                           AwardBanquet = user.AwardBanquet,
                                           NoneOfTheAbove = user.NoneOfTheAbove,
                                           Agreement = user.Agreement ?? false,
                                       };

            return vmApprovalRejectList;

        }
        public IEnumerable<VmApprovalReject> GetUsersByRoleNames(string[] roleNames)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.GetUsersByRoleNames(roleNames);

            var vmApprovalRejectList = from user in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = user.UserId,
                                           Sex = user.Sex,
                                           University = user.University,
                                           Abbreviation = user.Abbreviation,
                                           UniversityPictureUrl = user.UniversityPictureUrl,
                                           FirstName = user.FirstName,
                                           LastName = user.LastName,
                                           Name = user.Name,
                                           RoleName = user.RoleName,
                                           Email = user.Email,
                                           EmailConfirmed = user.EmailConfirmed,
                                           PhoneNumber = user.PhoneNumber,
                                           UserName = user.UserName,
                                           LockoutEnabled = user.LockoutEnabled,
                                           ProfilePictureUrl = user.ProfilePictureUrl,
                                           Approval = (user.EmailConfirmed == false && user.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (user.EmailConfirmed == false && user.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = user.WelcomeDinner,
                                           LunchOnMonday = user.LunchOnMonday,
                                           LunchOnTuesday = user.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = user.ReceptionNetworkOnTuesday,
                                           AwardBanquet = user.AwardBanquet,
                                           NoneOfTheAbove = user.NoneOfTheAbove,
                                           Agreement = user.Agreement ?? false,
                                       };

            return vmApprovalRejectList;

        }
        public IEnumerable<VmApprovalReject> GetUsersByEmails(string[] emails)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.GetUsersByEmails(emails);

            var vmApprovalRejectList = from user in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = user.UserId,
                                           Sex = user.Sex,
                                           University = user.University,
                                           Abbreviation = user.Abbreviation,
                                           UniversityPictureUrl = user.UniversityPictureUrl,
                                           FirstName = user.FirstName,
                                           LastName = user.LastName,
                                           Name = user.Name,
                                           RoleName = user.RoleName,
                                           Email = user.Email,
                                           EmailConfirmed = user.EmailConfirmed,
                                           PhoneNumber = user.PhoneNumber,
                                           UserName = user.UserName,
                                           LockoutEnabled = user.LockoutEnabled,
                                           ProfilePictureUrl = user.ProfilePictureUrl,
                                           Approval = (user.EmailConfirmed == false && user.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (user.EmailConfirmed == false && user.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = user.WelcomeDinner,
                                           LunchOnMonday = user.LunchOnMonday,
                                           LunchOnTuesday = user.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = user.ReceptionNetworkOnTuesday,
                                           AwardBanquet = user.AwardBanquet,
                                           NoneOfTheAbove = user.NoneOfTheAbove,
                                           Agreement = user.Agreement ?? false,
                                       };

            return vmApprovalRejectList;

        }
        public IEnumerable<VmApprovalReject> GetUsersByFilterAndRoleNames(string[] roleNames, VmApprovalReject filterItem = null)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.Select(roleNames, filterItem, 0, int.MaxValue);

            var vmApprovalRejectList = from user in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = user.UserId,
                                           Sex = user.Sex,
                                           University = user.University,
                                           FirstName = user.FirstName,
                                           LastName = user.LastName,
                                           Name = user.Name,
                                           RoleName = user.RoleName,
                                           Email = user.Email,
                                           EmailConfirmed = user.EmailConfirmed,
                                           PhoneNumber = user.PhoneNumber,
                                           UserName = user.UserName,
                                           LockoutEnabled = user.LockoutEnabled,
                                           ProfilePictureUrl = user.ProfilePictureUrl,
                                           Approval = (user.EmailConfirmed == false && user.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (user.EmailConfirmed == false && user.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = user.WelcomeDinner,
                                           LunchOnMonday = user.LunchOnMonday,
                                           LunchOnTuesday = user.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = user.ReceptionNetworkOnTuesday,
                                           AwardBanquet = user.AwardBanquet,
                                           NoneOfTheAbove = user.NoneOfTheAbove,
                                           Agreement = user.Agreement ?? false,
                                       };

            return vmApprovalRejectList.OrderBy(p => p.Approval);

        }
        public IEnumerable<VmPerson> GetUsersByFilter(VmPerson filterItem = null)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.Select(filterItem, 0, int.MaxValue);

            var vmApprovalRejectList = from person in usersList
                                       select new VmPerson
                                       {
                                           Id = person.Id,
                                           RoleId = person.RoleId,
                                           SizeId = person.SizeId,
                                           Sex = person.Sex,
                                           UniversityId = person.UniversityId,
                                           University = person.University ?? "",
                                           Affiliation = person.Affiliation ?? "",
                                           Abbreviation = person.Abbreviation ?? "",
                                           JacketSizeId = person.JacketSizeId,
                                           JacketSize = person.JacketSize ?? "",
                                           DietTypeId = person.DietTypeId,
                                           DietType = person.DietType ?? "",
                                           Allergies = person.Allergies ?? "",
                                           UserId = person.UserId,
                                           ProfilePictureUrl = person.ProfilePictureUrl ?? "",
                                           UniversityPictureUrl = person.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                           ResumeUrl = person.ResumeUrl ?? "",
                                           FirstName = person.FirstName ?? "",
                                           LastName = person.LastName ?? "",
                                           RoleName = person.RoleName ?? "",
                                           Email = person.Email,
                                           SecondaryEmail = person.SecondaryEmail,
                                           T_Shirt_Size = person.T_Shirt_Size ?? "",
                                           PhoneNumber = person.PhoneNumber ?? "",
                                           WorkPhoneNumber = person.WorkPhoneNumber ?? "",
                                           StreetLine1 = person.StreetLine1 ?? "",
                                           StreetLine2 = person.StreetLine2 ?? "",
                                           City = person.City ?? "",
                                           State = person.State ?? "",
                                           ZipCode = person.ZipCode ?? "",
                                           EmgyPersonFirstName = person.EmgyPersonFirstName ?? "",
                                           EmgyPersonLastName = person.EmgyPersonLastName ?? "",
                                           EmgyPersonPhoneNumber = person.EmgyPersonPhoneNumber ?? "",
                                           EmgyPersonRelationship = person.EmgyPersonRelationship ?? "",
                                           ShortBio = person.ShortBio ?? "",

                                           WelcomeDinner = person.WelcomeDinner,
                                           LunchOnMonday = person.LunchOnMonday,
                                           LunchOnTuesday = person.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = person.ReceptionNetworkOnTuesday,
                                           AwardBanquet = person.AwardBanquet,
                                           NoneOfTheAbove = person.NoneOfTheAbove,
                                           Agreement = person.Agreement ?? false,
                                       };

            return vmApprovalRejectList.OrderBy(p => p.LastName);

        }
        public IEnumerable<VmPerson> GetUsersFulInfoByFilter(VmPerson filterItem = null)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();
            var viewTeamMemberRepository = UnitOfWork.GetRepository<ViewTeamMemberRepository>();
            var teamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();
            var viewJudgeFullInfoRepository = UnitOfWork.GetRepository<ViewJudgeFullInfoRepository>();
            var viewTeamFullInfoRepository = UnitOfWork.GetRepository<ViewTeamFullInfoRepository>();
            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

            var usersList = viewPersonInRoleRepository.Select(filterItem, 0, int.MaxValue);

            var vmPersons = (from person in usersList
                             select new VmPerson
                             {
                                 Id = person.Id,
                                 RoleId = person.RoleId,
                                 SizeId = person.SizeId,
                                 Sex = person.Sex,
                                 UniversityId = person.UniversityId,
                                 University = person.University ?? "",
                                 Affiliation = person.Affiliation ?? "",
                                 Abbreviation = person.Abbreviation ?? "",
                                 JacketSizeId = person.JacketSizeId,
                                 JacketSize = person.JacketSize ?? "",
                                 DietTypeId = person.DietTypeId,
                                 DietType = person.DietType ?? "",
                                 Allergies = person.Allergies ?? "",
                                 UserId = person.UserId,
                                 ProfilePictureUrl = person.ProfilePictureUrl ?? "",
                                 UniversityPictureUrl = person.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                 ResumeUrl = person.ResumeUrl ?? "",
                                 FirstName = person.FirstName ?? "",
                                 LastName = person.LastName ?? "",
                                 RoleName = person.RoleName ?? "",
                                 Email = person.Email,
                                 SecondaryEmail = person.SecondaryEmail,
                                 T_Shirt_Size = person.T_Shirt_Size ?? "",
                                 PhoneNumber = person.PhoneNumber ?? "",
                                 WorkPhoneNumber = person.WorkPhoneNumber ?? "",
                                 StreetLine1 = person.StreetLine1 ?? "",
                                 StreetLine2 = person.StreetLine2 ?? "",
                                 City = person.City ?? "",
                                 State = person.State ?? "",
                                 ZipCode = person.ZipCode ?? "",
                                 EmgyPersonFirstName = person.EmgyPersonFirstName ?? "",
                                 EmgyPersonLastName = person.EmgyPersonLastName ?? "",
                                 EmgyPersonPhoneNumber = person.EmgyPersonPhoneNumber ?? "",
                                 EmgyPersonRelationship = person.EmgyPersonRelationship ?? "",
                                 ShortBio = person.ShortBio ?? "",

                                 WelcomeDinner = person.WelcomeDinner,
                                 LunchOnMonday = person.LunchOnMonday,
                                 LunchOnTuesday = person.LunchOnTuesday,
                                 ReceptionNetworkOnTuesday = person.ReceptionNetworkOnTuesday,
                                 AwardBanquet = person.AwardBanquet,
                                 NoneOfTheAbove = person.NoneOfTheAbove,
                                 Agreement = person.Agreement ?? false,
                                 Tasks = "",
                                 TeamName = "",

                             }).ToList();

            var teamMemberIds = (from p in vmPersons select p.UserId).ToArray();
            var teamMemebersInfo = viewTeamMemberRepository.GetTeamMembers(teamMemberIds);
            var teamMemebersTableInfo = teamMemberRepository.GetTeamMembers(teamMemberIds);

            var advisors = teamMemebersInfo.Where(t => t.RoleName.ToLower() == "advisor");

            foreach (var person in vmPersons)
            {

                var teamMember = teamMemebersInfo.Where(t => t.MemberUserId == person.UserId).FirstOrDefault();
                if (teamMember != null)
                {
                    person.TeamName = teamMember.TeamName;
                    person.Tasks = teamMember.Task ?? "";
                    var result = advisors.Where(a => a.TeamId == teamMember.TeamId);

                    if (result != null && result.Count() > 0)
                    {
                        person.University = result.First().University;
                    }
                }
            }


            var checkTeamFullInfos = teamRepository.GetAllTeam();

            foreach (var team in checkTeamFullInfos)
            {
                if (team.Deactivate == true)
                {
                    var tempTeamMembers = teamMemebersTableInfo.Where(t => t.TeamId == team.Id).Select(t => t.MemberUserId);
                    foreach (var person in tempTeamMembers)
                    {
                        var result = vmPersons.Where(p => p.UserId == person);
                        if (result.Count() > 0)
                        {
                            var deletePerson = vmPersons.Where(p => p.UserId == person).First();
                            vmPersons.Remove(deletePerson);
                        }
                    }

                }
            }
            var teamFullInfos = viewTeamFullInfoRepository.GetMemberUserTeamFullInfos(teamMemberIds);

            foreach (var person in vmPersons)
            {
                var teamFullInfo = teamFullInfos.Where(t => t.MemberUserId == person.UserId);

                if (teamFullInfo != null && teamFullInfo.Count() > 0)
                {
                    person.TeamName = string.Join(",<br/>", teamFullInfo.Select(t => t.Name));
                    person.Tasks = string.Join(",<br/>", teamFullInfo.Select(t => t.TaskName));
                }
            }

            var judgeFullInfos = viewJudgeFullInfoRepository.GetMemberUserJudgeFullInfos(teamMemberIds);

            foreach (var person in vmPersons)
            {
                var judgeFullInfo = judgeFullInfos.Where(t => t.UserId == person.UserId).FirstOrDefault();
                if (judgeFullInfo != null && judgeFullInfo.Tasks != null)
                {
                    person.Tasks = string.Join(",<br/>", judgeFullInfo.Tasks.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }

            if (filterItem.Tasks != null)
            {
                vmPersons = vmPersons.Where(t => t.Tasks.ToLower().Contains(filterItem.Tasks.ToLower())).ToList();
            }

            if (filterItem.TeamName != null)
            {
                vmPersons = vmPersons.Where(t => t.TeamName.ToLower().Contains(filterItem.TeamName.ToLower())).ToList();
            }

            return vmPersons.OrderBy(p => p.LastName);

        }
        public IEnumerable<VmJudgeFullInfo> GetJudgeFullInfoByFilter(VmJudgeFullInfo filterItem)
        {
            var viewJudgeFullInfoRepository = UnitOfWork.GetRepository<ViewJudgeFullInfoRepository>();

            var viewFilterItem = new ViewJudgeFullInfo
            {
                FirstName = filterItem.FirstName,
                LastName = filterItem.LastName,
                Email = filterItem.Email,
                Tasks = filterItem.Tasks,
                Teams = filterItem.Teams,
            };

            var viewjudgeFullInfoList = viewJudgeFullInfoRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmJudgeFullInfoList = (from judgeFullInfo in viewjudgeFullInfoList
                                       select new VmJudgeFullInfo
                                       {
                                           Id = judgeFullInfo.Id,
                                           PhoneNumber = judgeFullInfo.PhoneNumber,
                                           WorkPhoneNumber = judgeFullInfo.WorkPhoneNumber ?? "",
                                           Sex = judgeFullInfo.Sex,
                                           UserId = judgeFullInfo.UserId,
                                           UserName = judgeFullInfo.UserName,
                                           Email = judgeFullInfo.Email,
                                           RoleName = judgeFullInfo.RoleName,
                                           RoleId = judgeFullInfo.RoleId,
                                           DietTypeId = judgeFullInfo.DietTypeId,
                                           DietType = judgeFullInfo.DietType ?? "",
                                           StreetLine1 = judgeFullInfo.StreetLine1,
                                           StreetLine2 = judgeFullInfo.StreetLine2,
                                           City = judgeFullInfo.City,
                                           State = judgeFullInfo.State,
                                           ZipCode = judgeFullInfo.ZipCode,
                                           ShortBio = judgeFullInfo.ShortBio,
                                           T_Shirt_Size = judgeFullInfo.T_Shirt_Size,
                                           ProfilePictureUrl = judgeFullInfo.ProfilePictureUrl,
                                           ResumeUrl = judgeFullInfo.ResumeUrl,
                                           EmailConfirmed = judgeFullInfo.EmailConfirmed,
                                           FirstName = judgeFullInfo.FirstName,
                                           LastName = judgeFullInfo.LastName,
                                           SizeId = judgeFullInfo.SizeId,
                                           Tasks = judgeFullInfo.Tasks,
                                           Teams = judgeFullInfo.Teams,
                                           Agreement = judgeFullInfo.Agreement ?? false,

                                       }).ToList();

            var blTempUserTask = new BLTempUserTask();
            var tempUserTaskList = blTempUserTask.GetAlluserTasks().ToList();

            foreach (var item in vmJudgeFullInfoList)
            {
                item.PreferredTasks = string.Join(",", (from t in tempUserTaskList
                                                        where t.UserId == item.UserId
                                                        select t.TaskName).ToArray());
            }
            return vmJudgeFullInfoList;
        }

    }
}