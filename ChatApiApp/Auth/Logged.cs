using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ChatApiApp.Auth
{
    public class Logged : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            // 1) No header → unauthorized
            if (authHeader == null || string.IsNullOrWhiteSpace(authHeader.Parameter))
            {
                actionContext.Response =
                    actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { Msg = "Token not supplied" });

                return;
            }

            // 2) Get only the token value (key), not whole header
            var tkey = authHeader.Parameter;   // this is what we stored in DB

            // 3) Validate with AuthService
            if (!AuthService.IsTokenValid(tkey))
            {
                actionContext.Response =
                    actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { Msg = "Token is Invalid Or Expired" });

                return;
            }

            // 4) All good → continue pipeline
            base.OnAuthorization(actionContext);
        }
    }
}