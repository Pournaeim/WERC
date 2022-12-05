using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class AspNetUsers2Repository : EFBaseRepository<AspNetUsers2>
    {
        public void CreateAspNetUsers2(AspNetUsers2 AspNetUsers2)
        {
            Add(AspNetUsers2);
        }

        public AspNetUsers2 GetAspNetUsers2ById(int id)
        {
            var aspNetUsers2 = Context.AspNetUsers2.SingleOrDefault(a => a.Id == id);

            return aspNetUsers2;
        }
      
        public AspNetUsers2 GetAspNetUsers2ByUserId(string userId)
        {
            try
            {
                var aspNetUsers2 = Context.AspNetUsers2.SingleOrDefault(a => a.UserId == userId);

                return aspNetUsers2;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<AspNetUsers2> GetAllAspNetUsers2()
        {
            var aspNetUsers2 = Context.AspNetUsers2;

            return aspNetUsers2;
        }
        public int GetAspNetUsers2Count()
        {
            return Context.AspNetUsers2.Count();
        }
        public void UpdateAspNetUsers2(AspNetUsers2 aspNetUsers2)
        {
            var oldAspNetUsers2 = Context.AspNetUsers2.Find(aspNetUsers2.Id);

            oldAspNetUsers2.UserId = aspNetUsers2.UserId;
            oldAspNetUsers2.Password = aspNetUsers2.Password;

            Update(oldAspNetUsers2);
        }

        public bool DeleteAspNetUsers2(int id)
        {
            var oldSafetyIteam = Context.AspNetUsers2.Find(id);

            Delete(oldSafetyIteam);

            return true;
        }

    }
}
