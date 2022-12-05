using System.Xml.Serialization;

namespace Model.ShopCart.Message
{
    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
    }

    public enum ResultCode
    {
        // - The action was considered a success.
        NoError = 0,

        // This is usually due to passing in the wrong [shopid].
        ShopNotFound = 1,

        // - This is usually due to passing in the wrong [productid].
        ProductNotFound = 2,

        // - This is usually due to passing in the wrong [orderid], or perhaps it has "perished".
        OrderNotFound = 3,

        // - This is usually due to failing to pass in the correct "key" for your shop (see "Your service key" above).
        InvalidServiceKey = 4,

        // - This is usually due to the shop not being fully configured yet.
        ShopNotTakingOrdersCurrently = 5,

        // - Once an order has been "sent," you cannot change it any more.
        OrderIsLocked = 6,

        //- This is usually due to a malformed email address being submitted.
        BadPersonalInformationSubmitted = 7
    };
}
