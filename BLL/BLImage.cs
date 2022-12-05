using Model.ViewModels;
using Model.ViewModels.User;
using Repository.EF.Repository;
using System.Linq;
using System.Collections.Generic;
using BLL.Base;
using Model.ViewModels.Image;

namespace BLL
{
    public class BLImage : BLBase
    {
       
        public IEnumerable<VmImage> GetImagesByType(byte imageType)
        {
            var imageRepository = UnitOfWork.GetRepository<ImageRepository>();
            var imageList = imageRepository.GetImagesByType(imageType);

            var vmImages =from image in imageList
                          select new VmImage()
                          {
                              Id = image.Id,
                              Title = image.Title,
                              Type = image.Type,
                              ImageUrl = image.ImageUrl,
                              Priority = image.Priority,
                          };

            return vmImages;
        }

    }
}
