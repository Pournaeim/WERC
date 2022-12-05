using System.Web.Mvc;
using WERC.Filters.ActionFilterAttributes;
using WERC.Filters.CacheFilters;
using WERC.Filters.ResultFilters;

namespace WERC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LocalizationAttribute(), 0);
            filters.Add(new BaseModelDataProviderResultFilter(), 1);
            filters.Add(new PreLoadDataActionFilter(), 2);
            filters.Add(new HandleErrorAttribute(), 3);
           //filters.Add(new NoCacheAttribute(), 4);
        }
    }
}
