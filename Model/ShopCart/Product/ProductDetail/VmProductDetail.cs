
using System.Xml.Serialization;

/// <summary>
/// url = /service/[shopid]/products/[productid]
/// </summary>
namespace Model.ShopCart.Product.ProductDetail
{
    [XmlRoot(ElementName = "categories")]
    public class Categories
    {
        [XmlElement(ElementName = "category")]
        public string Category { get; set; }
    }

    [XmlRoot(ElementName = "product")]
    public class Product
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "forsale")]
        public string Forsale { get; set; }
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "photo")]
        public string Photo { get; set; }
        [XmlElement(ElementName = "categories")]
        public Categories Categories { get; set; }
        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
    }

    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
        [XmlElement(ElementName = "product")]
        public Product Product { get; set; }
    }

}
