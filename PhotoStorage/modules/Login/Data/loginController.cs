using PhotoStorage.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PhotoStorage.modules.Login.Data
{
    public class loginController : ApiController
    {
        [HttpPost, Route("api/login/")]
        public HttpResponseMessage login(login login)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new loginData().login(login));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/checkIfLogin/")]

        public HttpResponseMessage checkIfLogin(dataResult utoken)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new loginData().checkIfLogin(utoken));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost, Route("api/register/")]

        public HttpResponseMessage register(login login)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new loginData().register(login));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }
    }
}
