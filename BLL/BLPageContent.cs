using Model.ViewModels;
using Model.ViewModels.User;
using Repository.EF.Repository;
using System.Linq;
using System.Collections.Generic;
using BLL.Base;
using Model.ViewModels.PageContent;
using Model;

namespace BLL
{
    public class BLPageContent : BLBase
    {
        public VmPageContent GetById(int id)
        {
            PageContentRepository PageContentRepository = UnitOfWork.GetRepository<PageContentRepository>();

            var result = PageContentRepository.GetById(id);

            var vmPageContent = new VmPageContent
            {
                Id = result.Id,
                Content = result.Content,
            };

            return vmPageContent;
        }
        public bool UpdatePageContent(VmPageContent pageContent)
        {
            PageContentRepository PageContentRepository = UnitOfWork.GetRepository<PageContentRepository>();

            var updateablePageContent = new PageContent
            {
                Id = pageContent.Id,
                Content = pageContent.Content
            };

            PageContentRepository.UpdatePageContent(updateablePageContent);

            UnitOfWork.Commit();

            return true;
        }
    }
}