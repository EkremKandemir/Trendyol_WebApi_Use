using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dovi_Trendyol_Entegrasyonu.Model;
{
using Newtonsoft.Json.Linq;

namespace Trendyol_Api_Connection
{
    public static class Program
    {
        //write your supplierid
        public static string baseUrl = @"https://api.trendyol.com/sapigw/suppliers/{supplierid}/";

        //WebApi Connections
        public static string SimpleRequest(string apiUrl = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.ContentType = "application/json; charset=utf-8";
            request.Headers["Authorization"] = @"Basic {Turn your username and password into Basic Authorization}";//attention
            request.PreAuthenticate = true;
            var response = request.GetResponse() as HttpWebResponse;
            string result = "";
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                result = reader.ReadToEnd();

            }
            return result;
        }
        //Convert datetime to timestamp 
        public static long ToUnixTimestamp(this DateTime d)
        {
            var epoch = d - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)epoch.TotalSeconds;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            string temp = "";//for double chracter size
            temp = unixTimeStamp.ToString().Substring(0, 10);
            unixTimeStamp = Convert.ToDouble(temp);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        // for order integration
        static void Main(string[] args)
        {
            var list = new List<Jsondata>();

            //To send parameters to the program
            string startdate = "";
            string endDate = "";

            try
            {
                if (args.Length == 0)
                {
                    Console.Write("Start Date: (dd-MM-yyyy) ");
                    startdate = Console.ReadLine();
                    Console.Write("End  Date: (dd-MM-yyyy) ");
                    endDate = Console.ReadLine();
                }
                if (args.Length == 1)
                {
                    startdate = args[0];
                }
                if (args.Length == 2)
                {
                    startdate = args[0];
                    endDate = args[1];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            //Convert parameters to DateTime
            DateTime converted_startdate = DateTime.ParseExact(startdate, "dd-MM-yyyy", null);
            DateTime converted_endDate = DateTime.ParseExact(endDate, "dd-MM-yyyy", null);

            //Convert DateTime to Timestamp
            long startdateTimestamp = ToUnixTimestamp(converted_startdate);
            long endDateTimestamp = ToUnixTimestamp(converted_endDate);

            //json data of orders in result
            var result = SimpleRequest(baseUrl + @"orders?status=Created&startDate=" + startdateTimestamp + "000&endDate=" + endDateTimestamp + "000&orderByField=PackageLastModifiedDate&orderByDirection=DESC&size=50");

            /******************************************************************************************************************************/
            var jsonData = JObject.Parse(result);

            var newItem = new Jsondata()
            {
                //JObject
                page = Convert.ToInt32(jsonData["page"].ToString()),
                size = Convert.ToInt32(jsonData["size"].ToString()),
                totalPages = Convert.ToInt32(jsonData["totalPages"].ToString()),
                totalElements = Convert.ToInt32(jsonData["totalElements"].ToString())
            };

            var lstContent = new List<Content>();
            var shipmentlist = new List<ShipmentAddress>();
            var invoiceAddresslist = new List<InvoiceAddress>();
            var packageHistoriesList = new List<PackageHistories>();
            var Lineslist = new List<Lines>();
            var discountDetailslist = new List<DiscountDetails>();

            //json data parsing
            foreach (JObject i in jsonData["content"])
            {
                var content = new Content()
                {
                    orderNumber = i["orderNumber"].ToString(),
                    grossAmount = Convert.ToDouble(i["grossAmount"].ToString()),
                    totalDiscount = Convert.ToDouble(i["totalDiscount"].ToString()),
                    totalPrice = Convert.ToDouble(i["totalPrice"].ToString()),
                    taxNumber = i["taxNumber"].ToString(),
                    customerFirstName = i["customerFirstName"].ToString(),
                    customerEmail = i["customerEmail"].ToString(),
                    customerId = i["customerId"].ToString(),
                    customerLastName = i["customerLastName"].ToString(),
                    id = i["id"].ToString(),
                    cargoTrackingNumber = Convert.ToInt64(i["cargoTrackingNumber"].ToString()),
                    cargoProviderName = i["cargoProviderName"].ToString(),
                    orderDate = Convert.ToInt64(i["orderDate"].ToString()),
                    tcIdentityNumber = i["tcIdentityNumber"].ToString(),
                    currencyCode = i["currencyCode"].ToString(),
                    shipmentPackageStatus = i["shipmentPackageStatus"].ToString(),
                    deliveryType = i["deliveryType"].ToString(),
                    estimatedDeliveryStartDate = Convert.ToInt64(i["estimatedDeliveryStartDate"].ToString()),
                    estimatedDeliveryEndDate = Convert.ToInt64(i["estimatedDeliveryEndDate"].ToString())

                };

                shipmentlist.Add(new ShipmentAddress
                {
                    id = i["shipmentAddress"]["id"].ToString(),
                    firstName = i["shipmentAddress"]["firstName"].ToString(),
                    lastName = i["shipmentAddress"]["lastName"].ToString(),
                    address1 = i["shipmentAddress"]["address1"].ToString(),
                    address2 = i["shipmentAddress"]["address2"].ToString(),
                    city = i["shipmentAddress"]["city"].ToString(),
                    cityCode = i["shipmentAddress"]["cityCode"].ToString(),
                    district = i["shipmentAddress"]["district"].ToString(),
                    districtId = i["shipmentAddress"]["districtId"].ToString(),
                    postalCode = i["shipmentAddress"]["postalCode"].ToString(),
                    countryCode = i["shipmentAddress"]["countryCode"].ToString(),
                    fullAddress = i["shipmentAddress"]["fullAddress"].ToString(),
                    fullName = i["shipmentAddress"]["fullName"].ToString()

                });
                content.shipmentAddress = shipmentlist;

                if (i["lines"].Count() != 0)
                {
                    foreach (var item in i["lines"])
                    {
                        foreach (var j in item["discountDetails"])
                        {
                            discountDetailslist.Add(new DiscountDetails
                            {
                                lineItemPrice = Convert.ToDouble(j["lineItemPrice"].ToString()),
                                lineItemDiscount = Convert.ToDouble(j["lineItemDiscount"].ToString())
                            });
                        }

                        Lineslist.Add(new Lines
                        {
                            quantity = item["quantity"].ToString(),
                            productId = item["productId"].ToString(),
                            salesCampaignId = item["salesCampaignId"].ToString(),
                            productSize = item["productSize"].ToString(),
                            merchantSku = item["merchantSku"].ToString(),
                            productName = item["productName"].ToString(),
                            productCode = item["productCode"].ToString(),
                            merchantId = item["merchantId"].ToString(),
                            amount = Convert.ToDouble(item["amount"].ToString()),
                            discount = Convert.ToDouble(item["discount"].ToString()),
                            currencyCode = item["currencyCode"].ToString(),
                            productColor = item["productColor"].ToString(),
                            id = item["id"].ToString(),
                            sku = item["sku"].ToString(),
                            vatBaseAmount = Convert.ToInt32(item["vatBaseAmount"].ToString()),
                            barcode = item["barcode"].ToString(),
                            orderLineItemStatusName = item["orderLineItemStatusName"].ToString(),
                            price = Convert.ToDouble(item["price"].ToString()),
                            discountDetails = discountDetailslist
                        });

                    }
                    content.lines = Lineslist;
                }

                invoiceAddresslist.Add(new InvoiceAddress
                {
                    id = i["invoiceAddress"]["id"].ToString(),
                    firstName = i["invoiceAddress"]["firstName"].ToString(),
                    lastName = i["invoiceAddress"]["lastName"].ToString(),
                    company = i["invoiceAddress"]["company"].ToString(),
                    address1 = i["invoiceAddress"]["address1"].ToString(),
                    address2 = i["invoiceAddress"]["address2"].ToString(),
                    city = i["invoiceAddress"]["city"].ToString(),
                    cityCode = i["invoiceAddress"]["cityCode"].ToString(),
                    district = i["invoiceAddress"]["district"].ToString(),
                    districtId = i["invoiceAddress"]["districtId"].ToString(),
                    countryCode = i["invoiceAddress"]["countryCode"].ToString(),
                    fullAddress = i["invoiceAddress"]["fullAddress"].ToString(),
                    fullName = i["invoiceAddress"]["fullName"].ToString()
                });
                content.invoiceAddress = invoiceAddresslist;

                if (i["packageHistories"].Count() != 0)
                {
                    foreach (var item in i["packageHistories"])
                    {
                        packageHistoriesList.Add(new PackageHistories
                        {
                            createdDate = Convert.ToInt64(item["createdDate"].ToString()),
                            status = item["status"].ToString()
                        });


                    }
                    content.packageHistories = packageHistoriesList;
                }



                lstContent.Add(content);
            }
            //Convert Timestamp to DateTime for saving database
            foreach (var item in lstContent)
            {
                item.DEstimatedDeliveryStartDate = UnixTimeStampToDateTime(item.estimatedDeliveryStartDate);
                item.DEstimatedDeliveryEndDate = UnixTimeStampToDateTime(item.estimatedDeliveryEndDate);
                foreach (var i in item.packageHistories)
                {
                    i.DcreatedDate = UnixTimeStampToDateTime(i.createdDate);
                }
            }
            //list is ready now we can save
            Console.WriteLine(lstContent.Count());

        }
    }
}