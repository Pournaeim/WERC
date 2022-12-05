using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class ReferenceRepository : EFBaseRepository<Reference>
    {
        public IEnumerable<Reference> Select(int index, int count)
        {
            var ReferenceList = from Reference in Context.References
                           select Reference;

            return ReferenceList.OrderBy(A => A.Title).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<Reference> GetReferences(string referenceTitle = "")
        {

            var referenceList = from reference in Context.References
                           select reference;

            if (referenceTitle != "")
            {
                referenceList = referenceList.Where(t => t.Title.Contains(referenceTitle));
            }

            return referenceList.ToArray();
        }

        public void CreateReference(Reference newReference)
        {
            Add(newReference);
        }
        public void UpdateReference(Reference updateableReference)
        {
            var oldReference = (from s in Context.References where s.Id == updateableReference.Id select s).FirstOrDefault();

            oldReference.Title = updateableReference.Title;
            if (!string.IsNullOrEmpty(updateableReference.ReferenceFileUrl))
            {
                oldReference.ReferenceFileUrl = updateableReference.ReferenceFileUrl;
            }

            Update(oldReference);
        }
        public bool DeleteReference(int referenceId)
        {
            var oldReference = (from s in Context.References where s.Id == referenceId select s).FirstOrDefault();

            Delete(oldReference);

            return true;
        }

        public Reference GetReferenceById(int id)
        {
            return Context.References.Find(id);

        }
    }
}
