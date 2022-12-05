using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.SubmissionRule
{
    public class VmTeamSubmissionRule
    {

        public int Id { get; set; }
        public int TeamId { get; set; }
        public int SubmissionRuleId { get; set; }
        public DateTime UploadDate { get; set; }
    }

}
