using BLL.SystemTools;

using Model.ApplicationDomainModels;
using Model.ViewModels.Invoice;
using Model.ViewModels.Order;
using Model.ViewModels.Product;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;

namespace BLL
{
    class ShopProductInfo
    {
        public int Id;
        public string Name;
        public decimal Price;
        /// <summary>
        /// New Definition to implement discount Algorithm
        /// </summary>
        List<string> Discouns = new List<string>();
    }
    public class BLShopCart
    {
        readonly Dictionary<string, ShopProductInfo> ProductDictionary_BenchScale = new Dictionary<string, ShopProductInfo>()
        {
            {"1First",new ShopProductInfo{Id = 1392, Name = "Early First", Price = 871.25m}},
            {"1Extra",new ShopProductInfo{Id = 1393, Name = "Early Extra", Price = 666.25m } },
            {"2First",new ShopProductInfo{Id = 1394 ,Name = "Regular First", Price = 973.75m } },
            {"2Extra",new ShopProductInfo{Id = 1395 ,Name = "Regular Extra", Price = 768.75m } },
            {"3First",new ShopProductInfo{Id = 1396 ,Name = "Late First", Price = 1127.50m } },
            {"3Extra",new ShopProductInfo{Id = 1397 ,Name = "Late Extra", Price = 973.75m } },
            {"ExtraParticipant",new ShopProductInfo{Id = 1398 ,Name = "Extra Participant", Price = 153.75m } }
        };

        readonly Dictionary<string, ShopProductInfo> ProductDictionary_Desktop = new Dictionary<string, ShopProductInfo>()
        {
            {"5First",new ShopProductInfo{Id = 1901, Name = "Early First", Price = 307.50m}},
            {"5Extra",new ShopProductInfo{Id = 1902, Name = "Early Extra", Price = 205m } },
            {"6First",new ShopProductInfo{Id = 1903 ,Name = "Regular First", Price = 410m } },
            {"6Extra",new ShopProductInfo{Id = 1904 ,Name = "Regular Extra", Price = 307.50m } },
            {"7First",new ShopProductInfo{Id = 1905 ,Name = "Late First", Price = 512.50m } },
            {"7Extra",new ShopProductInfo{Id = 1906 ,Name = "Late Extra", Price = 410m } },
            {"ExtraParticipant",new ShopProductInfo{Id = 1907 ,Name = "Extra Participant", Price = 30.75m } }
        };

        HttpClient client = new HttpClient();

        public readonly string ServiceKey = "ff23964b7b3ccf746f914c4adedb76e7";
        public readonly string BaseServiceUrl = "https://shopcart.nmsu.edu/service/95/";

