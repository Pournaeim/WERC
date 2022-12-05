
using System.Xml.Serialization;

/// <summary>
/// url = /service/[shopid]/orders
/// </summary>
namespace Model.ShopCart.Order.AllOrders
{
    [XmlRoot(ElementName = "product")]
    public class Product
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "quantity")]
        public string Quantity { get; set; }
    }

    [XmlRoot(ElementName = "products")]
    public class Products
    {
        [XmlElement(ElementName = "product")]
        public Product Product { get; set; }
    }

    [XmlRoot(ElementName = "order")]
    public class Order
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "shop")]
        public string Shop { get; set; }
        [XmlElement(ElementName = "products")]
        public Products Products { get; set; }
        [XmlElement(ElementName = "created")]
        public string Created { get; set; }
        [XmlElement(ElementName = "sent")]
        public string Sent { get; set; }
        [XmlElement(ElementName = "received")]
        public string Received { get; set; }
        [XmlElement(ElementName = "perish")]
        public string Perish { get; set; }
    }

    [XmlRoot(ElementName = "orders")]
    public class Orders
    {
        [XmlElement(ElementName = "order")]
        public Order Order { get; set; }
    }

    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
        [XmlElement(ElementName = "orders")]
        public Orders Orders { get; set; }
    }
}
