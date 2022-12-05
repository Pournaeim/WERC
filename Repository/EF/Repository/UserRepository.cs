using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class UserRepository : EFBaseRepository<AspNetUser>
    {
        public IEnumerable<ViewPersonInRole> GetAllUsers()
        {

            var aspNetUserList = from user in Context.ViewPersonInRoles
                                 orderby user.Email
                                 select user;

            return aspNetUserList.ToArray();

        }
        public AspNetUser GetUserById(string userId)
        {

            var aspNetUser = Context.AspNetUsers.Find(userId);

            return aspNetUser;

        }
        public IEnumerable<ViewPersonInRole> GetUserByFiler(string searchText)
        {

            var aspNetUserList = from user in Context.ViewPersonInRoles
                                 where
                                    user.UserName.Contains(searchText) ||
                                    user.RoleName.Contains(searchText) ||
                                    user.FirstName.Contains(searchText) ||
                                    user.LastName.Contains(searchText) ||
                                    user.Email.Contains(searchText)
                                 orderby user.Email
                                 select user;

            return aspNetUserList.ToArray();

        }
        public void UpdatePhoneUserNumber(string userId, string phoneNumber, string workPhoneNumber)
        {

            var aspNetUser = Context.AspNetUsers.Find(userId);
            aspNetUser.PhoneNumber = phoneNumber;
            aspNetUser.WorkPhoneNumber = workPhoneNumber;

        }
        public void ConfirmEmail(string userId)
        {
            var aspNetUser = Context.AspNetUsers.Find(userId);
            aspNetUser.EmailConfirmed = true;
        }
        public void DeleteAllUsersExecptAdmin()
        {

            var aspNetUserRoleList = from role in Context.ViewUserRoles
                                     where
                                     role.Id != "652a69dc-d46c-4cbf-ba28-8e7759b37752" &&
                                     role.Id != "58c326dd-38ea-4d3c-92f9-3935e3763e68" &&
                                     role.Id != "4d7951d8-8eda-4452-8ff1-dfc9076d8bbe"
                                     select role;

            var aspNetUserList = from user in Context.AspNetUsers
                                 select user;

            foreach (var item in aspNetUserList)
            {
                if (aspNetUserRoleList.Where(u => u.UserId == item.Id).Count() > 0)
                {
                    Delete(item);
                }
            }

        }
        public void DeleteUser(string userId)
        {
            var aspNetUser = Context.AspNetUsers.Find(userId);

            Delete(aspNetUser);

        }

        public void UpdateEmailAndUserName(string userId, string email)
        {
            var aspNetUser = Context.AspNetUsers.Find(userId);
            aspNetUser.Email = email;
            aspNetUser.UserName = email;
        }
    }
}
