using Model;

using Repository.EF.Base;

using System;
using System.Linq;

namespace Repository.EF.Repository
{
    public class PersonRepository : EFBaseRepository<Person>
    {
        public void CreatePerson(Person person)
        {
            Add(person);
        }

        public Person GetPersonById(int id)
        {
            var person = Context.People.SingleOrDefault(a => a.Id == id);

            return person;
        }
        public Person GetPersonByUserId(string userId)
        {
            var person = Context.People.SingleOrDefault(a => a.UserId == userId);

            return person;
        }
        public bool PersonIsExistByUserId(string userId)
        {
            return Context.People.Any(a => a.UserId == userId);
        }
        public void UpdatePersonNameByUserId(Person updateablePerson)
        {
            var oldPerson = (from s in Context.People where s.UserId == updateablePerson.UserId select s).FirstOrDefault();

            oldPerson.FirstName = updateablePerson.FirstName;
            oldPerson.LastName = updateablePerson.LastName;

            Update(oldPerson);
        }
        public void UpdateProfileImage(string userId, string profilePictureUrl)
        {
            var oldPerson = (from s in Context.People where s.UserId == userId select s).FirstOrDefault();

            oldPerson.ProfilePictureUrl = profilePictureUrl;

            Update(oldPerson);
        }

        public void UpdateResume(string userId, string resumeUrl)
        {
            var oldPerson = (from s in Context.People where s.UserId == userId select s).FirstOrDefault();

            oldPerson.ResumeUrl = resumeUrl;

            Update(oldPerson);
        }

        public void UpdatePerson(Person person)
        {
            var oldPerson = Context.People.Find(person.Id);
            if (person.UniversityId != null)
            {
                oldPerson.UniversityId = person.UniversityId;
            }

            oldPerson.Sex = person.Sex;
            oldPerson.SizeId = person.SizeId;
            oldPerson.JacketSizeId = person.JacketSizeId;
            oldPerson.UserId = person.UserId;
            oldPerson.FirstName = person.FirstName;
            oldPerson.LastName = person.LastName;
            oldPerson.StreetLine1 = person.StreetLine1;
            oldPerson.StreetLine2 = person.StreetLine2;
            oldPerson.City = person.City;
            oldPerson.State = person.State;
            oldPerson.ZipCode = person.ZipCode;
            oldPerson.EmgyPersonFirstName = person.EmgyPersonFirstName;
            oldPerson.EmgyPersonLastName = person.EmgyPersonLastName;
            oldPerson.EmgyPersonPhoneNumber = person.EmgyPersonPhoneNumber;
            oldPerson.EmgyPersonRelationship = person.EmgyPersonRelationship;
            oldPerson.ShortBio = person.ShortBio;
            oldPerson.DietTypeId = person.DietTypeId;

            oldPerson.WelcomeDinner = person.WelcomeDinner;
            oldPerson.LunchOnMonday = person.LunchOnMonday;
            oldPerson.LunchOnTuesday = person.LunchOnTuesday;
            oldPerson.ReceptionNetworkOnTuesday = person.ReceptionNetworkOnTuesday;
            oldPerson.AwardBanquet = person.AwardBanquet;
            oldPerson.NoneOfTheAbove = person.NoneOfTheAbove;
            oldPerson.SecondaryEmail = person.SecondaryEmail;
            oldPerson.Agreement = person.Agreement;
            oldPerson.Abbreviation = person.Abbreviation;
            oldPerson.Affiliation = person.Affiliation;
            oldPerson.Major = person.Major;
            oldPerson.Minor = person.Minor;
            oldPerson.YearClassificationId = person.YearClassificationId;
            oldPerson.GoalsAfterGraduationIds = person.GoalsAfterGraduationIds;
            oldPerson.GoalsAfterGraduationSite = person.GoalsAfterGraduationSite;
            oldPerson.OtherGoals = person.OtherGoals;
            oldPerson.EthnicityIds = person.EthnicityIds;
            oldPerson.IEEEMembership = person.IEEEMembership;
            oldPerson.Disability = person.Disability;
            oldPerson.Accommodation = person.Accommodation;
            oldPerson.IndividualDisadvantaged = person.IndividualDisadvantaged;
            oldPerson.FirstGeneration = person.FirstGeneration;
            oldPerson.LevelOfConfidenceId = person.LevelOfConfidenceId;
            oldPerson.HouseholdEducationId = person.HouseholdEducationId;

            if (!string.IsNullOrEmpty(person.Allergies))
            {
                oldPerson.Allergies = person.Allergies;
            }

            Update(oldPerson);
        }

        public void ReplaceAdvisor(string userId, string firstName, string lastName, string email)
        {
            var oldPerson = Context.People.Where(p => p.UserId == userId).First();

            oldPerson.FirstName = firstName;
            oldPerson.LastName = lastName;
            oldPerson.SecondaryEmail = email;

            Update(oldPerson);
        }
    }
}
