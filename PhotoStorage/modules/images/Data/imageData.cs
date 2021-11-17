using PhotoStorage.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoStorage.modules.Images.Data
{
    public class imageData
    {
        public dataResult createAlbum(album album)
        {
            var ret = new dataResult { isSuccess = true, errorMessage = "", };
            ret.album = new album { };
            try
            {
                var parameters = new[]{
                    new SqlParameter("@fName",album.fName),
                    new SqlParameter("@token",album.tokenId),
                    new SqlParameter("@id",album.id),
                };
                var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "createAlbum", parameters);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ret.isSuccess = false;
                        ret.album.id = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }
                }
            }
            catch(SqlException se)
            {
                ret.errorMessage = se.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","createAlbum"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","createAlbum"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","createAlbum"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }

            return ret;
        }

        public List<album> getAlbumByUser(album album)
        {
            var ret = new List<album>();
            var parameters = new[]{
                    new SqlParameter("@token",album.tokenId),
                };
            var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "getAlbumByUser", parameters);

            foreach(var d in ds.Tables[0].AsEnumerable())
            {
                ret.Add(new album
                {
                    id = Convert.ToInt32(d["id"]),
                    fName = d["albumName"].ToString(),
                    createdDate = Convert.ToDateTime(d["createdDate"])
                });
            }

            return ret;
        }

        public List<album> getAlbumByShared(album album)
        {
            var ret = new List<album>();
            var parameters = new[]{
                    new SqlParameter("@token",album.tokenId),
                };
            var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "getAlbumByShared", parameters);

            foreach (var d in ds.Tables[0].AsEnumerable())
            {
                ret.Add(new album
                {
                    id = Convert.ToInt32(d["id"]),
                    fName = d["albumName"].ToString(),
                    createdDate = Convert.ToDateTime(d["createdDate"])
                });
            }

            return ret;
        }

        public dataResult AddUpdateImages(HttpRequest request, int id, int albumId, string geolocation, string tags, string capturedBy, string capturedDate)
        {
            var ret = new dataResult { isSuccess = true, errorMessage = ""};
            try
            {
                var parameters = new[]
                    {
                        new SqlParameter("@id", id),
                        new SqlParameter("@album", albumId),
                        new SqlParameter("@geolocation", geolocation),
                        new SqlParameter("@tags", tags),
                        new SqlParameter("@capturedDate", capturedDate),
                        new SqlParameter("@capturedBy", capturedBy)
                    };
                var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "AddUpdateImages", parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    SaveImage(request, Convert.ToInt32(ds.Tables[0].Rows[0][0]), albumId);
                }

            }
            catch (SqlException se)
            {
                ret.errorMessage = se.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","AddUpdateImage"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","AddUpdateImage"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","AddUpdateImage"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }


            return ret;
        }

        public dataResult SaveImage(HttpRequest request, int id, int albumId)
        {
            var ret = new dataResult { isSuccess = true };
            try
            {
                string imageName = "";
                string FullPath = "";
                if (request.Files.Count > 0)
                {
                    for (var i = 0; i < request.Files.Count; i++)
                    {
                        var currentFile = request.Files[i];
                        var directory = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["imgs"] + "/" + albumId);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        
                        var filePath = Path.Combine(directory, id + Path.GetExtension(currentFile.FileName));
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        currentFile.SaveAs(filePath);

                        DbHelper.ExecuteNonQuery(CommandType.Text, "Update UserImages set imgEx = " + "'" + Path.GetExtension(currentFile.FileName) + "' where id = '" + id + "' ");
                    }
                }
            }
            catch (SqlException se)
            {
                ret.errorMessage = se.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SaveImage"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SaveImage"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SaveImage"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }

            return ret;
        }

        public dataResult deleteImage (images img)
        {
            var ret = new dataResult();
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "Delete from UserImages where id =" + img.id);
            }
            catch (SqlException se)
            {
                ret.errorMessage = se.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","deleteImage"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","deleteImage"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","deleteImage"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            return ret;
        }

        public dataResult DeleteAlbum(album album)
        {
            var ret = new dataResult();
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "Delete from AlbumUser where id =" + album.id + " Delete from UserImages where album =" + album.id);
            }
            catch (SqlException se)
            {
                ret.errorMessage = se.Message;
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","DeleteAlbum"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","DeleteAlbum"),
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
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","DeleteAlbum"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            return ret;
        }

        public dataResult SharedImagebyEmail(images img)
        {
            var ret = new dataResult { isSuccess = true};
            try
            {
                var parameters = new[]{
                    new SqlParameter("@imgId",img.id),
                    new SqlParameter("@shareTo",img.sharedTo),
                };
                var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "SharedImagebyEmail",parameters);
                if (ds.Tables.Count > 0){
                    ret.errorMessage = ds.Tables[0].Rows[0][0].ToString();
                    ret.isSuccess = false;
                }
            }
            catch (SqlException se)
            {
                ret.errorMessage = "please try to refresh and create again!";
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SharedImagebyEmail"),
                    new SqlParameter("@innerException",se.InnerException),
                    new SqlParameter("@exception",se.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (TimeoutException te)
            {
                ret.errorMessage = "please try to refresh and create again!";
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SharedImagebyEmail"),
                    new SqlParameter("@innerException",te.InnerException),
                    new SqlParameter("@exception",te.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (Exception ex)
            {
                ret.errorMessage = "please try to refresh and create again!";
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SharedImagebyEmail"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            return ret;
        }


        public dataResult SharedAlbumbyEmail(album album)
        {
            var ret = new dataResult { isSuccess = true };
            try
            {
                var parameters = new[]{
                    new SqlParameter("@albumId",album.id),
                    new SqlParameter("@shareTo",album.sharedTo),
                };
                var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "SharedAlbumbyEmail", parameters);
                if (ds.Tables.Count > 0)
                {
                    ret.errorMessage = ds.Tables[0].Rows[0][0].ToString();
                    ret.isSuccess = false;
                }
            }
            catch (SqlException se)
            {
                ret.errorMessage = "please try to refresh and create again!";
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SharedImagebyEmail"),
                    new SqlParameter("@innerException",se.InnerException),
                    new SqlParameter("@exception",se.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (TimeoutException te)
            {
                ret.errorMessage = "please try to refresh and create again!";
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SharedImagebyEmail"),
                    new SqlParameter("@innerException",te.InnerException),
                    new SqlParameter("@exception",te.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            catch (Exception ex)
            {
                ret.errorMessage = "please try to refresh and create again!";
                ret.isSuccess = false;
                var parameters = new[]{
                    new SqlParameter("@module","imageData"),
                    new SqlParameter("@function","SharedImagebyEmail"),
                    new SqlParameter("@innerException",ex.InnerException),
                    new SqlParameter("@exception",ex.Message),
                };
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "CreateErrorLogs", parameters);
            }
            return ret;
        }

        public List<images> getImagesByAlbumAndUser(album album)
        {
            var ret = new List<images>();
            var parameters = new[]{
                    new SqlParameter("@albumId",album.id),
                    new SqlParameter("@type",album.type),
                    new SqlParameter("@token",album.tokenId),
                };
            var ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, "getImagesByAlbumAndUser", parameters);

            foreach (var d in ds.Tables[0].AsEnumerable())
            {
                ret.Add(new images
                {
                    id = Convert.ToInt32(d["id"]),
                    albumId = Convert.ToInt32(d["album"]),
                    tags = d["tags"].ToString(),
                    geolocation = d["geolocation"].ToString(),
                    capturedBy = d["capturedBy"].ToString(),
                    imgEx = d["imgEx"].ToString(),
                    capturedDate = Convert.ToDateTime(d["capturedDate"]),
                    createdDate = Convert.ToDateTime(d["uploadDate"]),
                });
            }

            return ret;
        }
    }

    public class images
    {
        public int id { get; set; }
        public int albumId { get; set; }
        public string geolocation { get; set; }

        public string tags { get; set; }
        public DateTime capturedDate { get; set; }
        public string capturedBy { get; set; }
        public string imgEx { get; set; }
        public DateTime createdDate { get; set; }
        public string sharedTo { get; set; }

    }
}