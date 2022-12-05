using Model.ViewModels.Person;
using System.Collections.Generic;

namespace WERC.AppDomainHelper
{
    public static class StaticObjects
    {
        public static Dictionary<string, VmPerson> ActiveUsers = new Dictionary<string, VmPerson>();
    }
}