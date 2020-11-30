using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Server.Logic
{
    public class Product
    {
        private string connection;

        public Product(string _connection) {
            this.connection = _connection;
        }

        public List<Entities.Product> All()
        {
            DataTable Result = new DataAccess (connection).Search ("Sp_Product_Search", new Dictionary<string, object>()) ?? new DataTable();
            if (Result.Rows.Count > 0)
            {
                return MapInstance.GetObject <Entities.Product>(Result).ToList();
            }
            return new List<Entities.Product>();
        }

    }
}
