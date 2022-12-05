
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Model.ViewModels.Team
{
    public class VmTeamFullInfo
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Advisor { get; set; }
        public string Leader { get; set; }
        public string TaskName { get; set; }
        public string Judges { get; set; }
        public string Identifier { get; set; }
        public bool? Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string UserDefiner { get; set; }
        public DateTime? LastSignIn { get; set; }
        public int? UniversityId { get; set; }
        public string University { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ShortBio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ResumeUrl { get; set; }
        public string MemberUserId { get; set; }
        public System.DateTime Date { get; set; }
        public int? TeamState { get; set; }
        public string TeamStateDescription { get; set; }
        public bool? PayStatus { get; set; }
        public string PayStatusDescription { get; set; }
        public bool SafetyStatus { get; set; }
        public string TeamImageUrl { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool? Survey { get; set; }
        public bool? RegistrationStatus { get; set; }
        public int? SizeId { get; set; }
        public string T_Shirt_Size { get; set; }
        public string EmgyPersonFirstName { get; set; }
        public string EmgyPersonLastName { get; set; }
        public string EmgyPersonPhoneNumber { get; set; }
        public string EmgyPersonRelationship { get; set; }
        public string PhoneNumber { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public string LabResultUrl { get; set; }
        public string Shipping { get; set; }
        public string WrittenReportUrl { get; set; }
        public int? JacketSizeId { get; set; }
        public string JacketSize { get; set; }
        public int? DietTypeId { get; set; }
        public string DietType { get; set; }
        public string UniversityPictureUrl { get; set; }
        public bool Deactivate { get; set; }
        public bool? SubmitStatus { get; set; }
        public bool? Approved { get; set; }
        public bool? Status { get; set; }
        public string WrittenReportUrlForMember { get; set; }
        public DateTime? WrittenReportDate { get; set; }
        public decimal Payment { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string PreliminaryReportUrl { get; set; }
        public string PreliminaryReportUrlForMember { get; set; }
        public DateTime? PreliminaryReportDate { get; set; }
        public string OpenTaskTestPlanUrl { get; set; }
        public string OpenTaskTestPlanUrlForMember { get; set; }
        public DateTime? OpenTaskTestPlanDate { get; set; }
        public bool Finished { get; set; }
        public bool? PreliminaryReport { get; set; }
        public string PreliminaryReportStatus { get; set; }
        public bool? OpenTaskTestPlan { get; set; }
        public string OpenTaskTestPlanStatus { get; set; }
        public bool PaidByCheque { get; set; }
        public decimal PaidByChequePercent { get; set; }
        public decimal Amount { get; set; }
        public bool? Scored { get; set; }
        public string FlashTalkReportStatus { get; set; }
        public string FlashTalkReportUrlForMember { get; set; }
        public string FlashTalkReportUrl { get; set; }
        public DateTime? FlashTalkReportDate { get; set; }
        public string BrochureUrl { get; set; }
        public string BrochureUrlForMember { get; set; }
        public DateTime? BrochureDate { get; set; }
        public string BrochureDateString { get; set; }
        public string AwardNominationUrl { get; set; }
        public string AwardNominationUrlForMember { get; set; }
        public DateTime? AwardNominationDate { get; set; }
        public string WrittenReportDateString { get; set; }
        public string ProjectName { get; set; }
        public string ExcelFileUrl { get; set; }
        public string Submissions { get; set; }
        public bool WrittenReportLate { get; set; }
    }
}