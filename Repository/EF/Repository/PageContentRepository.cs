using Model;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF.Repository
{
    public class PageContentRepository : EFBaseRepository<PageContent>
    {

        public PageContent GetById(int id)
        {
            var pageContent = from p in Context.PageContents
                              where p.Id == id
                              select p;


            return pageContent.Single();
        }

        public void AddNewPageContent(PageContent PageContent)
        {
            Add(PageContent);
        }

        public void UpdatePageContent(PageContent PageContent)
        {
            var oldPageContent = (from s in Context.PageContents where s.Id == PageContent.Id select s).FirstOrDefault();
            oldPageContent.Content = PageContent.Content;

            Update(oldPageContent);
        }
    }

}
