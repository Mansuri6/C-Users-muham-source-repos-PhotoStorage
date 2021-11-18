using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PhotoStorage.modules.Images.Data
{
    public class imageController : ApiController
    {
        [HttpPost, Route("api/createAlbum/")]
        public HttpResponseMessage createAlbum(album album)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().createAlbum(album));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/AddUpdateImages/")]
        public HttpResponseMessage AddUpdateImages(int id, int albumId, string geolocation, string tags, string capturedBy, string capturedDate)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().AddUpdateImages(HttpContext.Current.Request, id, albumId, geolocation, tags, capturedBy, capturedDate));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPost, Route("api/getAlbumByUser/")]
        public HttpResponseMessage getAlbumByUser(album album)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().getAlbumByUser(album));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/getAlbumByShared/")]
        public HttpResponseMessage getAlbumByShared(album album)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().getAlbumByShared(album));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/deleteImage/")]
        public HttpResponseMessage deleteImage(images img)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().deleteImage(img));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/DeleteAlbum/")]
        public HttpResponseMessage DeleteAlbum(album album)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().DeleteAlbum(album));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/SharedImagebyEmail/")]
        public HttpResponseMessage SharedImagebyEmail(images img)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().SharedImagebyEmail(img));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/SharedAlbumbyEmail/")]
        public HttpResponseMessage SharedAlbumbyEmail(album album)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().SharedAlbumbyEmail(album));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/getImagesByAlbumAndUser/")]
        public HttpResponseMessage getImagesByAlbumAndUser(album album)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new imageData().getImagesByAlbumAndUser(album));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }
    }


    public class album
    {
        public int id { get; set; }
        public string fName { get; set; }
        public string tokenId { get; set; }
        public DateTime createdDate { get; set; }
        public int userId { get; set; }
        public string type { get; set; }
        public string sharedTo { get; set; }
    }
}
    //public class                                                                                   