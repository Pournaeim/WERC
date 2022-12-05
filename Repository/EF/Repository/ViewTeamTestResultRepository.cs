using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamTestResultRepository : EFBaseRepository<ViewTeamTestResult>
    {
       
        public IEnumerable<ViewTeamTestResult> GetViewTeamTestResultsByTeam(int teamId)
        {

            var taskList = from task in Context.ViewTeamTestResults
                           where task.TeamId == teamId
                           select task;

            return taskList.ToArray();
        }

 
    }
}
