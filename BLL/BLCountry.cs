using Model.ViewModels;
using Model;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Base;

namespace BLL
{
    public class BLCountry : BLBase
    {
        public IEnumerable<VmCountry> GetCountries()
        {
            CountryRepository countryRepository = UnitOfWork.GetRepository<CountryRepository>();

            var countryList = countryRepository.GetCountries();
            var vmCountryList = from country in countryList
                                orderby country.Name
                                select new VmCountry
                                {
                                    Code = country.NumCode.ToString(),
                                     Name = country.Name
                                };


            return vmCountryList;
        }
    }
}
