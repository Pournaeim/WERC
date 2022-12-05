using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.Image
{
    public class VmImage: BaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte Type { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<int> JournalId { get; set; }
        public int Priority { get; set; }
    }
}
