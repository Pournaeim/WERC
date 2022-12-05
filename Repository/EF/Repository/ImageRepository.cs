using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using DAL;
using Model.ViewModels.Image;
using static Model.ApplicationDomainModels.ConstantObjects;
using Model.ApplicationDomainModels;

namespace Repository.EF.Repository
{
    public class ImageRepository : EFBaseRepository<Image>
    {

        public IEnumerable<Image> GetImagesByType(byte imageType)
        {

            var images = from image in Context.Images
                         where image.Type == imageType

                         select image;

            return images.OrderBy(i => i.Priority).ToArray();

        }
    }
}
