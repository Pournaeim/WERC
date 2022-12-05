using Model.Base;

namespace Model.ToolsModels.Grid
{
    public class VmJsGrid : BaseViewModel
    {
        public string DataAction { get; set; }
        public string DataController { get; set; }
        public string CreateAction { get; set; }
        public string CreateController { get; set; }

        public string EditAction { get; set; }
        public string EditController { get; set; }

        public string DeleteAction { get; set; }
        public string DeleteController { get; set; }
 
        public string ExternalHtmlControlId { get; set; }
        public string HtmlElementId { get; set; } 
    }
}
