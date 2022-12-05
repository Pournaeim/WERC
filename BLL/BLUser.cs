using BLL.Base;

using Model.ApplicationDomainModels;
using Model.ViewModels.User;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLUser : BLBase
    {
        public VmUserList GetAllUsers()
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            var aspNetUserList = userRepository.GetAllUsers();
            var userList = from user in aspNetUserList
                           orderby user.Email
                           select new VmUserFullInfo
                           {
                               Id = user.UserId,
                               UserName = user.UserName,
                               Email = user.Email,
                               RoleName = user.RoleName,
                               RegisterDate = user.RegisterDate.Value,
                               FullName = user.Name
                           };

            var vmUserList = new VmUserList
            {
                Users = userList.ToList()
            };

            var blUserTask = new BLUserTask();
            var userTaskList = blUserTask.GetAllUserTasks().ToList();


            var blTeamMember = new BLTeamMember();
            var tempTeamMemberList = blTeamMember.GetAllTeamMembers().ToList();

            foreach (var item in vmUserList.Users)
            {
                if (item.RoleName.ToLower() == "judge")
                {
                    item.Tasks = string.Join(",", (from t in userTaskList
                                                   where t.UserId == item.Id
                                                   select t.TaskName).ToArray());
                }
                else
                {
                    item.Tasks = string.Join(",", (from t in tempTeamMemberList
                                                   where t.MemberUserId == item.Id
                                                   select t.Task).ToArray());
                }

            }

            return vmUserList;
        }
        public VmUserList GetUserByFiler(string searchText)
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            var aspNetUserList = userRepository.GetUserByFiler(searchText);
            var userList = from user in aspNetUserList
                           orderby user.Email
                           select new VmUserFullInfo
                           {
                               Id = user.UserId,
                               UserName = user.UserName,
                               Email = user.Email,
                               RoleName = user.RoleName,
                               RegisterDate = user.RegisterDate.Value,
                               FullName = user.Name
                           };

            var vmUserList = new VmUserList
            {
                Users = userList.ToList()
            };

            var blUserTask = new BLUserTask();
            var userTaskList = blUserTask.GetAllUserTasks().ToList();


            var blTeamMember = new BLTeamMember();
            var tempTeamMemberList = blTeamMember.GetAllTeamMembers().ToList();

            foreach (var item in vmUserList.Users)
            {
                if (item.RoleName.ToLower() == "judge")
                {
                    item.Tasks = string.Join(",", (from t in userTaskList
                                                   where t.UserId == item.Id
                                                   select t.TaskName).ToArray());
                }
                else
                {
                    item.Tasks = string.Join(",", (from t in tempTeamMemberList
                                                   where t.MemberUserId == item.Id
                                                   select t.Task).ToArray());
                }

            }
            return vmUserList;
        }
        public IEnumerable<SmUserRoles> GetAllUserRoles()
        {
            var viewUserRoleRepository = UnitOfWork.GetRepository<ViewUserRoleRepository>();

            var userRoleList = viewUserRoleRepository.GetAllUserRoles();

            var smUserRoleList = from userRoles in userRoleList
                                 select new SmUserRoles
                                 {
                                     Id = userRoles.Id,
                                     UserId = userRoles.UserId,
                                     UserName = userRoles.UserName,
                                     RoleName = userRoles.RoleName,
                                 };

            return smUserRoleList;
        }

        public IEnumerable<SmUserRoles> GetUsersByRole(string roleId)
        {
            var viewUserRoleRepository = UnitOfWork.GetRepository<ViewUserRoleRepository>();

            var userRoleList = viewUserRoleRepository.GetUsersByRole(roleId);

            var smUserRoleList = from userRoles in userRoleList
                                 select new SmUserRoles
                                 {
                                     Id = userRoles.Id,
                                     UserId = userRoles.UserId,
                                     UserName = userRoles.UserName,
                                     RoleName = userRoles.RoleName,
                                 };

            return smUserRoleList;

        }

        public IEnumerable<SmUserRoles> GetUsersByRoleName(string roleName)
        {
            var viewUserRoleRepository = UnitOfWork.GetRepository<ViewUserRoleRepository>();

            var userRoleList = viewUserRoleRepository.GetUsersByRoleName(roleName);

            var smUserRoleList = from userRoles in userRoleList
                                 select new SmUserRoles
                                 {
                                     Id = userRoles.Id,
                                     UserId = userRoles.UserId,
                                     UserName = userRoles.UserName,
                                     RoleName = userRoles.RoleName,
                                     Email = userRoles.Email,
                                 };

            return smUserRoleList;

        }
        public void UpdatePhoneUserNumber(string userId, string phoneNumber, string workPhoneNumber)
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            userRepository.UpdatePhoneUserNumber(userId, phoneNumber, workPhoneNumber);
            UnitOfWork.Commit();
        }
        public bool ConfirmEmail(string userId)
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            userRepository.ConfirmEmail(userId);
            return UnitOfWork.Commit();
        }
        public bool HasUserRoles(string userId)
        {
            var userRepository = UnitOfWork.GetRepository<ViewUserRoleRepository>();
            return userRepository.HasUserRoles(userId);
        }

        public bool DeleteAllUsersExecptAdmin()
        {
            try
            {
                var userRepository = UnitOfWork.GetRepository<UserRepository>();
                var taskRepository = UnitOfWork.GetRepository<TaskRepository>();

                taskRepository.DeleteAllTask();
                userRepository.DeleteAllUsersExecptAdmin();

                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {

                return false;

            }
        }
        public bool DeleteUser(string userId)
        {
            try
            {
                var userRepository = UnitOfWork.GetRepository<UserRepository>();
                var vewiTeamRepository = UnitOfWork.GetRepository<ViewTeamRepository>();

                var viewUserRoleRepository = UnitOfWork.GetRepository<ViewUserRoleRepository>();

                var userInfo = viewUserRoleRepository.GetUsersById(userId);
                if (userInfo != null && userInfo.Count() > 0 && userInfo.First().RoleName.ToLower() == "advisor")
                {
                    if (vewiTeamRepository.HasTeam(userId, "58c326dd-38ea-4d3c-92f9-3935e3763e68") == false)
                    {
                        userRepository.DeleteUser(userId);
                        return UnitOfWork.Commit();
                    }
                }
                else
                {
                    userRepository.DeleteUser(userId);
                    return UnitOfWork.Commit();
                }

                return false;
            }
            catch (Exception ex)
            {

                return false;

            }
        }

        public void UpdateEmailAndUserName(string userId, string email)
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            userRepository.UpdateEmailAndUserName(userId, email);
            UnitOfWork.Commit();
        }
    }
}
