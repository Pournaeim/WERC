using Model.Base;
using Model.ViewModels.Ethnicity;
using Model.ViewModels.GoalsAfterGraduation;
using Model.ViewModels.HouseholdEducation;
using Model.ViewModels.LevelOfConfidence;
using Model.ViewModels.MealType;
using Model.ViewModels.Task;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Model.ViewModels.Person
{
    public class VmPerson : BaseViewModel
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int? SizeId { get; set; }
        public int? UniversityId { get; set; }
        public int? DietTypeId { get; set; }
        public int? JacketSizeId { get; set; }
        public string UserId { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ResumeUrl { get; set; }
        public string UniversityUrl { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Long-Term Personal Email")]
        public string SecondaryEmail { get; set; }

        [Display(Name = "T-Shirt Size")]
        public string T_Shirt_Size { get; set; }
        [Required]
        [Display(Name = "Phone (work)")]
        public string WorkPhoneNumber { get; set; }
        [Required]
        [Display(Name = "Phone (at contest)")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Street Line 1")]
        public string StreetLine1 { get; set; }

        [Display(Name = "Street Line 2")]
        public string StreetLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string EmgyPersonFirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string EmgyPersonLastName { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string EmgyPersonPhoneNumber { get; set; }
        [Required]
        [Display(Name = "Relationship")]
        public string EmgyPersonRelationship { get; set; }

        public string ShortBio { get; set; }

        [Display(Name = "Welcome Dinner on Sunday April 5th, 2020 at 5:00 PM")]
        public bool WelcomeDinner { get; set; }

        [Display(Name = "Lunch on Monday April 6th, 2020")]
        public bool LunchOnMonday { get; set; }

        [Display(Name = "Lunch on Tuesday")]
        public bool LunchOnTuesday { get; set; }

        [Display(Name = "Networking Reception on Tuesday")]
        public bool ReceptionNetworkOnTuesday { get; set; }

        [Display(Name = "Award Banquet")]
        public bool AwardBanquet { get; set; }

        [Display(Name = "None of the above")]
        public bool NoneOfTheAbove { get; set; }

        [Display(Name = "Preferred abbreviation of your university / college")]
        [Required]
        public string Abbreviation { get; set; }

        public HttpPostedFileBase UploadedProfilePicture { get; set; }
        public HttpPostedFileBase UploadedResume { get; set; }

        public string FulName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string University { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public bool HideEmergency { get; set; }
        public object LabResultUrl { get; set; }
        public bool? Sex { get; set; }
        public string JacketSize { get; set; }
        public string DietType { get; set; }
        public string Allergies { get; set; }
        public string UniversityPictureUrl { get; set; }
        public bool? Agreement { get; set; }
        public bool? EmailConfirmed { get; set; }
        public int[] TaskIds { get; set; }
        public string ClientTaskIds { get; set; }
        public string ClientMealTypeIds { get; set; }
        [Required]
        [Display(Name = "Affiliation to be printed in the handbook")]
        public string Affiliation { get; set; }
        public IEnumerable<VmTask> TaskList { get; set; }
        public List<VmMealType> MealTypeList { get; set; }

        [Required]
        public string Major { get; set; }
        public string Minor { get; set; }

        public string YearClassification { get; set; }
        public string GoalsAfterGraduations { get; set; }
        public string Ethnicities { get; set; }

        public int? YearClassificationId { get; set; }
        public int[] GoalsAfterGraduationIds { get; set; }
        public IEnumerable<VmGoalsAfterGraduation> GoalsAfterGraduationList { get; set; }
        public string ClientGoalsAfterGraduationIds { get; set; }
        public int[] EthnicityIds { get; set; }
        public IEnumerable<VmEthnicity> EthnicityList { get; set; }
        public string ClientEthnicityIds { get; set; }
        public string TeamName { get; set; }
        public string Tasks { get; set; }
        public string ExcelFileUrl { get; set; }
        public int[] MealTypeIds { get; set; }
        public bool? IEEEMembership { get; set; }
        public string OtherGoals { get; set; }
        public List<VmLevelOfConfidence> LevelOfConfidenceList { get; set; }
        public int? LevelOfConfidenceId { get; set; }
        public List<VmHouseholdEducation> HouseholdEducationList { get; set; }
        public int? HouseholdEducationId { get; set; }

        public string HouseholdEducation { get; set; }
        public string LevelOfConfidence { get; set; }
        public bool? IndividualDisadvantaged { get; set; }
        public bool? FirstGeneration { get; set; }
        public bool? Disability { get; set; }
        public string Accommodation { get; set; }
        public bool SaveAndFinishLater { get; set; }
        public string GoalsAfterGraduationSite { get; set; }
    }
}
