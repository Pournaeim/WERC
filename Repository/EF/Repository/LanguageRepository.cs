using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using Model.ViewModels;
using DAL;

namespace Repository.EF.Repository
{
    public class LanguageRepository : EFBaseRepository<AspNetUser>
    {
        public Dictionary<string, string> GetDictionary(string cultureInfoCode)
        {
            using (var context = new WERCEntities())
            {
                var dictionary = (from dict in context.Dictionaries
                                  join refWord in context.RefrenceWords on dict.RefrenceWordId equals refWord.Id
                                  where dict.CultureInfoCode == cultureInfoCode
                                  select new { refWord.Word, dict.Value }).ToList();

                return dictionary.ToDictionary(rw => rw.Word.Trim(), d => d.Value);
            }
        }
        public List<VmActiveLanguage> GetActiveLanguages()
        {
            using (var context = new WERCEntities())
            {
                var activeLanguage = (from lang in context.Languages
                                      where lang.IsActive == true
                                      select new VmActiveLanguage()
                                      {
                                          Name = lang.Language1,
                                          CultureInfo = lang.CultureInfoCode
                                      }).ToList();

                return activeLanguage;
            }
        }
    }
}