        #region Handel Checkout
        public string HandelCheckout(List<VmTeamSelection> teamSelectionList, string advisorUserId)
        {
            var productCode = 0;

            var blInvoice = new BLInvoice();

            var invoice = blInvoice.PayChequeGetInvoiceFullInfoByUserId(advisorUserId, false);

            var blProduct = new BLProduct();
            var productList = new List<VmProduct>();

            var blOrder = new BLOrder();

            var newOrder = new VmOrder()
            {
                InvoiceId = invoice.Id,
                OrderDate = DateTime.Now,
                UserId = advisorUserId,
                Complete = false
            };

            int? shopOrderId = blOrder.GetLastOrder(invoice.Id);
            bool createProduct = false;
            if (shopOrderId == null)
            {
                shopOrderId = blOrder.CreateOrder(newOrder);
                createProduct = true;
            }
            
            string allTeamNames = "";
            foreach (var item in invoice.InvoiceDetails)
            {
                var teamData = new BLTeam().GetTeamById(item.TeamId);
                allTeamNames += teamData.Name + ", ";
                if (item.IsChecked)
                {
                    if (teamData.Payment > 0 && teamData.Payment < 100)
                    {

                        if (teamData.PaymentTypeId == (int)ConstantObjects.PaymentType.Bench_Scale)
                        {
                            productCode = (int)GetDicountProductCode_BenchScale(teamData.Payment, item.PaymentRuleId, item.FirstTeamOrExtraTeam);
                        }
                        else
                        {
                            productCode = (int)GetDicountProductCode_Desktop(teamData.Payment, item.PaymentRuleId, item.FirstTeamOrExtraTeam);

                        }
                    }
                    else
                    {
                        if (teamData.PaymentTypeId == (int)ConstantObjects.PaymentType.Bench_Scale)
                        {
                            productCode = ProductDictionary_BenchScale[item.PaymentRuleId + item.FirstTeamOrExtraTeam].Id;

                        }
                        else
                        {
                            productCode = ProductDictionary_Desktop[item.PaymentRuleId + item.FirstTeamOrExtraTeam].Id;

                        }
                    }

                    productList.Add(new VmProduct
                    {
                        Amount = 1,
                        ShopOrderId = shopOrderId.Value,
                        ShopProductId = productCode

                    });

                    if (item.ExtraParticipantCount > 0)
                    {
                        if (teamData.Payment > 0 && teamData.Payment < 100)
                        {
                            if (teamData.PaymentTypeId == (int)ConstantObjects.PaymentType.Bench_Scale)
                            {
                                productCode = 1398 + 73 + (7 * 18) + ((int)teamData.Payment / 5);
                            }
                            else
                            {
                                productCode = 1907 + (7 * 18) + ((int)teamData.Payment / 5) - 12;
                            }
                        }
                        else
                        {
                            if (teamData.PaymentTypeId == (int)ConstantObjects.PaymentType.Bench_Scale)
                            {
                                productCode = ProductDictionary_BenchScale["ExtraParticipant"].Id;
                            }
                            else
                            {
                                productCode = ProductDictionary_Desktop["ExtraParticipant"].Id;
                            }
                        }

                        // orderInfo = AddProduct(newShopOrder.Order.Id, productCode.ToString(), item.ExtraParticipantCount);

                        //if (orderInfo.Error != "0")
                        //{
                        //    return null;
                        //}
                        //else
                        {
                            productList.Add(new VmProduct
                            {
                                Amount = item.ExtraParticipantCount,
                                ShopOrderId = shopOrderId.Value,
                                ShopProductId = productCode
                            });
                        }
                    }
                }
            }

            if (createProduct)
            {
                blProduct.CreateBatchProduct(productList);
            }

            var amountDue = invoice.Subtotal/* - invoice.TotalConventionalFee*/;

            var person = new BLPerson();
            var userData = person.GetPersonsByUserIds(new string[] { advisorUserId }).First();

            if(!string.IsNullOrWhiteSpace(allTeamNames))
            {
                allTeamNames = allTeamNames.Substring(0, allTeamNames.Length - 2);
            }

            return $"https://commerce.cashnet.com/nmsuwercpay" +
                $"?itemcode=WERC2-REG" +
                $"&EMAIL_G={userData.Email}" +
                $"&EMAIL_G_EDT=N" +
                $"&TEAM_NAME={allTeamNames}" +
                "&TEAM_NAME_EDT=N" +
                $"&NAME_G={userData.FirstName + " " + userData.LastName}" +
                "&NAME_G_EDT=N" +
                "&amount=" + amountDue;
        }

