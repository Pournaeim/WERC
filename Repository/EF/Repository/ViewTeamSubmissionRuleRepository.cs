using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamSubmissionRuleRepository : EFBaseRepository<ViewTeamSubmissionRule>
    {

        public IEnumerable<ViewTeamSubmissionRule> GetViewTeamSubmissionRulesByTeam(int teamId)
        {

            var viewTeamSubmissionRuleList = from ts in Context.ViewTeamSubmissionRules
                                             where ts.TeamId == teamId
                                             select ts;

            return viewTeamSubmissionRuleList.ToArray();
        }

        public IEnumerable<ViewTeamSubmissionRule> GetTeamsSubmissionRuleById(int id)
        {

            var viewTaskSubmissionRuleList = from ts in Context.ViewTeamSubmissionRules
                                             where ts.Id == id
                                             select ts;

            return viewTaskSubmissionRuleList.ToArray();
        }

        public IEnumerable<ViewDownloadTeamSubmissionRule> GetDownloadTeamsSubmissionRuleById(int id)
        {

            var viewTaskSubmissionRuleList = from ts in Context.ViewDownloadTeamSubmissionRules
                                             where ts.SubmissionRuleId == id
                                             select ts;

            return viewTaskSubmissionRuleList.ToArray();
        }
        public IEnumerable<ViewDownloadTeamSubmissionRule> GetDownloadTeamsSubmissionRuleByTeamId(int[] teamIds)
        {

            var viewTaskSubmissionRuleList = from ts in Context.ViewDownloadTeamSubmissionRules
                                             where teamIds.Contains(ts.TeamId)
                                             select ts;

            return viewTaskSubmissionRuleList.ToArray();
        }

    }
}
