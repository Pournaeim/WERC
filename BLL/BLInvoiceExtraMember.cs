using BLL.Base;
using Model;
using Repository.EF.Repository;
using System.Collections.Generic;

namespace BLL
{
    public class BLInvoiceExtraMember : BLBase
    {

        #region Extra Member      
        public IEnumerable<ViewInvoice> GetInvoiceByUserIdAndPayStatus(string userId, bool v)
        {
            var viewInvoiceRepository = UnitOfWork.GetRepository<ViewInvoiceRepository>();

            return viewInvoiceRepository.GetViewInvoiceByUserId(userId, true);
        }

        #endregion Extra Member 


    }
}
