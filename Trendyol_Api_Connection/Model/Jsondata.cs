using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovi_Trendyol_Entegrasyonu.Model
{
    //: IEnumerable
    public class Jsondata
    {
        public int page { get; set; }
        public int size { get; set; }
        public int totalPages { get; set; }
        public int totalElements { get; set; }
        public Content content { get; set; }
    }
}
