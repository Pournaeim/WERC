using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class SubSafetyItemRepository : EFBaseRepository<SubSafetyItem>
    {
        public void CreateSubSafetyItem(SubSafetyItem SubSafetyItem)
        {
            Add(SubSafetyItem);
        }

        public SubSafetyItem GetSubSafetyItemById(int id)
        {
            var subSafetyItem = Context.SubSafetyItems.SingleOrDefault(a => a.Id == id);

            return subSafetyItem;
        }
        public IEnumerable<SubSafetyItem> GetAllSubSafetyItems()
        {
            var subSafetyItem = Context.SubSafetyItems.OrderBy(a => a.Priority);

            return subSafetyItem;
        }
        public int GetSubSafetyItemsCount()
        {
            return Context.SubSafetyItems.Count();
        }
        public void UpdateSubSafetyItem(SubSafetyItem subSafetyItem)
        {
            var oldSubSafetyItem = Context.SubSafetyItems.Find(subSafetyItem.Id);

            oldSubSafetyItem.Name = subSafetyItem.Name;
            oldSubSafetyItem.Instruction = subSafetyItem.Instruction;
            oldSubSafetyItem.Priority = subSafetyItem.Priority;

            Update(oldSubSafetyItem);
        }

        public bool DeleteSubSafetyItem(int id)
        {
            var oldSafetyIteam = Context.SubSafetyItems.Find(id);

            Delete(oldSafetyIteam);

            return true;
        }

    }
}
