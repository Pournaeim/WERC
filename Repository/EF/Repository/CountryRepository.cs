using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using Model.ViewModels;
using DAL;
using Model.ViewModels.User;

namespace Repository.EF.Repository
{
    public class CountryRepository : EFBaseRepository<AspNetUser>/*, IcountryRepository*/
    {
        public IEnumerable<Country> GetCountries()
        {

            var countryList = from country in Context.Countries
                              orderby country.Name
                              select new Country
                              {
                                  NumCode = country.NumCode,
                                  Name = country.Name
                              };
            return countryList.ToArray();

        }
    }
}
