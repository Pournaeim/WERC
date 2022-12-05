using Model.Base;

namespace Model.ViewModels.SundryRule
{
    public class VmSundryRule : BaseViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string DueDate { get; set; }
    }
}