        public decimal GetDicountProductCode_BenchScale(decimal discount, int paymentRuleId, string firstTeamOrExtraTeam)
        {
            int code = 0;
            int coefficient = 0;

            // Early First
            if (paymentRuleId == 1 && firstTeamOrExtraTeam.ToLower() == "first")
            {
                code = 1392;
                coefficient = 1;
            }
            // Early Extra
            else
            if (paymentRuleId == 1 && firstTeamOrExtraTeam.ToLower() == "extra")
            {
                code = 1393;
                coefficient = 2;
            }
            else
            //Regular First
            if (paymentRuleId == 2 && firstTeamOrExtraTeam.ToLower() == "first")
            {
                code = 1394;
                coefficient = 3;
            }
            else
            //Regular Extra
            if (paymentRuleId == 2 && firstTeamOrExtraTeam.ToLower() == "extra")
            {
                code = 1395;
                coefficient = 4;
            }

            else
            //Late First
            if (paymentRuleId == 3 && firstTeamOrExtraTeam.ToLower() == "first")
            {
                code = 1396;
                coefficient = 5;
            }
            else
            //Late Extra
            if (paymentRuleId == 3 && firstTeamOrExtraTeam.ToLower() == "extra")
            {
                code = 1397;
                coefficient = 6;
            }

            return code + 73 + (coefficient * 18) + (discount / 5);
        }
        public decimal GetDicountProductCode_Desktop(decimal discount, int paymentRuleId, string firstTeamOrExtraTeam)
        {
            int code = 0;
            int coefficient = 0;

            // Early First
            if (paymentRuleId == 5 && firstTeamOrExtraTeam.ToLower() == "first")
            {
                code = 1901;
                coefficient = 1;
            }
            // Early Extra
            else
            if (paymentRuleId == 5 && firstTeamOrExtraTeam.ToLower() == "extra")
            {
                code = 1902;
                coefficient = 2;
            }
            else
            //Regular First
            if (paymentRuleId == 6 && firstTeamOrExtraTeam.ToLower() == "first")
            {
                code = 1903;
                coefficient = 3;
            }
            else
            //Regular Extra
            if (paymentRuleId == 6 && firstTeamOrExtraTeam.ToLower() == "extra")
            {
                code = 1904;
                coefficient = 4;
            }

            else
            //Late First
            if (paymentRuleId == 7 && firstTeamOrExtraTeam.ToLower() == "first")
            {
                code = 1905;
                coefficient = 5;
            }
            else
            //Late Extra
            if (paymentRuleId == 7 && firstTeamOrExtraTeam.ToLower() == "extra")
            {
                code = 1906;
                coefficient = 6;
            }

            return code + (coefficient * 18) + (discount / 5) - 12;
        }
        public Model.ShopCart.Checkout.Result HandelCheckout(int invoiceId, string advisorUserId)
        {
            var productCode = 0;

            var blInvoice = new BLInvoice();

            var invoiceList = blInvoice.GetExtraMemberInvoiceFullInfoByUserId(advisorUserId);

            var newShopOrder = CreateOrder();

            if (newShopOrder.Error == "0")
            {
                var blProduct = new BLProduct();
                var productList = new List<VmProduct>();

                var blOrder = new BLOrder();

                var newOrder = new VmOrder()
                {
                    InvoiceId = invoiceList.Id,
                    OrderDate = DateTime.Now,
                    ShopOrderId = int.Parse(newShopOrder.Order.Id),
                    UserId = advisorUserId,
                    Complete = false
                };

                blOrder.CreateOrder(newOrder);

                foreach (var item in invoiceList.InvoiceDetails)
                {
                    if (item.IsChecked)
                    {
                        if (item.PaymentTypeId == (int)ConstantObjects.PaymentType.Bench_Scale)
                        {
                            productCode = ProductDictionary_BenchScale[item.PaymentRuleId + item.FirstTeamOrExtraTeam].Id;
                        }
                        else
                        {
                            productCode = ProductDictionary_Desktop[item.PaymentRuleId + item.FirstTeamOrExtraTeam].Id;
                        }

                        productList.Add(new VmProduct
                        {
                            Amount = 1,
                            ShopOrderId = int.Parse(newShopOrder.Order.Id),
                            ShopProductId = productCode

                        });

                        var orderInfo = AddProduct(newShopOrder.Order.Id, productCode.ToString());
                        if (orderInfo.Error != "0")
                        {
                            return null;
                        }

                        if (item.ExtraParticipantCount > 0)
                        {
                            if (item.PaymentTypeId == (int)ConstantObjects.PaymentType.Bench_Scale)
                            {
                                productCode = ProductDictionary_BenchScale["ExtraParticipant"].Id;
                            }
                            else
                            {
                                productCode = ProductDictionary_Desktop["ExtraParticipant"].Id;
                            }

                            orderInfo = AddProduct(newShopOrder.Order.Id, productCode.ToString(), item.ExtraParticipantCount);

                            if (orderInfo.Error != "0")
                            {
                                return null;
                            }
                            else
                            {
                                productList.Add(new VmProduct
                                {
                                    Amount = item.ExtraParticipantCount,
                                    ShopOrderId = int.Parse(newShopOrder.Order.Id),
                                    ShopProductId = productCode

                                });

                            }
                        }

                    }
                }

                blProduct.CreateBatchProduct(productList);

                return PrepareCheckout(int.Parse(newShopOrder.Order.Id));

            }

            return null;

        }

