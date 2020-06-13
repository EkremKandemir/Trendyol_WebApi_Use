using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovi_Trendyol_Entegrasyonu.Model
{
    public class InvoiceAddress
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string cityCode { get; set; }
        public string district { get; set; }
        public string districtId { get; set; }
        public string countryCode { get; set; }
        public string fullAddress { get; set; }
        public string fullName { get; set; }
    }
}
