using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.DietType
{
    public class VmDietType : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Display { get; set; }
    }
}
