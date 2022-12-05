using BLL.Base;
using Model;
using Model.ViewModels.Product;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLProduct : BLBase
    {

        public VmProduct GetProductById(int id)
        {
            var productRepository = UnitOfWork.GetRepository<ProductRepository>();

            var product = productRepository.GetProductById(id);

            var vmProduct = new VmProduct
            {
                Id = product.Id,
                ShopOrderId = product.ShopOrderId,
                ShopProductId = product.ShopProductId,
            };

            return vmProduct;
        }
        public IEnumerable<VmProduct> GetAllProduct()
        {
            var productRepository = UnitOfWork.GetRepository<ProductRepository>();

            var productList = productRepository.Select(0, int.MaxValue);
            var vmProductList = from product in productList
                                select new VmProduct
                                {
                                    Id = product.Id,
                                    ShopOrderId = product.ShopOrderId,
                                    ShopProductId = product.ShopProductId,
                                };

            return vmProductList;
        }

        public int CreateProduct(VmProduct vmProduct)
        {
            var result = -1;
            try
            {
                var productRepository = UnitOfWork.GetRepository<ProductRepository>();

                var newProduct = new Product
                {
                    ShopOrderId = vmProduct.ShopOrderId,
                    ShopProductId = vmProduct.ShopProductId,
                };

                productRepository.CreateProduct(newProduct);

                UnitOfWork.Commit();

                result = newProduct.Id;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool CreateBatchProduct(List<VmProduct> vmNewProductList)
        {
            var result = true;
            try
            {
                var productRepository = UnitOfWork.GetRepository<ProductRepository>();
                var newProductList = new List<Product>();

                foreach (var item in vmNewProductList)
                {
                    newProductList.Add(new Product
                    {
                        Amount = item.Amount,
                        ShopOrderId = item.ShopOrderId,
                        ShopProductId = item.ShopProductId,
                    });
                }


                productRepository.CreateProduct(newProductList);

                UnitOfWork.Commit();

            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
        public bool UpdateProduct(VmProduct vmProduct)
        {
            try
            {
                var productRepository = UnitOfWork.GetRepository<ProductRepository>();

                var updateableProduct = new Product
                {
                    Id = vmProduct.Id,
                    ShopProductId = vmProduct.ShopProductId,
                    ShopOrderId = vmProduct.ShopOrderId,
                };

                productRepository.UpdateProduct(updateableProduct);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteProduct(int productId)
        {
            try
            {
                var productRepository = UnitOfWork.GetRepository<ProductRepository>();


                productRepository.DeleteProduct(productId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
    }
}
