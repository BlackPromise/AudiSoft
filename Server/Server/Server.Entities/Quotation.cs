using System;
using System.Collections.Generic;

namespace Server.Entities
{
    public class Quotation
    {
        public int QuotationId{ get; set; } 
        public string SessionValue{ get; set; } = "";
        public DateTime GenerateDate{ get; set; } 
        public string Client{ get; set; } = "";
        public string Ruc{ get; set; } = "";
        public string Seller { get; set; } = "";
        public List<QuotationDetail> Detail { get; set; } = new List<QuotationDetail>();
    }
}
