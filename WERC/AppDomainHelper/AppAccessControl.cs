using BLL;

using Model.ApplicationDomainModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.AppDomainHelper
{
    public class AppAccessControl
    {
        public static bool CheckRoleAccessRight(string userName, params SystemRoles[] roles)
        {
            var roleList = roles.Select(r => Enum.GetName(r.GetType(), r)).ToList();

            for (int i = 0; i < roleList.Count; i++)
            {
                roleList[i] = roleList[i].Replace("_", " ");
            }

            if (SmUserRolesList.UserRoles == null)
            {
                var blUser = new BLUser();
                SmUserRolesList.UserRoles = blUser.GetAllUserRoles();
            }

            return SmUserRolesList.UserRoles.Any(r => r.UserName == userName && roleList.Contains(r.RoleName));
        }
    }
}
