using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;

namespace Repository.EF.Repository
{
    public class ProductRepository : EFBaseRepository<Product>
    {

        public void CreateProduct(Product newProduct)
        {
            Add(newProduct);
        }
         public void CreateProduct(List<Product> newProductList)
        {
            foreach (var newProduct in newProductList)
            {
                Add(newProduct);
            }
        }
        public void UpdateProduct(Product updateableProduct)
        {
            var oldProduct = (from s in Context.Products where s.Id == updateableProduct.Id select s).FirstOrDefault();

            oldProduct.ShopOrderId = updateableProduct.ShopOrderId;
            oldProduct.ShopProductId = updateableProduct.ShopProductId;
            oldProduct.Amount = updateableProduct.Amount;

            Update(oldProduct);
        }
        public void DeleteProduct(int ProductId)
        {
            var oldProduct = (from s in Context.Products where s.Id == ProductId select s).FirstOrDefault();
            Delete(oldProduct);
        }
        public IEnumerable<Product> EntityList { get; set; }
        public int Count(Func<Product, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<Product> Select(int index = 0, int count = int.MaxValue)
        {
            var ProductList = from Product in Context.Products
                              select Product;

            return ProductList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<Product> Select(Func<Product, bool> predicate, int index, int count)
        {
            var ProductList = (from Product in Context.Products
                               select Product).Where(predicate);

            return ProductList.Skip(index).Take(count).ToArray();
        }
        public Product GetProductById(int id)
        {
            var Product = Context.Products.SingleOrDefault(a => a.Id == id);

            return Product;
        }

        public Product GetProductByOrderId(int shopOrderId)
        {
            var Product = Context.Products.AsNoTracking().SingleOrDefault(a => a.ShopOrderId == shopOrderId);

            return Product;
        }
    }
}
