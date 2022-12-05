using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class AspNetUserRepository : EFBaseRepository<AspNetUser>
    {
        public void CreateAspNetUser(AspNetUser AspNetUser)
        {
            Add(AspNetUser);
        }

      
        public AspNetUser GetAspNetUserByUserId(string userId)
        {
            try
            {
                var AspNetUser = Context.AspNetUsers.SingleOrDefault(a => a.Id == userId);

                return AspNetUser;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<AspNetUser> GetAllAspNetUser()
        {
            var AspNetUser = Context.AspNetUsers;

            return AspNetUser;
        }
        public int GetAspNetUserCount()
        {
            return Context.AspNetUsers.Count();
        }
        public void UpdateAspNetUser(AspNetUser AspNetUser)
        {
            var oldAspNetUser = Context.AspNetUsers.Find(AspNetUser.Id);

            oldAspNetUser.Id = AspNetUser.Id;
            oldAspNetUser.PasswordHash = AspNetUser.PasswordHash;
            oldAspNetUser.Email= AspNetUser.Email;
            oldAspNetUser.UserName= AspNetUser.UserName;

            Update(oldAspNetUser);
        }
        public bool DeleteAspNetUser(int id)
        {
            var oldSafetyIteam = Context.AspNetUsers.Find(id);

            Delete(oldSafetyIteam);

            return true;
        }

    }
}
