using BLL.DTOs;
using BLL.Services;
using ChatApiApp.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ChatApiApp.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/chat")]
    public class ChatController : ApiController
    {
        [Logged]
        [HttpPost]
        [Route("send")]
        public async Task<HttpResponseMessage> Send(ChatRequestDTO req)
        {
            try
            {
                var authHeader = Request.Headers.Authorization;
                var tkey = authHeader?.Parameter;

                var username = AuthService.GetUserNameFromToken(tkey);
                if (username == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        new { Message = "Invalid token" });

                // find user by username via BLL
                var users = UserService.Get();
                var user = users.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        new { Message = "User not found" });

                var reply = await ChatService.SendMessage(user.Id, req.Message);
                return Request.CreateResponse(HttpStatusCode.OK, reply);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Logged]
        [HttpGet]
        [Route("history")]
        public HttpResponseMessage History()
        {
            try
            {
                var authHeader = Request.Headers.Authorization;
                var tkey = authHeader?.Parameter;

                var username = AuthService.GetUserNameFromToken(tkey);
                if (username == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        new { Message = "Invalid token" }); 

                var users = UserService.Get();
                var user = users.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        new { Message = "User not found" });

                var data = ChatService.GetHistory(user.Id, 30);
                if (data == null || data.Count == 0)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No data found");

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
