
using PhotoStorage.modules.Login.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PhotoStorage.Class
{
    public static class DbHelper
    {
        public static string connection = ConfigurationManager.AppSettings["connection"];
        public static void SanitizeParameters(params SqlParameter[] parameters)
        {
            foreach (var param in parameters.Where(param => param.Value is string))
            {
                param.Value = param.Value.ToString().Trim();
            }
        }

        public static int ToInt(string value)
        {
            try
            {
                return int.Parse(string.IsNullOrEmpty(value) ? "0" : value);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DataSet ExecuteDataset(CommandType commandType, string commandName, List<SqlParameter> parameters, int timeOut = 0)
        {
            var ds = new DataSet();

            var con = new SqlConnection(connection);
            var com = new SqlCommand
            {
                Connection = con,
                CommandText = commandName,
                CommandType = commandType,
                CommandTimeout = timeOut
            };
            var da = new SqlDataAdapter(com);
            da.Fill(ds);
            com.Parameters.Clear();
            com.Dispose();
            con.Close();
            con.Dispose();
            return ds;
        }

        public static DataSet ExecuteDataset(CommandType commandType, string commandName, SqlParameter[] parameters, int timeOut = 0)
        {
            var ds = new DataSet();

            var con = new SqlConnection(connection);
            var com = new SqlCommand
            {
                Connection = con,
                CommandText = commandName,
                CommandType = commandType,
                CommandTimeout = timeOut
            };
            com.Parameters.AddRange(parameters);
            var da = new SqlDataAdapter(com);
            da.Fill(ds);
            com.Parameters.Clear();
            com.Dispose();
            con.Close();
            con.Dispose();
            return ds;
        }

        public static DataSet ExecuteDataset(CommandType commandType, string commandName, params SqlParameter[] parameters)
        {
            SanitizeParameters(parameters);

            var ds = new DataSet();
            const int timeOut = 0;

            var con = new SqlConnection(connection);
            var com = new SqlCommand
            {
                Connection = con,
                CommandText = commandName,
                CommandType = commandType,
                CommandTimeout = timeOut
            };

            com.Parameters.AddRange(parameters);

            var da = new SqlDataAdapter(com);

            da.Fill(ds);
            com.Parameters.Clear();
            com.Dispose();
            con.Close();
            con.Dispose();
            return ds;
        }


        public static DataTable ExecuteDataTable(CommandType commandType, string commandName, params SqlParameter[] parameters)
        {
            var ds = ExecuteDataset(commandType, commandName, parameters);

            return ds.Tables[0];
        }

        public static void ExecuteNonQuery(CommandType commandType, string commandName, params SqlParameter[] parameters)
        {
            const int timeOut = 0;
            var ds = new DataSet();
            var con = new SqlConnection(connection);
            var com = new SqlCommand
            {
                Connection = con,
                CommandText = commandName,
                CommandType = commandType,
                CommandTimeout = timeOut
            };

            com.Parameters.AddRange(parameters);
            var da = new SqlDataAdapter(com);

            da.Fill(ds);
            com.Parameters.Clear();
            com.Dispose();
            con.Close();
            con.Dispose();
        }

        public static string Encrypt(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }
    }

    public class dataResult
    {
        public bool isSuccess { get; set; }
        public string errorMessage { get; set; }
        public string token { get; set; }
        public login login { get; set; }
        public List<login> loginList { get; set; }
        //public album album { get; set; }
        //public List<album> albumList { get; set; }
    }
}