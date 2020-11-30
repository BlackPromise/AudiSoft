using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Server.Logic
{
    public static class MapInstance
    {
        public static List<T> GetObject<T>(DataTable dataTable)
        {
            List<T> Result = new List<T>();
            try
            {
                string SerializeObject = JsonConvert.SerializeObject(dataTable, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (SerializeObject != null)
                {
                    Result = (List<T>)JsonConvert.DeserializeObject(SerializeObject, typeof(List<T>));
                }
            }
            catch (JsonSerializationException)
            {
                try
                {
                    string SerializeObject = JsonConvert.SerializeObject(dataTable, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    var sb = new StringBuilder(SerializeObject);
                    sb[0] = string.Empty.FirstOrDefault();
                    sb[SerializeObject.Length - 1] = string.Empty.FirstOrDefault();
                    SerializeObject = sb.ToString();
                    Result = (List<T>)JsonConvert.DeserializeObject(SerializeObject, typeof(List<T>));
                }
                catch (Exception exc)
                { throw exc; }
            }
            catch (Exception ex)
            { throw ex; }
            return Result;
        }
    }
}
