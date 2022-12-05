using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class AspNetUsersArchiveRepository : EFBaseRepository<AspNetUsersArchive>
    {
        public void CreateAspNetUsersArchive(AspNetUsersArchive AspNetUsersArchive)
        {
            Add(AspNetUsersArchive);
        }

        public AspNetUsersArchive GetAspNetUsersArchiveByEmail(string email)
        {
            var aspNetUsersArchive = Context.AspNetUsersArchives.SingleOrDefault(a => a.Email == email);

            return aspNetUsersArchive;
        }
        public AspNetUsersArchive GetAspNetUsersArchiveById(string id)
        {
            var aspNetUsersArchive = Context.AspNetUsersArchives.SingleOrDefault(a => a.Id == id);

            return aspNetUsersArchive;
        }
      
        public AspNetUsersArchive GetAspNetUsersArchiveByUserId(string userId)
        {
            try
            {
                var aspNetUsersArchive = Context.AspNetUsersArchives.SingleOrDefault(a => a.Id == userId);

                return aspNetUsersArchive;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<AspNetUsersArchive> GetAllAspNetUsersArchive()
        {
            var aspNetUsersArchive = Context.AspNetUsersArchives;

            return aspNetUsersArchive;
        }
        public int GetAspNetUsersArchiveCount()
        {
            return Context.AspNetUsersArchives.Count();
        }

        public bool DeleteAspNetUsersArchive(int id)
        {
            var oldSafetyIteam = Context.AspNetUsersArchives.Find(id);

            Delete(oldSafetyIteam);

            return true;
        }

    }
}
