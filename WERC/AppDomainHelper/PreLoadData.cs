using BLL;
using Model.ApplicationDomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WERC.AppDomainHelper
{
    public static class PreLoadData
    {
        public static Dictionary<string, string> LoadLanguage(string cultureInfoCode)
        {
            var blLanguage = new BLLanguage();
            return blLanguage.GetDictionary(cultureInfoCode);
        }
    }
}