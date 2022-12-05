using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.InlineWebPageHandler
{
    public class VmMousePositionCollection : BaseViewModel
    {

        public int Url { get; set; }
        public IEnumerable<VmMousePosition> MousePositionList { get; set; }
    }
}
