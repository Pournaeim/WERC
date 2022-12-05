using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.Reference
{
    public class VmReferenceCollection : BaseViewModel
    {

        public IEnumerable<VmReference> ReferenceList { get; set; }
    }
}
