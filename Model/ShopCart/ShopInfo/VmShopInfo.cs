using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// url = /service/[shopid]/shop
/// </summary>
namespace Model.ShopCart.ShopInfo
{
    
    [XmlRoot(ElementName = "categories")]
    public class Categories
    {
        [XmlElement(ElementName = "category")]
        public List<string> Category { get; set; }
    }

    [XmlRoot(ElementName = "shop")]
    public class Shop
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "introduction")]
        public string Introduction { get; set; }
        [XmlElement(ElementName = "categories")]
        public Categories Categories { get; set; }
    }

    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
        [XmlElement(ElementName = "shop")]
        public Shop Shop { get; set; }
    }
}
