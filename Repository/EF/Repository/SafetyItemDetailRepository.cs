using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class SafetyItemDetailRepository : EFBaseRepository<SafetyItemDetail>
    {
        public void CreateSafetyItemDetail(SafetyItemDetail SafetyItemDetail)
        {
            Add(SafetyItemDetail);
        }
        public void CreateBatchSafetyItemDetail(List<SafetyItemDetail> SafetyItemDetailList)
        {

            foreach (var item in SafetyItemDetailList)
            {
                Add(item);
            }
        }
        public SafetyItemDetail GetSafetyById(int id)
        {
            var SafetyItemDetail = (from s in Context.SafetyItemDetails where s.Id == id select s).FirstOrDefault();
            return SafetyItemDetail;


        }
 
        public List<SafetyItemDetail> GetSafetyItemDetail( int safetyItemId)
        {
            var SafetyItemDetailList = (from s in Context.SafetyItemDetails
                                      where s.SafetyItemId == safetyItemId
                                        select s).ToList();
            return SafetyItemDetailList;


        }
 
        public void UpdateSafetyItemDetail(SafetyItemDetail SafetyItemDetail)
        {
            var oldSafetyItemDetail = Context.SafetyItemDetails.Find(SafetyItemDetail.Id);

            oldSafetyItemDetail.Id = SafetyItemDetail.Id;
            oldSafetyItemDetail.Comment= SafetyItemDetail.Comment;
            oldSafetyItemDetail.Name= SafetyItemDetail.Name;
            oldSafetyItemDetail.Value= SafetyItemDetail.Value;

            Update(oldSafetyItemDetail);
        }
    }
}
