using Model.Base;
using Model.ViewModels.Grade;
using System.Collections.Generic;

namespace Model.ViewModels.Grade
{
    public class VmGradeCollection : BaseViewModel
    {
        public string DataAction { get; set; }
        public string DataController { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool ReadOnlyForm { get; set; }
        public bool ShowSearchBox { get; set; }
        public bool Draggable { get; set; }
        public string SearchText { get; set; }
        public int ActiveItemId { get; set; }
        public string HtmlControlId { get; set; }
        public string ParentHtmlControlId { get; set; }
        public string OnItemSelected { get; set; }
        public string OnItemDragged { get; set; }
        public string SelectedItemHtmlControlId { get; set; }
        public IEnumerable<VmGrade> GradeList { get; set; }
    }
}
