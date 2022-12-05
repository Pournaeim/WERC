
using Model.Base;
using Model.ViewModels.Test;
using System;
using System.Collections.Generic;

namespace Model.ViewModels.Task
{
    public class VmUserTask : BaseViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ShortBio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
        public Nullable<int> SizeId { get; set; }
        public string T_Shirt_Size { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<int> JacketSizeId { get; set; }
        public string JacketSize { get; set; }
        public Nullable<int> DietTypeId { get; set; }
        public string DietType { get; set; }
        public string UniversityPictureUrl { get; set; }
        public string TaskDescription { get; set; }
        public List<VmTeamTest> TeamTests { get; set; }
    }
}