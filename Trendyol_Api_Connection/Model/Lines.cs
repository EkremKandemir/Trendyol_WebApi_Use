using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovi_Trendyol_Entegrasyonu.Model
{
    public class Lines
    {
        public string quantity { get; set; }
        public string productId { get; set; }
        public string salesCampaignId { get; set; }
        public string productSize { get; set; }
        public string merchantSku { get; set; }
        public string productName { get; set; }
        public string productCode { get; set; }
        public string merchantId { get; set; }
        public double amount { get; set; }
        public double discount { get; set; }
        public List<DiscountDetails> discountDetails { get; set; }
        public string currencyCode { get; set; }
        public string productColor { get; set; }
        public string id { get; set; }
        public string sku { get; set; }
        public int vatBaseAmount { get; set; }
        public string barcode { get; set; }
        public string orderLineItemStatusName { get; set; }
        public double price { get; set; }
    }
}
