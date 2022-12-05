using Model;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace Repository.EF.Repository
{
    public class TeamRepository : EFBaseRepository<Team>
    {

        public void CreateTeam(Team newTeam)
        {
            newTeam.Payment = 100;
            Add(newTeam);
        }
        public void UpdateTeam(Team updateableTeam)
        {
            var oldTeam = (from s in Context.Teams where s.Id == updateableTeam.Id select s).FirstOrDefault();

            oldTeam.Name = updateableTeam.Name;
            oldTeam.TaskId = updateableTeam.TaskId;
            oldTeam.ProjectName = updateableTeam.ProjectName;

            if (!string.IsNullOrEmpty(updateableTeam.ImageUrl))
            {
                oldTeam.ImageUrl = updateableTeam.ImageUrl;
            }

            Update(oldTeam);
        }
        public void UpdateTeamActivation(int teamId, bool deactivate)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.Deactivate = deactivate;

            Update(oldTeam);
        }
        public void UpdateTeamSubmitStatus(int teamId, bool submitStatus)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.SubmitStatus = submitStatus;

            Update(oldTeam);
        }
        public bool ReverseTeamActivation(int teamId)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.Deactivate = !oldTeam.Deactivate;

            Update(oldTeam);
            return oldTeam.Deactivate;
        }
        public void SetAllowSuppressScoringAllTeams(bool suppressScoring)
        {
            var allTeam = Context.Teams.ToList();

            foreach (var team in allTeam)
            {
                team.SuppressScoring = suppressScoring;
                team.AdminSuppressScoring = suppressScoring;
                Update(team);

            }
        }
        public void SetAllowViewScoringAllTeams(bool deactive)
        {
            var allTeam = Context.Teams.ToList();

            foreach (var team in allTeam)
            {
                team.ViewFinalScore = deactive;
                Update(team);

            }
        }
        public void SetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring()
        {
            var allTeam = Context.Teams.ToList();

            foreach (var team in allTeam)
            {
                //if (team.AdminSuppressScoring == false)
                {
                    team.SuppressScoring = true;

                    Update(team);
                }

            }
        }
        public void UnSetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring()
        {
            var allTeam = Context.Teams.ToList();

            foreach (var team in allTeam)
            {
                team.SuppressScoring = false;
                team.AdminSuppressScoring = false;

                Update(team);

            }
        }

        public bool ReversePaidByCheque(int teamId)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.PaidByCheque = !oldTeam.PaidByCheque;

            Update(oldTeam);
            return oldTeam.PaidByCheque;
        }

        public void UpdateteamPayment(int teamId, decimal payment)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.Payment = payment;

            Update(oldTeam);
        }
        public bool UpdateTeamPayStatus(int teamId)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.PayStatus = !oldTeam.PayStatus;

            Update(oldTeam);
            return oldTeam.PayStatus;
        }
        public bool UpdateTeamScored(int teamId)
        {
            var oldTeam = Context.Teams.Find(teamId);

            var scored = oldTeam.Scored ?? false;
            oldTeam.Scored = !scored;

            Update(oldTeam);
            return oldTeam.Scored.Value;
        }
        public bool UpdateTeamPayStatus(int teamId, bool payStatus)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.PayStatus = payStatus;

            Update(oldTeam);
            return oldTeam.PayStatus;
        }
        public bool ReverseTeamSuppressScoring(int teamId)
        {
            var oldTeam = Context.Teams.Find(teamId);

            oldTeam.SuppressScoring = !oldTeam.SuppressScoring;
            oldTeam.AdminSuppressScoring = !oldTeam.AdminSuppressScoring;

            Update(oldTeam);
            return oldTeam.SuppressScoring;
        }
        public int FindFirstEmptyTeamNumber()
        {
            var teamNumberList = Context.Teams.OrderBy(t => t.TeamNumber).Select(t => t.TeamNumber).ToArray();
            if (teamNumberList.Length == 0)
            {
                return 1;
            }

            int i = 1;
            foreach (var item in teamNumberList)
            {
                if (item != i)
                    break;
                i++;
            }

            return i;
        }

        public IEnumerable<Team> GetAllTeam()
        {
            var teamNumberList = Context.Teams.OrderBy(t => t.TeamNumber).ToArray();

            return teamNumberList;
        }
        public IEnumerable<Team> GetTeamsByTask(int taskId)
        {
            var teamNumberList = Context.Teams.Where(t => t.TaskId == taskId).ToArray();

            return teamNumberList;
        }
        public bool DeleteTeam(int teamId)
        {
            var oldTeam = (from s in Context.Teams where s.Id == teamId select s).FirstOrDefault();

            if (oldTeam.TeamMembers.Count() > 1)
            {
                return false;
            }

            Delete(oldTeam);

            return true;
        }

        public void UpdateTeamState(int teamId, TeamState teamState)
        {
            int state = GetTeamStateTypeNumber(teamState);

            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.State = state;
            Update(oldTeams);
        }
        public void UpdateTeamName(int teamId, string name)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.Name = name;
            Update(oldTeams);
        }
        public void UpdateProjectName(int teamId, string projectName)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.ProjectName = projectName;
            Update(oldTeams);
        }
       
        public void UpdateWrittenReport(int teamId, string writtenReport)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.WrittenReportUrl = writtenReport;
            oldTeams.WrittenReportDate = DateTime.Now;
            Update(oldTeams);
        }
        public void UpdatePreliminaryReport(int teamId, string preliminaryReport)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.PreliminaryReportUrl = preliminaryReport;
            oldTeams.PreliminaryReportDate = DateTime.Now;
            Update(oldTeams);
        }
        public void UpdateFlashTalkReport(int teamId, string FlashTalkReport)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.FlashTalkReportUrl = FlashTalkReport;
            oldTeams.FlashTalkReportDate = DateTime.Now;
            Update(oldTeams);
        }
        public void UpdateAwardNomination(int teamId, string AwardNomination)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.AwardNominationUrl = AwardNomination;
            oldTeams.AwardNominationDate = DateTime.Now;
            Update(oldTeams);
        }
        public void UpdateBrochure(int teamId, string Brochure)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.BrochureUrl = Brochure;
            oldTeams.BrochureDate = DateTime.Now;
            Update(oldTeams);
        }
        public void UpdateOpenTaskTestPlan(int teamId, string openTaskTestPlan)
        {
            var oldTeams = Context.Teams.Find(teamId);

            oldTeams.OpenTaskTestPlanUrl = openTaskTestPlan;
            oldTeams.OpenTaskTestPlanDate = DateTime.Now;
            Update(oldTeams);
        }

        public void UpdatePayStatus(int teamId, bool payStatus)
        {
            var oldTeam = (from s in Context.Teams where s.Id == teamId select s).FirstOrDefault();

            oldTeam.PayStatus = payStatus;
            Update(oldTeam);
        }

        public void UpdatePaidByCheque(int teamId, bool paidByCheque)
        {
            var oldTeam = (from s in Context.Teams where s.Id == teamId select s).FirstOrDefault();

            oldTeam.PaidByCheque = paidByCheque;
            Update(oldTeam);
        }

        public void UpdateTeamRegisterForFlashTalk(int teamId, bool teamRegisterForFlashTalk)
        {
            var oldTeam = (from s in Context.Teams where s.Id == teamId select s).FirstOrDefault();

            oldTeam.TeamRegisterForFlashTalk = teamRegisterForFlashTalk;
            Update(oldTeam);
        }

        public void UpdatePayStatus(int[] teamIds, bool payStatus)
        {
            var oldTeams = (from s in Context.Teams where teamIds.Contains(s.Id) select s);

            foreach (var item in oldTeams)
            {
                item.PayStatus = payStatus;
                Update(item);
            }

        }

    }
}
