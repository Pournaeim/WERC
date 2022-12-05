using Model.ViewModels;
using Model.ViewModels.User;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Base;

namespace BLL
{
    public class BLLanguage : BLBase
    {
        public Dictionary<string, string> GetDictionary(string cultureInfoCode)
        {
            var dictionary = new LanguageRepository();

            return dictionary.GetDictionary(cultureInfoCode);
        }
        public List<VmActiveLanguage> GetActiveLanguages()
        {
            var activeLanguage = new LanguageRepository();

            return activeLanguage.GetActiveLanguages();
        }
        public string GetActiveLanguagesCommaSeparated(List<VmActiveLanguage> activeLanguageList)
        {
            var tesmpString = string.Empty;

            return string.Join(",", activeLanguageList.Select(l => l.CultureInfo).ToList<string>());
        }
    }
}
