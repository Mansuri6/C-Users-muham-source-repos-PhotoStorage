using PhotoStorage.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PhotoStorage.modules.Login.Data
{
    public class loginData
    {
        public dataResult register(login login)
        {
            var ret = new dataResult { isSuccess = true, errorMessage ="" };
            login.password = DbHelper.Encrypt(login.password);
            try
            {
                var parameters = new[]{
                    new SqlParameter("@email",login.email),
                    new SqlParameter("@fullName",login.fullName),
                    new SqlParameter("@password",login.password),
                };
                var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "register", parameters);
                if (ds.Tables.Count <= 0)
                {
                    ret.isSuccess = false;
                }
            }
            catch (SqlException se)
            {
                ret.errorMessage = se.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","LoginData"),
                    new SqlParameter("@function","register"),
                    new SqlParameter("@innerException",se.InnerException),
                    new SqlParameter("@exception",se.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (TimeoutException te)
            {
                ret.errorMessage = te.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","LoginData"),
                    new SqlParameter("@function","register"),
                    new SqlParameter("@innerException",te.InnerException),
                    new SqlParameter("@exception",te.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (Exception ex)
            {
                ret.errorMessage = ex.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","LoginData"),
                    new SqlParameter("@function","register"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }

            return ret;
        }

        public dataResult login(login user)
        {
            var ret = new dataResult { isSuccess = true, errorMessage = "",};
            ret.token = DbHelper.Encrypt(DateTime.Now.ToString("yy/MM/dd HH:mm:ss:mss").Replace("/", "").Replace(" ", "").Replace(":", "") + user.password);
            ret.login = new login { ID = 0 };
            user.password = DbHelper.Encrypt(user.password);
            try
            {
                var parameters = new[]{
                    new SqlParameter("@email",user.email),
                    new SqlParameter("@password",user.password),
                    new SqlParameter("@token",ret.token),
                };
                var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "login", parameters);

                if (ds.Tables[0].Rows.Count <= 0){
                    ret.isSuccess = false;
                }else{
                    ret.login.ID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
            }
            catch (SqlException se)
            {
                ret.errorMessage = se.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","LoginData"),
                    new SqlParameter("@function","login"),
                    new SqlParameter("@innerException",se.InnerException),
                    new SqlParameter("@exception",se.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (TimeoutException te)
            {
                ret.errorMessage = te.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","LoginData"),
                    new SqlParameter("@function","login"),
                    new SqlParameter("@innerException",te.InnerException),
                    new SqlParameter("@exception",te.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (Exception ex)
            {
                ret.errorMessage = ex.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","LoginData"),
                    new SqlParameter("@function","login"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }

            return ret;
        }


        public bool checkIfLogin (dataResult data)
        {
            var ret = false;
            try
            {
                var parameters = new[]{
                    new SqlParameter("@token",data.token),
                };
                var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "checkIfLogin", parameters);
                ret = ds.Tables[0].Rows.Count > 0;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
    }

    public class login
    {
        public int ID { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
        public string password { get; set; }
        public DateTime createdDate { get; set; }
    }
}