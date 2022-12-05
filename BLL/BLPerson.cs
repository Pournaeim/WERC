using BLL.Base;

using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.Admin;
using Model.ViewModels.HouseholdEducation;
using Model.ViewModels.Judge;
using Model.ViewModels.LevelOfConfidence;
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
    public class BLPerson : BLBase
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
        public bool CreatePerson(VmPerson personData)
        {
            try
            {
                if (PersonIsExistByUserId(personData.UserId) == false)
                {
                    var personRepository = UnitOfWork.GetRepository<PersonRepository>();

                    personRepository.CreatePerson(
                        new Person
                        {
                            UserId = personData.UserId,
                            Sex = personData.Sex,
                            FirstName = personData.FirstName,
                            LastName = personData.LastName,
                            UniversityId = personData.UniversityId == 0 ? null : personData.UniversityId,
                            JacketSizeId = personData.JacketSizeId,
                            DietTypeId = personData.DietTypeId,
                            Allergies = personData.Allergies,

                            WelcomeDinner = personData.WelcomeDinner,
                            LunchOnMonday = personData.LunchOnMonday,
                            LunchOnTuesday = personData.LunchOnTuesday,
                            ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                            AwardBanquet = personData.AwardBanquet,
                            NoneOfTheAbove = personData.NoneOfTheAbove,
                            SecondaryEmail = personData.SecondaryEmail,
                            Agreement = personData.Agreement,
                            IEEEMembership = personData.IEEEMembership,
                            Disability = personData.Disability,
                            Accommodation = personData.Accommodation,
                            FirstGeneration = personData.FirstGeneration,
                            IndividualDisadvantaged = personData.IndividualDisadvantaged,
                            LevelOfConfidenceId = personData.LevelOfConfidenceId,
                            HouseholdEducationId = personData.HouseholdEducationId,



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
        public bool CreatePersonWithFullInfo(VmPerson personData)
        {
            try
            {
                if (PersonIsExistByUserId(personData.UserId) == false)
                {
                    var personRepository = UnitOfWork.GetRepository<PersonRepository>();

                    personRepository.CreatePerson(
                        new Person
                        {
                            UserId = personData.UserId,
                            Sex = personData.Sex,
                            FirstName = personData.FirstName,
                            LastName = personData.LastName,
                            UniversityId = personData.UniversityId == 0 ? null : personData.UniversityId,
                            JacketSizeId = personData.JacketSizeId,
                            DietTypeId = personData.DietTypeId,
                            Allergies = personData.Allergies,

                            WelcomeDinner = personData.WelcomeDinner,
                            LunchOnMonday = personData.LunchOnMonday,
                            LunchOnTuesday = personData.LunchOnTuesday,
                            ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                            AwardBanquet = personData.AwardBanquet,
                            NoneOfTheAbove = personData.NoneOfTheAbove,
                            SecondaryEmail = personData.SecondaryEmail,
                            Agreement = personData.Agreement,
                            IEEEMembership = personData.IEEEMembership,
                            Disability = personData.Disability,
                            Accommodation = personData.Accommodation,
                            FirstGeneration = personData.FirstGeneration,
                            IndividualDisadvantaged = personData.IndividualDisadvantaged,
                            LevelOfConfidenceId = personData.LevelOfConfidenceId,
                            HouseholdEducationId = personData.HouseholdEducationId,
                            Abbreviation = personData.Abbreviation,
                            Affiliation = personData.Affiliation,
                            EmgyPersonFirstName = personData.EmgyPersonFirstName,
                            EmgyPersonLastName = personData.EmgyPersonLastName,
                            EmgyPersonPhoneNumber = personData.EmgyPersonPhoneNumber,
                            EmgyPersonRelationship = personData.EmgyPersonRelationship,
                            ShortBio = personData.ShortBio,
                            ProfilePictureUrl = personData.ProfilePictureUrl,
                            ResumeUrl = personData.ResumeUrl,
                            SizeId = personData.SizeId,
                            State = personData.State,
                            StreetLine1 = personData.StreetLine1,
                            StreetLine2 = personData.StreetLine2,
                            YearClassificationId = personData.YearClassificationId,
                            ZipCode = personData.ZipCode,
                            Minor = personData.Minor,
                            Major = personData.Major,
                            GoalsAfterGraduationIds = personData.GoalsAfterGraduations,
                            GoalsAfterGraduationSite = personData.GoalsAfterGraduationSite,
                            OtherGoals = personData.OtherGoals,
                            City = personData.City,
                            EthnicityIds = personData.ClientEthnicityIds,


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
                var personRepository = UnitOfWork.GetRepository<PersonRepository>();

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
                var levelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();
                var householdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();
                var personMealTypeRepository = UnitOfWork.GetRepository<PersonMealTypeRepository>();

                var personData = personInRoleRepository.GetUsersById(userId);

                var taskIds = userTaskRepository.GetUserTaskIds(userId);
                var mealTypeIds = personMealTypeRepository.GetPersonMealTypeIds(personData.Id);


                var vmPerson = new VmPerson
                {
                    Id = personData.Id,
                    RoleId = personData.RoleId,
                    SizeId = personData.SizeId,
                    Sex = personData.Sex,
                    UniversityId = personData.UniversityId,
                    University = personData.University ?? "",
                    Affiliation = personData.Affiliation ?? "",
                    Abbreviation = personData.Abbreviation ?? "",
                    JacketSizeId = personData.JacketSizeId,
                    JacketSize = personData.JacketSize ?? "",
                    DietTypeId = personData.DietTypeId,
                    DietType = personData.DietType ?? "",
                    Allergies = personData.Allergies ?? "",
                    UserId = personData.UserId,
                    ProfilePictureUrl = personData.ProfilePictureUrl ?? "",
                    UniversityPictureUrl = personData.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                    ResumeUrl = personData.ResumeUrl ?? "",
                    FirstName = personData.FirstName ?? "",
                    LastName = personData.LastName ?? "",
                    RoleName = personData.RoleName ?? "",
                    Email = personData.Email,
                    SecondaryEmail = personData.SecondaryEmail,
                    T_Shirt_Size = personData.T_Shirt_Size ?? "",
                    PhoneNumber = personData.PhoneNumber ?? "",
                    WorkPhoneNumber = personData.WorkPhoneNumber ?? "",
                    StreetLine1 = personData.StreetLine1 ?? "",
                    StreetLine2 = personData.StreetLine2 ?? "",
                    City = personData.City ?? "",
                    State = personData.State ?? "",
                    ZipCode = personData.ZipCode ?? "",
                    EmgyPersonFirstName = personData.EmgyPersonFirstName ?? "",
                    EmgyPersonLastName = personData.EmgyPersonLastName ?? "",
                    EmgyPersonPhoneNumber = personData.EmgyPersonPhoneNumber ?? "",
                    EmgyPersonRelationship = personData.EmgyPersonRelationship ?? "",
                    ShortBio = personData.ShortBio ?? "",

                    WelcomeDinner = personData.WelcomeDinner,
                    LunchOnMonday = personData.LunchOnMonday,
                    LunchOnTuesday = personData.LunchOnTuesday,
                    ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                    AwardBanquet = personData.AwardBanquet,
                    NoneOfTheAbove = personData.NoneOfTheAbove,
                    Agreement = personData.Agreement ?? false,
                    IEEEMembership = personData.IEEEMembership,
                    Disability = personData.Disability,
                    Accommodation = personData.Accommodation,
                    FirstGeneration = personData.FirstGeneration,
                    IndividualDisadvantaged = personData.IndividualDisadvantaged,
                    LevelOfConfidenceId = personData.LevelOfConfidenceId,
                    HouseholdEducationId = personData.HouseholdEducationId,
                    TaskIds = taskIds,
                    ClientTaskIds = string.Join(",", taskIds),

                    MealTypeIds = mealTypeIds,
                    ClientMealTypeIds = string.Join(",", mealTypeIds),
                    Major = personData.Major,
                    Minor = personData.Minor,

                    YearClassificationId = personData.YearClassificationId,
                    YearClassification = personData.YearClassification ?? "",

                };



                vmPerson.LevelOfConfidenceList = (from loc in levelOfConfidenceRepository.Select(0, int.MaxValue)
                                                  select new VmLevelOfConfidence
                                                  {
                                                      Id = loc.Id,
                                                      Name = loc.Name,

                                                  }).ToList();
                vmPerson.HouseholdEducationList = (from hhe in householdEducationRepository.Select(0, int.MaxValue)
                                                   select new VmHouseholdEducation
                                                   {
                                                       Id = hhe.Id,
                                                       Name = hhe.Name,

                                                   }).ToList();

                if (personData.GoalsAfterGraduationIds != null && personData.GoalsAfterGraduationIds.Count() > 0)
                {
                    vmPerson.GoalsAfterGraduationIds = personData.GoalsAfterGraduationIds.Split(',').Select(int.Parse).ToArray();
                    vmPerson.ClientGoalsAfterGraduationIds = personData.GoalsAfterGraduationIds;
                    vmPerson.GoalsAfterGraduationSite = personData.GoalsAfterGraduationSite;
                    vmPerson.OtherGoals = personData.OtherGoals;
                }


                if (personData.EthnicityIds != null && personData.EthnicityIds.Count() > 0)
                {
                    vmPerson.EthnicityIds = personData.EthnicityIds.Split(',').Select(int.Parse).ToArray();
                    vmPerson.ClientEthnicityIds = personData.EthnicityIds;
                }
                return vmPerson;
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
                var mealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();
                var householdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();

                var personData = personInRoleRepository.GetUsersById(userId);

                var taskIds = tempUserTaskRepository.GetTempUserTaskIds(userId);
                var mealTypeIds = personMealTypeRepository.GetPersonMealTypeIds(personData.Id);
                var mealTypeList = new BLMealType().GetAllMealType();

                var vmPerson = new VmPerson
                {
                    Id = personData.Id,
                    RoleId = personData.RoleId,
                    SizeId = personData.SizeId,
                    Sex = personData.Sex,
                    UniversityId = personData.UniversityId,
                    University = personData.University ?? "",
                    Affiliation = personData.Affiliation ?? "",
                    Abbreviation = personData.Abbreviation ?? "",
                    JacketSizeId = personData.JacketSizeId,
                    JacketSize = personData.JacketSize ?? "",
                    DietTypeId = personData.DietTypeId,
                    DietType = personData.DietType ?? "",
                    Allergies = personData.Allergies ?? "",
                    UserId = personData.UserId,
                    ProfilePictureUrl = personData.ProfilePictureUrl ?? "",
                    UniversityPictureUrl = personData.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                    ResumeUrl = personData.ResumeUrl ?? "",
                    FirstName = personData.FirstName ?? "",
                    LastName = personData.LastName ?? "",
                    RoleName = personData.RoleName ?? "",
                    Email = personData.Email,
                    SecondaryEmail = personData.SecondaryEmail,
                    T_Shirt_Size = personData.T_Shirt_Size ?? "",
                    PhoneNumber = personData.PhoneNumber ?? "",
                    WorkPhoneNumber = personData.WorkPhoneNumber ?? "",
                    StreetLine1 = personData.StreetLine1 ?? "",
                    StreetLine2 = personData.StreetLine2 ?? "",
                    City = personData.City ?? "",
                    State = personData.State ?? "",
                    ZipCode = personData.ZipCode ?? "",
                    EmgyPersonFirstName = personData.EmgyPersonFirstName ?? "",
                    EmgyPersonLastName = personData.EmgyPersonLastName ?? "",
                    EmgyPersonPhoneNumber = personData.EmgyPersonPhoneNumber ?? "",
                    EmgyPersonRelationship = personData.EmgyPersonRelationship ?? "",
                    ShortBio = personData.ShortBio ?? "",

                    WelcomeDinner = personData.WelcomeDinner,
                    LunchOnMonday = personData.LunchOnMonday,
                    LunchOnTuesday = personData.LunchOnTuesday,
                    ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                    AwardBanquet = personData.AwardBanquet,
                    NoneOfTheAbove = personData.NoneOfTheAbove,
                    Agreement = personData.Agreement ?? false,
                    IEEEMembership = personData.IEEEMembership,
                    Disability = personData.Disability,
                    Accommodation = personData.Accommodation,
                    FirstGeneration = personData.FirstGeneration,
                    IndividualDisadvantaged = personData.IndividualDisadvantaged,
                    LevelOfConfidenceId = personData.LevelOfConfidenceId,
                    HouseholdEducationId = personData.HouseholdEducationId,

                    TaskIds = taskIds,
                    ClientMealTypeIds = string.Join(",", mealTypeIds),

                    MealTypeList = mealTypeList,
                    MealTypeIds = mealTypeIds,
                    ClientTaskIds = string.Join(",", taskIds),
                    Major = personData.Major,
                    Minor = personData.Minor,

                    YearClassificationId = personData.YearClassificationId,
                    YearClassification = personData.YearClassification ?? "",


                };

                vmPerson.HouseholdEducationList = (from hhe in householdEducationRepository.Select(0, int.MaxValue)
                                                   select new VmHouseholdEducation
                                                   {
                                                       Id = hhe.Id,
                                                       Name = hhe.Name,

                                                   }).ToList();

                if (personData.GoalsAfterGraduationIds != null && personData.GoalsAfterGraduationIds.Count() > 0)
                {
                    vmPerson.GoalsAfterGraduationIds = personData.GoalsAfterGraduationIds.Split(',').Select(int.Parse).ToArray();
                    vmPerson.ClientGoalsAfterGraduationIds = personData.GoalsAfterGraduationIds;
                    vmPerson.GoalsAfterGraduationSite = personData.GoalsAfterGraduationSite;
                    vmPerson.OtherGoals = personData.OtherGoals;
                }

                if (personData.EthnicityIds != null && personData.EthnicityIds.Count() > 0)
                {
                    vmPerson.EthnicityIds = personData.EthnicityIds.Split(',').Select(int.Parse).ToArray();
                    vmPerson.ClientEthnicityIds = personData.EthnicityIds;
                }

                foreach (var item in vmPerson.MealTypeList)
                {
                    item.Checked = (mealTypeIds.Contains(item.Id) == true) ? "checked" : "";
                }

                return vmPerson;
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
        public bool UpdatePerson(VmPerson personData)
        {
            try
            {
                var personRepository = UnitOfWork.GetRepository<PersonRepository>();
                var dietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();

                var person = new Person
                {
                    Id = personData.Id,
                    SizeId = personData.SizeId,
                    Sex = personData.Sex,
                    UniversityId = personData.UniversityId,
                    JacketSizeId = personData.JacketSizeId,
                    UserId = personData.UserId,
                    ResumeUrl = personData.ResumeUrl,
                    FirstName = personData.FirstName,
                    LastName = personData.LastName,
                    StreetLine1 = personData.StreetLine1,
                    StreetLine2 = personData.StreetLine2,
                    City = personData.City,
                    State = personData.State,
                    ZipCode = personData.ZipCode,
                    EmgyPersonFirstName = personData.EmgyPersonFirstName,
                    EmgyPersonLastName = personData.EmgyPersonLastName,
                    EmgyPersonPhoneNumber = personData.EmgyPersonPhoneNumber,
                    EmgyPersonRelationship = personData.EmgyPersonRelationship,
                    ShortBio = personData.ShortBio,

                    WelcomeDinner = personData.WelcomeDinner,
                    LunchOnMonday = personData.LunchOnMonday,
                    LunchOnTuesday = personData.LunchOnTuesday,
                    ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                    AwardBanquet = personData.AwardBanquet,
                    NoneOfTheAbove = personData.NoneOfTheAbove,
                    SecondaryEmail = personData.SecondaryEmail,
                    Agreement = personData.Agreement,
                    IEEEMembership = personData.IEEEMembership,
                    Disability = personData.Disability,
                    Accommodation = personData.Accommodation,
                    FirstGeneration = personData.FirstGeneration,
                    IndividualDisadvantaged = personData.IndividualDisadvantaged,
                    LevelOfConfidenceId = personData.LevelOfConfidenceId,
                    HouseholdEducationId = personData.HouseholdEducationId,
                    Abbreviation = personData.Abbreviation,
                    Affiliation = personData.Affiliation,
                    Major = personData.Major,
                    Minor = personData.Minor,
                    YearClassificationId = personData.YearClassificationId,

                };

                if (personData.DietTypeId == 5)
                {
                    var dietType = new DietType
                    {
                        Id = dietTypeRepository.GetDietTypeNewId(),
                        Name = personData.DietType,
                        Display = true,
                    };

                    person.DietTypeId = dietType.Id;

                    dietTypeRepository.CreateDietType(dietType);
                }
                else
                {
                    person.DietTypeId = personData.DietTypeId;
                }

                person.Allergies = personData.Allergies;

                var oldPerson = new BLPerson().GetPersonByUserId(personData.CurrentUserId);

                //// if (personData.RoleId == "f3b628a1-ab7d-48dc-811d-d509e645d7f0" || personData.RoleId == "291d6069-44a3-4960-86d3-b91bda430e71") // Student or Leader
                {

                    if (!string.IsNullOrWhiteSpace(personData.OtherGoals))
                    {
                        ////var goalsAfterGraduationRepository = UnitOfWork.GetRepository<GoalsAfterGraduationRepository>();
                        ////var goalsAfterGraduation = new GoalsAfterGraduation
                        ////{
                        ////    Id = goalsAfterGraduationRepository.GetGoalsAfterGraduationNewId(),
                        ////    Name = personData.OtherGoals,
                        ////    Display = true,
                        ////    OrderNo = goalsAfterGraduationRepository.GetGoalsAfterGraduationNewOrderNmber(),
                        ////};

                        ////goalsAfterGraduationRepository.CreateGoalsAfterGraduation(goalsAfterGraduation);

                        ////for (int i = 0; i < personData.GoalsAfterGraduationIds.Length; i++)
                        ////{
                        ////    if (personData.GoalsAfterGraduationIds[i] == 4)
                        ////    {
                        ////        personData.GoalsAfterGraduationIds[i] = goalsAfterGraduation.Id;
                        ////        break;
                        ////    }
                        ////}
                    }
                    if (personData.GoalsAfterGraduationIds != null)
                    {
                        person.GoalsAfterGraduationIds = string.Join(",", personData.GoalsAfterGraduationIds);
                    }

                    person.GoalsAfterGraduationSite = personData.GoalsAfterGraduationSite;
                    person.OtherGoals = personData.OtherGoals;

                }
                if (personData.EthnicityIds != null)
                {
                    person.EthnicityIds = string.Join(",", personData.EthnicityIds);

                }

                personRepository.UpdatePerson(person);

                //Do not delete its for next year
                if (personData.RoleId == "4d7951d8-8eda-4452-8ff1-dfc9076d8bbe" && personData.TaskIds != null) //Judge
                {
                    var tempUserTaskRepository = UnitOfWork.GetRepository<TempUserTaskRepository>();

                    tempUserTaskRepository.DeleteTempUserTasks(personData.UserId);
                    tempUserTaskRepository.CreateTasksUser(personData.UserId, personData.TaskIds);
                }

                if (personData.MealTypeIds != null)
                {
                    var personMealTypeRepository = UnitOfWork.GetRepository<PersonMealTypeRepository>();

                    personMealTypeRepository.DeletePersonMealTypes(personData.Id);
                    personMealTypeRepository.CreatePersonMealTypes(personData.Id, personData.MealTypeIds);

                }

                var teamMemberRepository = UnitOfWork.GetRepository<TeamMemberRepository>();

                var blTeam = new BLTeam();
                var teamRepository = UnitOfWork.GetRepository<TeamRepository>();

                var updateableTeamMember = new TeamMember
                {

                    MemberUserId = personData.UserId,
                    RegistrationStatus = true,
                };

                teamMemberRepository.UpdateTeamMemberRegistrationStatusByUserId(updateableTeamMember);

                if (!string.IsNullOrWhiteSpace(oldPerson.Abbreviation) && oldPerson.Abbreviation != personData.Abbreviation)
                {
                    var teams = blTeam.GetAdvisorTeams(personData.CurrentUserId);
                    foreach (var item in teams)
                    {
                        item.Name = item.Name.Replace(oldPerson.Abbreviation, personData.Abbreviation);
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
                var personRepository = UnitOfWork.GetRepository<PersonRepository>();
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

                var personRepository = UnitOfWork.GetRepository<PersonRepository>();
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

                var personRepository = UnitOfWork.GetRepository<PersonRepository>();
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

            var vmApprovalRejectList = from personData in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = personData.UserId,
                                           Sex = personData.Sex,
                                           University = personData.University,
                                           Abbreviation = personData.Abbreviation,
                                           UniversityPictureUrl = personData.UniversityPictureUrl,
                                           FirstName = personData.FirstName,
                                           LastName = personData.LastName,
                                           Name = personData.Name,
                                           RoleName = personData.RoleName,
                                           Email = personData.Email,
                                           EmailConfirmed = personData.EmailConfirmed,
                                           PhoneNumber = personData.PhoneNumber,
                                           UserName = personData.UserName,
                                           LockoutEnabled = personData.LockoutEnabled,
                                           ProfilePictureUrl = personData.ProfilePictureUrl,
                                           Approval = (personData.EmailConfirmed == false && personData.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (personData.EmailConfirmed == false && personData.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = personData.WelcomeDinner,
                                           LunchOnMonday = personData.LunchOnMonday,
                                           LunchOnTuesday = personData.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                                           AwardBanquet = personData.AwardBanquet,
                                           NoneOfTheAbove = personData.NoneOfTheAbove,
                                           Agreement = personData.Agreement ?? false,
                                           IEEEMembership = personData.IEEEMembership,
                                           Disability = personData.Disability,
                                           Accommodation = personData.Accommodation,
                                           FirstGeneration = personData.FirstGeneration,
                                           IndividualDisadvantaged = personData.IndividualDisadvantaged,
                                           LevelOfConfidenceId = personData.LevelOfConfidenceId,
                                           HouseholdEducationId = personData.HouseholdEducationId,
                                       };

            return vmApprovalRejectList;

        }
        public IEnumerable<VmApprovalReject> GetUsersByRoleNames(string[] roleNames)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.GetUsersByRoleNames(roleNames);

            var vmApprovalRejectList = from personData in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = personData.UserId,
                                           Sex = personData.Sex,
                                           University = personData.University,
                                           Abbreviation = personData.Abbreviation,
                                           UniversityPictureUrl = personData.UniversityPictureUrl,
                                           FirstName = personData.FirstName,
                                           LastName = personData.LastName,
                                           Name = personData.Name,
                                           RoleName = personData.RoleName,
                                           Email = personData.Email,
                                           EmailConfirmed = personData.EmailConfirmed,
                                           PhoneNumber = personData.PhoneNumber,
                                           UserName = personData.UserName,
                                           LockoutEnabled = personData.LockoutEnabled,
                                           ProfilePictureUrl = personData.ProfilePictureUrl,
                                           Approval = (personData.EmailConfirmed == false && personData.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (personData.EmailConfirmed == false && personData.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = personData.WelcomeDinner,
                                           LunchOnMonday = personData.LunchOnMonday,
                                           LunchOnTuesday = personData.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                                           AwardBanquet = personData.AwardBanquet,
                                           NoneOfTheAbove = personData.NoneOfTheAbove,
                                           Agreement = personData.Agreement ?? false,
                                           IEEEMembership = personData.IEEEMembership,
                                           Disability = personData.Disability,
                                           Accommodation = personData.Accommodation,
                                           FirstGeneration = personData.FirstGeneration,
                                           IndividualDisadvantaged = personData.IndividualDisadvantaged,
                                           LevelOfConfidenceId = personData.LevelOfConfidenceId,
                                           HouseholdEducationId = personData.HouseholdEducationId,
                                       };

            return vmApprovalRejectList;

        }
        public IEnumerable<VmApprovalReject> GetUsersByEmails(string[] emails)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.GetUsersByEmails(emails);

            var vmApprovalRejectList = from personData in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = personData.UserId,
                                           Sex = personData.Sex,
                                           University = personData.University,
                                           Abbreviation = personData.Abbreviation,
                                           UniversityPictureUrl = personData.UniversityPictureUrl,
                                           FirstName = personData.FirstName,
                                           LastName = personData.LastName,
                                           Name = personData.Name,
                                           RoleName = personData.RoleName,
                                           Email = personData.Email,
                                           EmailConfirmed = personData.EmailConfirmed,
                                           PhoneNumber = personData.PhoneNumber,
                                           UserName = personData.UserName,
                                           LockoutEnabled = personData.LockoutEnabled,
                                           ProfilePictureUrl = personData.ProfilePictureUrl,
                                           Approval = (personData.EmailConfirmed == false && personData.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (personData.EmailConfirmed == false && personData.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = personData.WelcomeDinner,
                                           LunchOnMonday = personData.LunchOnMonday,
                                           LunchOnTuesday = personData.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                                           AwardBanquet = personData.AwardBanquet,
                                           NoneOfTheAbove = personData.NoneOfTheAbove,
                                           Agreement = personData.Agreement ?? false,
                                           IEEEMembership = personData.IEEEMembership,
                                           Disability = personData.Disability,
                                           Accommodation = personData.Accommodation,
                                           FirstGeneration = personData.FirstGeneration,
                                           IndividualDisadvantaged = personData.IndividualDisadvantaged,
                                           LevelOfConfidenceId = personData.LevelOfConfidenceId,
                                           HouseholdEducationId = personData.HouseholdEducationId,
                                       };

            return vmApprovalRejectList;

        }
        public IEnumerable<VmApprovalReject> GetUsersByFilterAndRoleNames(string[] roleNames, VmApprovalReject filterItem = null)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.Select(roleNames, filterItem, 0, int.MaxValue);

            var vmApprovalRejectList = from personData in usersList
                                       select new VmApprovalReject
                                       {
                                           UserId = personData.UserId,
                                           Sex = personData.Sex,
                                           University = personData.University,
                                           FirstName = personData.FirstName,
                                           LastName = personData.LastName,
                                           Name = personData.Name,
                                           RoleName = personData.RoleName,
                                           Email = personData.Email,
                                           EmailConfirmed = personData.EmailConfirmed,
                                           PhoneNumber = personData.PhoneNumber,
                                           UserName = personData.UserName,
                                           LockoutEnabled = personData.LockoutEnabled,
                                           ProfilePictureUrl = personData.ProfilePictureUrl,
                                           Approval = (personData.EmailConfirmed == false && personData.LockoutEnabled == false) ? (int)Approval.Pending
                                           : (personData.EmailConfirmed == false && personData.LockoutEnabled == true) ? 2 : 1,

                                           WelcomeDinner = personData.WelcomeDinner,
                                           LunchOnMonday = personData.LunchOnMonday,
                                           LunchOnTuesday = personData.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                                           AwardBanquet = personData.AwardBanquet,
                                           NoneOfTheAbove = personData.NoneOfTheAbove,
                                           Agreement = personData.Agreement ?? false,
                                           IEEEMembership = personData.IEEEMembership,
                                           Disability = personData.Disability,
                                           Accommodation = personData.Accommodation,
                                           FirstGeneration = personData.FirstGeneration,
                                           IndividualDisadvantaged = personData.IndividualDisadvantaged,
                                           LevelOfConfidenceId = personData.LevelOfConfidenceId,
                                           HouseholdEducationId = personData.HouseholdEducationId,
                                       };

            return vmApprovalRejectList.OrderBy(p => p.Approval);

        }

        public IEnumerable<VmSelectListItem> GetUsersByRoleNameListItem(string[] roleNames)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.GetUsersByRoleNames(roleNames);
            var vmSelectListItem = (from user in usersList
                                    select new VmSelectListItem
                                    {
                                        Value = user.UserId.ToString(),
                                        Text = user.Name,
                                    });

            return vmSelectListItem;
        }

        public IEnumerable<VmPerson> GetUsersByFilter(VmPerson filterItem = null)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.Select(filterItem, 0, int.MaxValue);

            var vmApprovalRejectList = from personData in usersList
                                       select new VmPerson
                                       {
                                           Id = personData.Id,
                                           RoleId = personData.RoleId,
                                           SizeId = personData.SizeId,
                                           Sex = personData.Sex,
                                           UniversityId = personData.UniversityId,
                                           University = personData.University ?? "",
                                           Affiliation = personData.Affiliation ?? "",
                                           Abbreviation = personData.Abbreviation ?? "",
                                           JacketSizeId = personData.JacketSizeId,
                                           JacketSize = personData.JacketSize ?? "",
                                           DietTypeId = personData.DietTypeId,
                                           DietType = personData.DietType ?? "",
                                           Allergies = personData.Allergies ?? "",
                                           UserId = personData.UserId,
                                           ProfilePictureUrl = personData.ProfilePictureUrl ?? "",
                                           UniversityPictureUrl = personData.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                           ResumeUrl = personData.ResumeUrl ?? "",
                                           FirstName = personData.FirstName ?? "",
                                           LastName = personData.LastName ?? "",
                                           RoleName = personData.RoleName ?? "",
                                           Email = personData.Email,
                                           SecondaryEmail = personData.SecondaryEmail,
                                           T_Shirt_Size = personData.T_Shirt_Size ?? "",
                                           PhoneNumber = personData.PhoneNumber ?? "",
                                           WorkPhoneNumber = personData.WorkPhoneNumber ?? "",
                                           StreetLine1 = personData.StreetLine1 ?? "",
                                           StreetLine2 = personData.StreetLine2 ?? "",
                                           City = personData.City ?? "",
                                           State = personData.State ?? "",
                                           ZipCode = personData.ZipCode ?? "",
                                           EmgyPersonFirstName = personData.EmgyPersonFirstName ?? "",
                                           EmgyPersonLastName = personData.EmgyPersonLastName ?? "",
                                           EmgyPersonPhoneNumber = personData.EmgyPersonPhoneNumber ?? "",
                                           EmgyPersonRelationship = personData.EmgyPersonRelationship ?? "",
                                           ShortBio = personData.ShortBio ?? "",

                                           WelcomeDinner = personData.WelcomeDinner,
                                           LunchOnMonday = personData.LunchOnMonday,
                                           LunchOnTuesday = personData.LunchOnTuesday,
                                           ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                                           AwardBanquet = personData.AwardBanquet,
                                           NoneOfTheAbove = personData.NoneOfTheAbove,
                                           Agreement = personData.Agreement ?? false,
                                           IEEEMembership = personData.IEEEMembership,
                                           Disability = personData.Disability,
                                           Accommodation = personData.Accommodation,
                                           FirstGeneration = personData.FirstGeneration,
                                           IndividualDisadvantaged = personData.IndividualDisadvantaged,
                                           LevelOfConfidenceId = personData.LevelOfConfidenceId,
                                           HouseholdEducationId = personData.HouseholdEducationId,
                                       };

            return vmApprovalRejectList.OrderBy(p => p.LastName).ToList();

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

            var vmPersons = (from personData in usersList
                             select new VmPerson
                             {
                                 Id = personData.Id,
                                 RoleId = personData.RoleId,
                                 SizeId = personData.SizeId,
                                 Sex = personData.Sex,
                                 UniversityId = personData.UniversityId,
                                 University = personData.University ?? "",
                                 Affiliation = personData.Affiliation ?? "",
                                 Abbreviation = personData.Abbreviation ?? "",
                                 JacketSizeId = personData.JacketSizeId,
                                 JacketSize = personData.JacketSize ?? "",
                                 DietTypeId = personData.DietTypeId,
                                 DietType = personData.DietType ?? "",
                                 Allergies = personData.Allergies ?? "",
                                 UserId = personData.UserId,
                                 ProfilePictureUrl = personData.ProfilePictureUrl ?? "",
                                 UniversityPictureUrl = personData.UniversityPictureUrl ?? "/Resources/Images/university-empty-pic.png",
                                 ResumeUrl = personData.ResumeUrl ?? "",
                                 FirstName = personData.FirstName ?? "",
                                 LastName = personData.LastName ?? "",
                                 RoleName = personData.RoleName ?? "",
                                 Email = personData.Email,
                                 SecondaryEmail = personData.SecondaryEmail,
                                 T_Shirt_Size = personData.T_Shirt_Size ?? "",
                                 PhoneNumber = personData.PhoneNumber ?? "",
                                 WorkPhoneNumber = personData.WorkPhoneNumber ?? "",
                                 StreetLine1 = personData.StreetLine1 ?? "",
                                 StreetLine2 = personData.StreetLine2 ?? "",
                                 City = personData.City ?? "",
                                 State = personData.State ?? "",
                                 ZipCode = personData.ZipCode ?? "",
                                 EmgyPersonFirstName = personData.EmgyPersonFirstName ?? "",
                                 EmgyPersonLastName = personData.EmgyPersonLastName ?? "",
                                 EmgyPersonPhoneNumber = personData.EmgyPersonPhoneNumber ?? "",
                                 EmgyPersonRelationship = personData.EmgyPersonRelationship ?? "",
                                 ShortBio = personData.ShortBio ?? "",

                                 WelcomeDinner = personData.WelcomeDinner,
                                 LunchOnMonday = personData.LunchOnMonday,
                                 LunchOnTuesday = personData.LunchOnTuesday,
                                 ReceptionNetworkOnTuesday = personData.ReceptionNetworkOnTuesday,
                                 AwardBanquet = personData.AwardBanquet,
                                 NoneOfTheAbove = personData.NoneOfTheAbove,
                                 Agreement = personData.Agreement ?? false,
                                 IEEEMembership = personData.IEEEMembership,
                                 Disability = personData.Disability,
                                 Accommodation = personData.Accommodation,
                                 FirstGeneration = personData.FirstGeneration,
                                 IndividualDisadvantaged = personData.IndividualDisadvantaged,
                                 LevelOfConfidenceId = personData.LevelOfConfidenceId,
                                 HouseholdEducationId = personData.HouseholdEducationId,
                                 Tasks = "",
                                 TeamName = "",
                                 ClientEthnicityIds= personData.EthnicityIds,
                                 ClientGoalsAfterGraduationIds= personData.GoalsAfterGraduationIds
                             }).ToList();


            var blEthnicity = new BLEthnicity();
            var allEthnicity = blEthnicity.GetAllEthnicity();

            var blGoalsAfterGraduation = new BLGoalsAfterGraduation();
            var allGoalsAfterGraduation = blGoalsAfterGraduation.GetAllGoalsAfterGraduation();


            //if (personData.GoalsAfterGraduationIds != null && personData.GoalsAfterGraduationIds.Count() > 0)
            //{
            //    vmPerson.GoalsAfterGraduationIds = personData.GoalsAfterGraduationIds.Split(',').Select(int.Parse).ToArray();
            //    vmPerson.ClientGoalsAfterGraduationIds = personData.GoalsAfterGraduationIds;
            //    vmPerson.GoalsAfterGraduationSite = personData.GoalsAfterGraduationSite;
            //    vmPerson.OtherGoals = personData.OtherGoals;
            //}


            //if (personData.EthnicityIds != null && personData.EthnicityIds.Count() > 0)
            //{
            //    vmPerson.EthnicityIds = personData.EthnicityIds.Split(',').Select(int.Parse).ToArray();
            //    vmPerson.ClientEthnicityIds = personData.EthnicityIds;
            //}

            foreach (var person in vmPersons)
            {
                if (!string.IsNullOrWhiteSpace(person.ClientEthnicityIds))
                {
                    var  ethnicityIdArray = new List<int>();

                    foreach (var item in person.ClientEthnicityIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray())
                    {
                        ethnicityIdArray.Add(int.Parse(item));
                    }    

                    var ethnicities = allEthnicity.Where(t => ethnicityIdArray.Contains(t.Id)).ToList();
                    if (ethnicities != null)
                    {
                        person.EthnicityList = ethnicities;

                    }
                }
                if (!string.IsNullOrWhiteSpace(person.ClientGoalsAfterGraduationIds))
                {

                    var goalsAfterGraduationArray = new List<int>();

                    foreach (var item in person.ClientGoalsAfterGraduationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray())
                    {
                        goalsAfterGraduationArray.Add(int.Parse(item));
                    }
                    var goalsAfterGraduation = allGoalsAfterGraduation.Where(t => goalsAfterGraduationArray.Contains(t.Id)).ToList();
                    if (goalsAfterGraduation != null)
                    {
                        person.GoalsAfterGraduationList = goalsAfterGraduation;
                    }
                }
            }

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