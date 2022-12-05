using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.Admin
{
    public class VmApprovalReject
    {
        public string UserId { get; set; }
        public string University { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Approval { get; set; }
        public bool LockoutEnabled { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool? Sex { get; set; }
        public bool WelcomeDinner { get; set; }
        public bool LunchOnMonday { get; set; }
        public bool LunchOnTuesday { get; set; }
        public bool ReceptionNetworkOnTuesday { get; set; }
        public bool AwardBanquet { get; set; }
        public bool NoneOfTheAbove { get; set; }
        public bool Agreement { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UniversityPictureUrl { get; set; }
        public string Abbreviation { get; set; }
        public bool? IEEEMembership { get; set; }
        public int? LevelOfConfidenceId { get; set; }
        public int? HouseholdEducationId { get; set; }
        public bool? IndividualDisadvantaged { get; set; }
        public bool? FirstGeneration { get; set; }
        public bool? Disability { get; set; }
        public string Accommodation { get; set; }
    }
}
