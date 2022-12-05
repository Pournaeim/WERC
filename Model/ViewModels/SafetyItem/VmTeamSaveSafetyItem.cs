using System.Web;
using System.Web.Mvc;

namespace Model.ViewModels.SafetyItem
{
    public class VmTeamSaveSafetyItem
    {
        public string OldAttachedFileUrl { get; set; }
        public HttpPostedFileBase UploadedAttachedFile { get; set; }
        public int SafetyItemId { get; set; }
        public int SubSafetyItemId { get; set; }
        [AllowHtml]
        public string DescriptionContent { get; set; }
        public int? ItemStatus { get; set; }
        public int TeamId { get; set; }
        public string ChecklistIds { get; set; }

    }
}