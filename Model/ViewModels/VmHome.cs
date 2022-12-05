using Model.Base;
using Model.ViewModels.Image;
using Model.ViewModels.SubmissionRule;

using System.Collections.Generic;

namespace Model.ViewModels
{
    public class VmHome : BaseViewModel
    {
       public IEnumerable<VmImage> Images { get; set; }
    }
}
