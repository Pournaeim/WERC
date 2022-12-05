 
using System.Xml.Serialization;
/// <summary>
/// url = /service/[shopid]/orders/create
/// </summary>
namespace Model.ShopCart.Order.NewOrder
{
    [XmlRoot(ElementName = "order")]
    public class Order
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "shop")]
        public string Shop { get; set; }
    }

    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
        [XmlElement(ElementName = "order")]
        public Order Order { get; set; }
    }

}
