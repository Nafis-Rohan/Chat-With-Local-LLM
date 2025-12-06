using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatApiApp.Controllers
{
    public class LogoutController : ApiController
    {
        [HttpPost]
        [Route(("api/logout"))]
        public HttpResponseMessage Post()
        {
            try
            {
                var authHeader = Request.Headers.Authorization;
                var tkey = authHeader?.Parameter;   // Bearer <token> → Parameter = <token>

                if (string.IsNullOrWhiteSpace(tkey))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        new { Message = "Token not supplied" });
                }

                var res = AuthService.Logout(tkey);
                if (!res)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        new { Message = "Logout failed / token not found or already expired" });
                }

                return Request.CreateResponse(HttpStatusCode.OK,
                    new { Message = "Token Expired" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
