//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            this.PersonMealTypes = new HashSet<PersonMealType>();
        }
    
        public int Id { get; set; }
        public string UserId { get; set; }
        public Nullable<int> JacketSizeId { get; set; }
        public Nullable<int> DietTypeId { get; set; }
        public Nullable<int> SizeId { get; set; }
        public Nullable<int> UniversityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Identifier { get; set; }
        public Nullable<bool> Sex { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string ShortBio { get; set; }
        public string EmgyPersonFirstName { get; set; }
        public string EmgyPersonLastName { get; set; }
        public string EmgyPersonPhoneNumber { get; set; }
        public string EmgyPersonRelationship { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ResumeUrl { get; set; }
        public bool WelcomeDinner { get; set; }
        public bool LunchOnMonday { get; set; }
        public bool LunchOnTuesday { get; set; }
        public bool ReceptionNetworkOnTuesday { get; set; }
        public bool AwardBanquet { get; set; }
        public bool NoneOfTheAbove { get; set; }
        public string Allergies { get; set; }
        public string SecondaryEmail { get; set; }
        public Nullable<bool> Agreement { get; set; }
        public string Abbreviation { get; set; }
        public string Affiliation { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public Nullable<int> YearClassificationId { get; set; }
        public string GoalsAfterGraduationIds { get; set; }
        public string GoalsAfterGraduationSite { get; set; }
        public string OtherGoals { get; set; }
        public string EthnicityIds { get; set; }
        public Nullable<bool> IEEEMembership { get; set; }
        public Nullable<int> LevelOfConfidenceId { get; set; }
        public Nullable<int> HouseholdEducationId { get; set; }
        public Nullable<bool> IndividualDisadvantaged { get; set; }
        public Nullable<bool> FirstGeneration { get; set; }
        public Nullable<bool> Disability { get; set; }
        public string Accommodation { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual DietType DietType { get; set; }
        public virtual HouseholdEducation HouseholdEducation { get; set; }
        public virtual LevelOfConfidence LevelOfConfidence { get; set; }
        public virtual University University { get; set; }
        public virtual YearClassification YearClassification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonMealType> PersonMealTypes { get; set; }
    }
}