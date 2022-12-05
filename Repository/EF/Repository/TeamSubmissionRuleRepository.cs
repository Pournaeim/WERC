using Model;
using Model.ViewModels.Test;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TeamSubmissionRuleRepository : EFBaseRepository<TeamSubmissionRule>
    {
        public void CreateTeamSubmissionRule(TeamSubmissionRule teamSubmissionRule)
        {
            Add(teamSubmissionRule);
        }
        public IEnumerable<TeamSubmissionRule> GetAllTeamSubmissionRules()
        {
            return Context.TeamSubmissionRules.ToArray();
        }

        public IEnumerable<TeamSubmissionRule> GetTeamSubmissionRulesByTeam(int teamId)
        {
            return (from s in Context.TeamSubmissionRules
                    where
                    s.TeamId == teamId
                    select s).ToArray();
        }
        public int[] GetTeamSubmissionRuleIds(int teamId)
        {
            return (from s in Context.TeamSubmissionRules where s.TeamId == teamId select s.Id).ToArray();
        }
        public TeamSubmissionRule GetTeamSubmissionRule(int teamId, int submissionRuleId)
        {
            return (from s in Context.TeamSubmissionRules
                    where s.TeamId == teamId && s.SubmissionRuleId == submissionRuleId
                    select s).FirstOrDefault();

        }
        public bool UpdateTeamSubmissionRule(int teamId, int submissionRuleId, string submissionRuleUrl)
        {
            var updateableTeamSubmissionRule = (from s in Context.TeamSubmissionRules
                                                where s.TeamId == teamId && s.SubmissionRuleId == submissionRuleId
                                                select s).FirstOrDefault();


            if (updateableTeamSubmissionRule != null)
            {
                updateableTeamSubmissionRule.UploadDate = DateTime.Now;
                updateableTeamSubmissionRule.SubmissionRuleUrl = submissionRuleUrl;
                Update(updateableTeamSubmissionRule);
                return true;
            }
            else
            {
                return false;
            }


        }
        public void DeleteTeamSubmissionRules(int teamId, int submissionRuleId)
        {
            var deletableTeamSubmissionRules = from s in Context.TeamSubmissionRules
                                               where s.TeamId == teamId && s.SubmissionRuleId == submissionRuleId
                                               select s;

            foreach (var item in deletableTeamSubmissionRules)
            {
                Delete(item);
            }

        }

        public IEnumerable<TeamSubmissionRule> GetTeamsSubmissionRuleById(int id)
        {
            return (from s in Context.TeamSubmissionRules
                    where
                    s.SubmissionRuleId == id
                    select s).ToArray();
        }

        
        public bool RemoveTheLate(int teamId, int submissionRuleId)
        {
            
            var submissionRule = (from s in Context.SubmissionRules
                                                where s.Id == submissionRuleId
                                                select s).FirstOrDefault();

            var updateableTeamSubmissionRule = (from s in Context.TeamSubmissionRules
                                                where s.TeamId == teamId && s.SubmissionRuleId == submissionRuleId
                                                select s).FirstOrDefault();


            if (updateableTeamSubmissionRule != null)
            {
                updateableTeamSubmissionRule.UploadDate = submissionRule.DueDate;
                Update(updateableTeamSubmissionRule);
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
