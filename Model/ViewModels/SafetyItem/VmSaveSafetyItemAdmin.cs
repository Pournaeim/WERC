using System.Web.Mvc;

namespace Model.ViewModels.SafetyItem
{
    public class VmSaveSafetyItemAdmin
    {
        public int SafetyItemId { get; set; }
        [AllowHtml]
        public string Comment { get; set; }
        public int ItemStatus { get; set; }
        public int TeamId { get; set; }
        public string AttachedFileUrl { get; set; }
        [AllowHtml]
        public string LastContent { get; set; }
        public int SubSafetyItemId { get; set; }
    }
}