        public string HandelCheckoutExtraMember(int invoiceId, string advisorUserId)
        {
            var productCode = 0;

            var blInvoice = new BLInvoice();

            var invoice = blInvoice.GetExtraMemberInvoiceFullInfoByUserId(advisorUserId);

            //var newShopOrder = CreateOrder();
            var newShopOrder = new VmOrder()
            {
                InvoiceId = invoice.Id,
                OrderDate = DateTime.Now,
                UserId = advisorUserId,
                Complete = false
            };

            var blProduct = new BLProduct();
            var productList = new List<VmProduct>();

            var blOrder = new BLOrder();

            var newOrder = new VmOrder()
            {
                InvoiceId = invoice.Id,
                OrderDate = DateTime.Now,
                ShopOrderId = newShopOrder.Id,
                UserId = advisorUserId,
                Complete = false
            };

            int? shopOrderId = blOrder.GetLastOrder(invoice.Id);
            bool createProduct = false;
            if (shopOrderId == null)
            {
                shopOrderId = blOrder.CreateOrder(newOrder);
                createProduct = true;
            }

            blOrder.CreateOrder(newOrder);

            foreach (var item in invoice.InvoiceDetails)
            {
                if (item.IsChecked)
                {

                    if (item.ExtraParticipantCount > 0)
                    {
                        if (item.PaymentTypeId == (int)ConstantObjects.PaymentType.Bench_Scale)
                        {
                            productCode = ProductDictionary_BenchScale["ExtraParticipant"].Id;
                        }
                        else
                        {
                            productCode = ProductDictionary_Desktop["ExtraParticipant"].Id;

                        }
                        //var orderInfo = AddProduct(newShopOrder.Id, productCode.ToString(), item.ExtraParticipantCount);

                        //if (orderInfo.Error != "0")
                        //{
                        //    return null;
                        //}
                        //else
                        {
                            productList.Add(new VmProduct
                            {
                                Amount = item.ExtraParticipantCount,
                                ShopOrderId = newShopOrder.Id,
                                ShopProductId = productCode

                            });

                        }
                    }

                }
            }

            if (createProduct)
            {
                blProduct.CreateBatchProduct(productList);
            }

            var amountDue = invoice.Subtotal/* - invoice.TotalConventionalFee*/;
            var person = new BLPerson();
            var userData = person.GetPersonsByUserIds(new string[] { advisorUserId }).First();

            return $"https://commerce.cashnet.com/nmsuwercpay" +
                $"?itemcode=WERC2-REG" +
                $"&EMAIL_G={userData.Email}" +
                $"&EMAIL_G_EDT=N" +
                $"&TEAM_NAME=NA" +
                "&TEAM_NAME_EDT=N" +
                $"&NAME_G={userData.FirstName + " " + userData.LastName}" +
                "&NAME_G_EDT=N" +
                "&amount=" + amountDue;
        }

