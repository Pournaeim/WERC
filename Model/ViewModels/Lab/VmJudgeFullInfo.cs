
using Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Model.ViewModels.Lab
{
    public class VmLabFullInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public bool? Sex { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ShortBio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ResumeUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public int? SizeId { get; set; }
        public string T_Shirt_Size { get; set; }
        public string PhoneNumber { get; set; }
        public int? DietTypeId { get; set; }
        public string DietType { get; set; }
        public string Tasks { get; set; }
        public string Teams { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public bool Agreement { get; set; }
    }
}