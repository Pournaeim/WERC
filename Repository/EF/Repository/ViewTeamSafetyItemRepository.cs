using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamSafetyItemRepository : EFBaseRepository<ViewTeamSafetyItem>
    {
        public ViewTeamSafetyItem GetViewTeamSafetyItemById(int id)
        {
            var viewTeamSafetyItem = Context.ViewTeamSafetyItems.SingleOrDefault(a => a.Id == id);

            return viewTeamSafetyItem;
        }
        public IEnumerable<ViewTeamSafetyItem> GetViewTeamSafetyItemByTeamId(int teamId)
        {
            var viewTeamSafetyItem = Context.ViewTeamSafetyItems.Where(a => a.TeamId == teamId);

            return viewTeamSafetyItem;
        }
        public IEnumerable<ViewTeamSafetyItem> GetViewTeamSafetyItemByTeamIdAndSafetyItemId(int teamId, int safetyItemId)
        {
            var viewTeamSafetyItem = Context.ViewTeamSafetyItems.Where(a => a.TeamId == teamId && a.SafetyItemId == safetyItemId);

            return viewTeamSafetyItem;
        }
        public bool ViewTeamSafetyItemIsExistByTeamId(int teamId)
        {
            return Context.ViewTeamSafetyItems.Any(a => a.TeamId == teamId);
        }

    }
}
