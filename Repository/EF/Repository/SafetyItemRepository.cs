using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class SafetyItemRepository : EFBaseRepository<SafetyItem>
    {
        public void CreateSafetyItem(SafetyItem SafetyItem)
        {
            Add(SafetyItem);
        }

        public SafetyItem GetSafetyItemById(int id)
        {
            var safetyItem = Context.SafetyItems.SingleOrDefault(a => a.Id == id);

            return safetyItem;
        }
        public IEnumerable<SafetyItem> GetAllSafetyItems()
        {
            var safetyItem = Context.SafetyItems.OrderBy(a => a.Priority);

            return safetyItem;
        }
        public int GetSafetyItemsCount()
        {
            return Context.SafetyItems.Count();
        }
        public void UpdateSafetyItem(SafetyItem safetyItem)
        {
            var oldSafetyItem = Context.SafetyItems.Find(safetyItem.Id);

            oldSafetyItem.Name = safetyItem.Name;
            oldSafetyItem.Instruction = safetyItem.Instruction;
            oldSafetyItem.Priority = safetyItem.Priority;

            Update(oldSafetyItem);
        }

        public bool DeleteSafetyItem(int id)
        {
            var oldSafetyIteam = Context.SafetyItems.Find(id);

            Delete(oldSafetyIteam);

            return true;
        }

    }
}