        /// <summary>
        /// url = ///service/[shopid]/orders/[orderid]/checkout
        /// </summary>
        /// <returns></returns>
        public Model.ShopCart.Checkout.Result PrepareCheckout(int orderId)
        {
            var checkoutResult = new Model.ShopCart.Checkout.Result();
            try
            {

                var url = BaseServiceUrl + "orders/" + orderId + "/checkout?key=" + ServiceKey;

                var xmlData = RequestHandler.GetSecureHttpData(url);
                var serializer = new XmlSerializer(typeof(Model.ShopCart.Checkout.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    checkoutResult = (Model.ShopCart.Checkout.Result)serializer.Deserialize(reader);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return checkoutResult;
        }
        #endregion Handel Checkout

        #region Order

        /// <summary>
        /// url = service/[shopid]/orders/create
        /// </summary>
        /// <returns></returns>
        public Model.ShopCart.Order.NewOrder.Result CreateOrder1()
        {
            var newOrder = new Model.ShopCart.Order.NewOrder.Result();
            try
            {

                var url = BaseServiceUrl + "orders/create?key=" + ServiceKey;
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {

                    var ser = new XmlSerializer(typeof(Model.ShopCart.Order.NewOrder.Result));
                    newOrder = (Model.ShopCart.Order.NewOrder.Result)ser.Deserialize(response.Content.ReadAsStreamAsync().Result);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return newOrder;
        }
        public Model.ShopCart.Order.NewOrder.Result CreateOrder()
        {
            var newOrder = new Model.ShopCart.Order.NewOrder.Result();
            try
            {

                var url = BaseServiceUrl + "orders/create?key=" + ServiceKey;

                var xmlData = RequestHandler.GetSecureHttpData(url);
                var serializer = new XmlSerializer(typeof(Model.ShopCart.Order.NewOrder.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    newOrder = (Model.ShopCart.Order.NewOrder.Result)serializer.Deserialize(reader);
                }


            }
            catch (Exception)
            {
                return null;
            }

            return newOrder;
        }

        /// <summary>
        /// url = /service/[shopid]/orders
        /// </summary>
        /// <returns></returns>
        public Model.ShopCart.Order.AllOrders.Result GetAllOrders()
        {
            var allOrders = new Model.ShopCart.Order.AllOrders.Result();
            try
            {

                var url = BaseServiceUrl + "orders?key=" + ServiceKey;
                var xmlData = RequestHandler.GetSecureHttpData(url);
                var serializer = new XmlSerializer(typeof(Model.ShopCart.Order.AllOrders.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    allOrders = (Model.ShopCart.Order.AllOrders.Result)serializer.Deserialize(reader);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return allOrders;
        }

        /// <summary>
        /// url = /service/[shopid]/orders?verbose=1
        /// </summary>
        /// <returns></returns>
        public Model.ShopCart.Order.AllOrdersVerbose.Result GetAllOrdersVerbose()
        {
            var allOrdersVerbose = new Model.ShopCart.Order.AllOrdersVerbose.Result();
            try
            {

                var url = BaseServiceUrl + "orders?key=" + ServiceKey + "&verbose=1";
                var xmlData = RequestHandler.GetSecureHttpData(url);
                var serializer = new XmlSerializer(typeof(Model.ShopCart.Order.AllOrdersVerbose.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    allOrdersVerbose = (Model.ShopCart.Order.AllOrdersVerbose.Result)serializer.Deserialize(reader);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return allOrdersVerbose;
        }

        /// <summary>
        /// url = /service/[shopid]/orders/[orderid]
        /// </summary>
        /// <returns></returns>
        public Model.ShopCart.Order.OrderInfo.Result GetOrderInfo(int orderId)
        {
            var orderInfo = new Model.ShopCart.Order.OrderInfo.Result();
            try
            {

                var url = BaseServiceUrl + "orders/" + orderId + "?key=" + ServiceKey;

                var xmlData = RequestHandler.GetSecureHttpData(url);

                var serializer = new XmlSerializer(typeof(Model.ShopCart.Order.OrderInfo.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    orderInfo = (Model.ShopCart.Order.OrderInfo.Result)serializer.Deserialize(reader);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return orderInfo;
        }

        /// <summary>
        /// url = /service/[shopid]/orders/[orderid]?verbose=1
        /// </summary>
        /// <returns></returns>
        public Model.ShopCart.Order.OrderInfoVerbose.Result GetOrderInfoVerbose(string orderId)
        {
            var orderInfoInfoVerbose = new Model.ShopCart.Order.OrderInfoVerbose.Result();
            try
            {

                var url = BaseServiceUrl + "orders/" + orderId + "?key=" + ServiceKey + "&verbose=1";
                var xmlData = RequestHandler.GetSecureHttpData(url);
                var serializer = new XmlSerializer(typeof(Model.ShopCart.Order.OrderInfoVerbose.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    orderInfoInfoVerbose = (Model.ShopCart.Order.OrderInfoVerbose.Result)serializer.Deserialize(reader);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return orderInfoInfoVerbose;
        }

        public Model.ShopCart.Order.OrderInfo.Result GetCheckoutStatus(string userId, int invoiceId, out int? lastOrderId)
        {
            var blOrder = new BLOrder();
            lastOrderId = blOrder.GetLastOrder(userId, invoiceId);

            var lastOrderInfo = new Model.ShopCart.Order.OrderInfo.Result()
            {
                Order = new Model.ShopCart.Order.OrderInfo.Order()
                {
                    Received = DateTime.Now.ToString(),
                    Tx = "",
                }
            };

            if (lastOrderId != null)
                return lastOrderInfo;
            return null;
        }

        public Model.ShopCart.Order.OrderInfo.Result GetCheckoutStatus(int invoiceId, out int? lastOrderId)
        {
            var blOrder = new BLOrder();
            lastOrderId = blOrder.GetLastOrder(invoiceId);

            var lastOrderInfo = new Model.ShopCart.Order.OrderInfo.Result()
            {
                Order = new Model.ShopCart.Order.OrderInfo.Order()
                {
                    Received = DateTime.Now.ToString(),
                    Tx = "",
                }
            };

            if (lastOrderId != null)
                return lastOrderInfo;
            return null;
        }

        public Model.ShopCart.Order.OrderInfo.Result PayChequeGetCheckoutStatus(int invoiceId, out int? lastOrderId)
        {
            var blOrder = new BLOrder();
            lastOrderId = blOrder.GetLastOrder(invoiceId);

            if (lastOrderId == null)
            {
                return null;
            }

            var lastOrderInfo = GetOrderInfo(lastOrderId.Value);

            return lastOrderInfo;

        }

        #endregion Order

        #region Product

        /// <summary>
        /// url = /service/[shopid]/orders/[orderid]/add/[productid]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.ShopCart.Order.OrderInfoVerbose.Result AddProduct(string orderId, string productId, int amount = 1)
        {
            var orderInfoInfoVerbose = new Model.ShopCart.Order.OrderInfoVerbose.Result();

            try
            {
                var url = BaseServiceUrl + "orders/" + orderId + "/add/" + productId + "?key=" + ServiceKey + "&amount=" + amount;
                var xmlData = RequestHandler.GetSecureHttpData(url);
                var serializer = new XmlSerializer(typeof(Model.ShopCart.Order.OrderInfoVerbose.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    orderInfoInfoVerbose = (Model.ShopCart.Order.OrderInfoVerbose.Result)serializer.Deserialize(reader);
                }


            }
            catch (Exception ex)
            {
                return null;
            }

            return orderInfoInfoVerbose;
        }
        public Model.ShopCart.Product.Result GetProductList(int orderId)
        {
            Model.ShopCart.Product.Result productList = null;

            try
            {
                var url = BaseServiceUrl + "order/" + orderId + "?key=" + ServiceKey + "&verbose=1";
                var xmlData = RequestHandler.GetSecureHttpData(url);
                var serializer = new XmlSerializer(typeof(Model.ShopCart.Product.Result));

                using (TextReader reader = new StringReader(xmlData))
                {
                    productList = (Model.ShopCart.Product.Result)serializer.Deserialize(reader);
                }


            }
            catch (Exception ex)
            {
                return null;
            }

            return productList;
        }

        #endregion Product



    }
}
