using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Server.Logic
{
    public class Quotation
    {
        private string connection;

        public Quotation(string _connection)
        {
            this.connection = _connection;
        }

        public Entities.Quotation All(string SessionValue)
        {
            Entities.Quotation Result = new Entities.Quotation();
            DataTable Result2 = new DataAccess(connection).Search("Sp_Quotation_Search", new Dictionary<string, object>() { { "SessionValue", SessionValue } }) ?? new DataTable();
            if (Result2.Rows.Count > 0)
            {
                Result = MapInstance.GetObject<Entities.Quotation>(Result2).FirstOrDefault();
                if (Result != null)
                {
                    Result.Detail = AllDetail(SessionValue);
                }
            }
            return Result ?? new Entities.Quotation();
        }

        private List<Entities.QuotationDetail> AllDetail(string SessionValue)
        {
            DataTable Result = new DataAccess(connection).Search("Sp_QuotationDetail_Search", new Dictionary<string, object>() { { "SessionValue", SessionValue } }) ?? new DataTable();
            if (Result.Rows.Count > 0)
            {
                return MapInstance.GetObject<Entities.QuotationDetail>(Result).ToList();
            }
            return new List<Entities.QuotationDetail>();
        }

        public int InsertOrUpdate(Entities.Quotation item)
        {
            int Result = new DataAccess(connection).Execute("Sp_Quotation_InsertOrUpdate", new Dictionary<string, object>() {
                { "SessionValue", item.SessionValue },
                { "GenerateDate", DateTime.Now },
                { "Ruc", item.Ruc ?? "" },
                { "Seller", item.Seller ?? "" },
                { "Client", item.Client ?? "" }
            });
            DeleteDetail(item.SessionValue);
            if (Result > 0)
            {
                item.QuotationId = Result;
                if (item.Detail != null)
                {
                    foreach (var group in item.Detail.GroupBy(x=>x.ProductId))
                    {
                        Entities.QuotationDetail detail = group.LastOrDefault();
                        detail.QuotationId = Result;
                        detail.Quantity = group.Sum(x => x.Quantity);
                        InsertDetail(detail);
                    }
                }
            }
            return Result;
        }

        public int Delete(string SessionValue)
        {
            DeleteDetail(SessionValue);
            return new DataAccess(connection).Execute("Sp_Quotation_Delete", new Dictionary<string, object>() {
                { "SessionValue", SessionValue }
            });
        }

        private int DeleteDetail(string SessionValue)
        {
            return new DataAccess(connection).Execute("Sp_QuotationDetail_Delete", new Dictionary<string, object>() {
                { "SessionValue", SessionValue }
            });
        }

        private int InsertDetail(Entities.QuotationDetail item)
        {
            return new DataAccess(connection).Execute("Sp_QuotationDetail_Insert", new Dictionary<string, object>() {
                { "ProductId", item.ProductId },
                { "QuotationId", item.QuotationId },
                {"Quantity",item.Quantity },
                { "Amount", item.Amount },
                { "Tax", item.Tax },
                { "Discount", item.Discount }
            });
        }

    }
}
