using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Server.Logic
{
    public class DataAccess
    {
        private SqlCommand Comando = null;
        private SqlConnection Conexion = null;

        public DataAccess(string connection) {
            try
            {
                Conexion = new SqlConnection(connection);
                Comando = new SqlCommand
                {
                    Connection = Conexion
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Connection error" + ex.Message);
            }
        }

        public int Execute(string Procedure, Dictionary<string, object> Parameters)
        {
            int Result = 0;
            try
            {
                Conexion.Open();
                Comando.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (var item in Parameters)
                {
                    Comando.Parameters.Add(new SqlParameter(item.Key, item.Value));
                }
                Comando.Parameters.Add(new SqlParameter("Result", 0));
                Comando.Parameters["Result"].Direction = ParameterDirection.InputOutput;
                Comando.CommandText = Procedure;
                Comando.ExecuteNonQuery();
                Result = int.Parse(Comando.Parameters["Result"].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Comando.Dispose();
                Conexion.Close();
                Conexion.Dispose();
            }
            return Result;
        }

        public DataTable Search(string Procedure, Dictionary<string, object> Parameters)
        {
            DataTable Result = new DataTable();
            try
            {
                Conexion.Open();
                Comando.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (var item in Parameters)
                {
                    Comando.Parameters.Add(new SqlParameter(item.Key, item.Value));
                }
                Comando.CommandText = Procedure;
                SqlDataReader Reader = Comando.ExecuteReader();
                Result.Load(Reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Comando.Dispose();
                Conexion.Close();
                Conexion.Dispose();
            }
            return Result;
        }

    }
}
