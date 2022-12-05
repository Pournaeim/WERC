using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class UniversityRepository : EFBaseRepository<University>
    {
        public IEnumerable<University> Select(int index, int count)
        {
            var UniversityList = from university in Context.Universities
                                 select university;

            return UniversityList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public University GetUniversityById(int id)
        {
            return Context.Universities.Find(id);
        }
        public void UpdateUniversityImage(int universityId, string universityPictureUrl)
        {
            var oldPerson = (from s in Context.Universities where s.Id == universityId select s).FirstOrDefault();

            oldPerson.UniversityPictureUrl = universityPictureUrl;

            Update(oldPerson);
        }

        public IEnumerable<University> Select(University filterItem, int index, int count)
        {
            var universityList = from university in Context.Universities
                                 select university;

            if (filterItem.Name != null)
            {
                universityList = universityList.Where(t => t.Name.Contains(filterItem.Name));
            }
            return universityList.OrderBy(t => t.Name).Skip(index).Take(count).ToArray();

        }

        public void CreateUniversity(University newUniversity)
        {
            Add(newUniversity);
        }
        public void UpdateUniversity(University updateableUniversity)
        {
            var oldUniversity = (from s in Context.Universities where s.Id == updateableUniversity.Id select s).FirstOrDefault();

            oldUniversity.Name = updateableUniversity.Name;

            Update(oldUniversity);
        }
        public void DeleteUniversity(int UniversityId)
        {
            var oldUniversity = (from s in Context.Universities where s.Id == UniversityId select s).FirstOrDefault();
            Delete(oldUniversity);
        }

    }
}
