using System;

namespace Model.ViewModels.GoalsAfterGraduation
{
    public class VmGoalsAfterGraduation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Display { get; set; }
        public int? OrderNo { get; set; }
        public string Checked { get; set; }
        public int Priority { get; set; }
    }
}
