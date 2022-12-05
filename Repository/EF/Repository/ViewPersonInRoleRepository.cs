using Model;
using Model.ViewModels.Admin;
using Model.ViewModels.Person;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace Repository.EF.Repository
{
    public class ViewPersonInRoleRepository : EFBaseRepository<ViewPersonInRoleRepository>/*, IcountryRepository*/
    {
        public IEnumerable<ViewPersonInRole> GetAllpersons()
        {

            var personInRoleList = from person in Context.ViewPersonInRoles
                                   select person;

            return personInRoleList.ToArray();
        }

        public IEnumerable<ViewPersonInRole> GetUsersByRole(string roleId)
        {
            var personInRoleList = from person in Context.ViewPersonInRoles
                                   where person.UserId == roleId
                                   select person;

            return personInRoleList.ToArray();
        }
        public IEnumerable<ViewPersonInRole> GetUsersByRoleNames(string[] roleNames)
        {
            var personInRoleList = from person in Context.ViewPersonInRoles
                                   where roleNames.Contains(person.RoleName)
                                   select person;

            return personInRoleList.ToArray();
        }
        public IEnumerable<ViewPersonInRole> GetUsersByEmails(string[] emails)
        {
            var personInRoleList = from person in Context.ViewPersonInRoles
                                   where emails.Contains(person.Email)
                                   select person;

            return personInRoleList.ToArray();
        }
        public ViewPersonInRole GetUsersById(string userId)
        {
            var personInRole = from person in Context.ViewPersonInRoles
                               where person.UserId == userId
                               select person;

            return personInRole.FirstOrDefault();
        }
        public string[] GetEmailsByUserIds(string[] userIds)
        {
            var personInRole = from person in Context.ViewPersonInRoles
                               where userIds.Contains(person.UserId)
                               select person.Email;

            return personInRole.ToArray();
        }
        public IEnumerable<ViewPersonInRole> GetPersonsByUserIds(string[] userIds)
        {
            var personInRole = from person in Context.ViewPersonInRoles
                               where userIds.Contains(person.UserId)
                               select person;

            return personInRole.ToArray();
        }
        public IEnumerable<ViewPersonInRole> GetPersonsByUserIdsAndRole(string[] userIds, SystemRoles roleName)
        {
            var role = GetSystemRolesString(roleName);

            var personInRole = from person in Context.ViewPersonInRoles
                               where userIds.Contains(person.UserId) && person.RoleName == role
                               select person;

            return personInRole.ToArray();
        }
        public IEnumerable<ViewPersonInRole> Select(string[] roleNames, VmApprovalReject filterItem, int index, int count)
        {
            var personInRoleList = from person in Context.ViewPersonInRoles
                                   where roleNames.Contains(person.RoleName)
                                   select person;

            if (filterItem.University != null)
            {
                personInRoleList = personInRoleList.Where(t => t.University.Contains(filterItem.University));
            }

            if (filterItem.Email != null)
            {
                personInRoleList = personInRoleList.Where(t => t.Email.Contains(filterItem.Email));
            }

            if (filterItem.Name != null)
            {
                personInRoleList = personInRoleList.Where(t => t.FirstName.Contains(filterItem.Name));
            }

            if (filterItem.PhoneNumber != null)
            {
                personInRoleList = personInRoleList.Where(t => t.PhoneNumber.Contains(filterItem.PhoneNumber));
            }

            return personInRoleList.OrderBy(t => t.FirstName).Skip(index).Take(count).ToArray();

        }
        public IEnumerable<ViewPersonInRole> Select(VmPerson filterItem, int index, int count)
        {
            var personInRoleList = from person in Context.ViewPersonInRoles

                                   select person;
            if (filterItem != null)
            {
                if (filterItem.University != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.University.ToLower().Contains(filterItem.University.ToLower()));
                }

                if (filterItem.Email != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.Email.ToLower().Contains(filterItem.Email.ToLower()));
                }

                if (filterItem.FirstName != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.FirstName.ToLower().Contains(filterItem.FirstName.ToLower()));
                }

                if (filterItem.LastName != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.LastName.ToLower().Contains(filterItem.LastName.ToLower()));
                }

                if (filterItem.WorkPhoneNumber != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.WorkPhoneNumber.Contains(filterItem.WorkPhoneNumber));
                }

                if (filterItem.PhoneNumber != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.PhoneNumber.Contains(filterItem.PhoneNumber));
                }

                if (filterItem.DietType != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.DietType.ToLower().Contains(filterItem.DietType.ToLower()));
                }

                if (filterItem.Allergies != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.Allergies.ToLower().Contains(filterItem.Allergies.ToLower()));
                }

                if (filterItem.RoleName != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.RoleName.ToLower().Contains(filterItem.RoleName.ToLower()));
                }

                if (filterItem.Agreement != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.Agreement == filterItem.Agreement);
                }

                if (filterItem.Sex != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.Sex == filterItem.Sex);
                }

                if (filterItem.JacketSize != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.JacketSize.ToLower() == filterItem.JacketSize.ToLower());
                }
                if (filterItem.T_Shirt_Size != null)
                {
                    personInRoleList = personInRoleList.Where(t => t.T_Shirt_Size.ToLower() == filterItem.T_Shirt_Size.ToLower());
                }
            }
            return personInRoleList.OrderBy(t => t.FirstName).Skip(index).Take(count).ToArray();

        }
    }
}
