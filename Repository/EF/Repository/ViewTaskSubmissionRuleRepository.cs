using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTaskSubmissionRuleRepository : EFBaseRepository<ViewTaskSubmissionRule>
    {

        public IEnumerable<ViewTaskSubmissionRule> GetViewTeamSubmissionRulesByTeam(int teamId)
        {

            var viewTaskSubmissionRuleList = from ts in Context.ViewTaskSubmissionRules
                                             where ts.TeamId == teamId
                                             select ts;

            return viewTaskSubmissionRuleList.ToArray();
        }

        public IEnumerable<ViewTaskSubmissionRule> GetViewTeamSubmissionRulesByTeams(int[] teamIds)
        {

            var viewTaskSubmissionRuleList = from ts in Context.ViewTaskSubmissionRules
                                             where teamIds.Contains(ts.TeamId)
                                             select ts;

            return viewTaskSubmissionRuleList.ToArray();
        }

        public IEnumerable<TeamSubmissionRule> GetTeamSubmissionRules()
        {

            var viewTaskSubmissionRuleList = from ts in Context.TeamSubmissionRules
                                             select ts;

            return viewTaskSubmissionRuleList.ToArray();
        }



    }
}
