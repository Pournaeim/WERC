using BLL.Base;
using Model;
using Model.ViewModels.TeamSafetyItemLog;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLTeamSafetyItemLog : BLBase
    {
        public IEnumerable<VmSafetyItemLog> GetTeamSafetyItemLog(int subSafetyItemId, int teamId, bool type)
        {
            try
            {
                var teamSafetyItemLogRepository = UnitOfWork.GetRepository<TeamSafetyItemLogRepository>();

                var teamSafetyItemLog = teamSafetyItemLogRepository.GetTeamSafetyItemLog(subSafetyItemId, teamId, type);

                var vmTeamSafetyItemLog = from tsil in teamSafetyItemLog
                                          select new VmSafetyItemLog
                                          {
                                              Id = tsil.Id,
                                              UserId = tsil.UserId,
                                              UserName = tsil.UserName,
                                              FirstName = tsil.FirstName,
                                              LastName = tsil.LastName,
                                              TeamSafetyItemId = tsil.TeamSafetyItemId,
                                              Content = tsil.Content,
                                              AttachedFileUrl = tsil.AttachedFileUrl,
                                              DateTime = tsil.DateTime,
                                              Type = tsil.Type,

                                          };

                return vmTeamSafetyItemLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VmTeamSafetyItemLog> GetTeamSafetyItemLog(int teamSafetyItemId)
        {
            try
            {
                var teamSafetyItemLogRepository = UnitOfWork.GetRepository<TeamSafetyItemLogRepository>();

                var teamSafetyItemLog = teamSafetyItemLogRepository.GetLastTeamSafetyItemLog(teamSafetyItemId);

                var vmTeamSafetyItemLog = from tsil in teamSafetyItemLog
                                          select new VmTeamSafetyItemLog
                                          {
                                              Id = tsil.Id,
                                              UserId = tsil.UserId,
                                              TeamSafetyItemId = tsil.TeamSafetyItemId,
                                              Content = tsil.Content,
                                              AttachedFileUrl = tsil.AttachedFileUrl,
                                              DateTime = tsil.DateTime,
                                              Type = tsil.Type,

                                          };

                return vmTeamSafetyItemLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VmTeamSafetyItemLog> GetTeamSafetyItemLogList(int[] teamSafetyItemIds)
        {
            try
            {
                var teamSafetyItemLogRepository = UnitOfWork.GetRepository<TeamSafetyItemLogRepository>();

                var teamSafetyItemLogList = teamSafetyItemLogRepository.GetLastTeamSafetyItemLogList(teamSafetyItemIds);

                var vmTeamSafetyItemLogList = from tsil in teamSafetyItemLogList
                                              select new VmTeamSafetyItemLog
                                              {
                                                  Id = tsil.Id,
                                                  UserId = tsil.UserId,
                                                  TeamSafetyItemId = tsil.TeamSafetyItemId,
                                                  Content = tsil.Content,
                                                  AttachedFileUrl = tsil.AttachedFileUrl,
                                                  DateTime = tsil.DateTime,
                                                  Type = tsil.Type,

                                              };

                return vmTeamSafetyItemLogList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CreateTeamSafetyItemLog(VmTeamSafetyItemLog vmTeamSafetyItemLog)
        {
            try
            {

                var teamSafetyItemLogRepository = UnitOfWork.GetRepository<TeamSafetyItemLogRepository>();

                teamSafetyItemLogRepository.CreateTeamSafetyItemLog(
                    new TeamSafetyItemLog
                    {
                        UserId = vmTeamSafetyItemLog.UserId,
                        TeamSafetyItemId = vmTeamSafetyItemLog.TeamSafetyItemId,
                        Content = vmTeamSafetyItemLog.Content,
                        AttachedFileUrl = vmTeamSafetyItemLog.AttachedFileUrl,
                        DateTime = vmTeamSafetyItemLog.DateTime,
                        Type = vmTeamSafetyItemLog.Type,
                    });

                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool UpdateTeamSafetyItemLog(VmTeamSafetyItemLog vmTeamSafetyItemLog)
        {
            try
            {
                var teamSafetyItemLogRepository = UnitOfWork.GetRepository<TeamSafetyItemLogRepository>();

                var teamSafetyItemLog = new TeamSafetyItemLog
                {
                    Id = vmTeamSafetyItemLog.Id,
                    UserId = vmTeamSafetyItemLog.UserId,
                    TeamSafetyItemId = vmTeamSafetyItemLog.TeamSafetyItemId,
                    Content = vmTeamSafetyItemLog.Content,
                    AttachedFileUrl = vmTeamSafetyItemLog.AttachedFileUrl,
                    DateTime = vmTeamSafetyItemLog.DateTime,
                    Type = vmTeamSafetyItemLog.Type,
                };

                teamSafetyItemLogRepository.UpdateTeamSafetyItemLog(teamSafetyItemLog);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}