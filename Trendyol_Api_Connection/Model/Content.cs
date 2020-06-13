using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovi_Trendyol_Entegrasyonu.Model
{
    public class Content
    {
        public List<ShipmentAddress> shipmentAddress { get; set; }
        public string orderNumber { get; set; }
        public double grossAmount { get; set; }
        public double totalDiscount { get; set; }
        public double totalPrice { get; set; }
        public string taxNumber { get; set; }
        public List<InvoiceAddress> invoiceAddress { get; set; }
        public string customerFirstName { get; set; }
        public string customerEmail { get; set; }
        public string customerId { get; set; }
        public string customerLastName { get; set; }
        public string id { get; set; }
        public long cargoTrackingNumber { get; set; }//date
        public string cargoProviderName { get; set; }
        public List<Lines> lines { get; set; }
        public long orderDate { get; set; }//date
        public string tcIdentityNumber { get; set; }
        public string currencyCode { get; set; }
        public List<PackageHistories> packageHistories { get; set; }
        public string shipmentPackageStatus { get; set; }
        public string deliveryType { get; set; }
        public long estimatedDeliveryStartDate { get; set; }//date
        public long estimatedDeliveryEndDate { get; set; }//date
        public DateTime DEstimatedDeliveryStartDate { get; set; }//date
        public DateTime DEstimatedDeliveryEndDate { get; set; }//date
        
    }
}
