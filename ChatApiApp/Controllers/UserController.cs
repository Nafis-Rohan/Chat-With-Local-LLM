using BLL.DTOs;
using BLL.Services;
using ChatApiApp.Auth;
using ChatApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatApiApp.Controllers
{
    
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        // GET api/user/all
        [Logged]
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage Get()
        {
            try
            {
                var data = UserService.Get();
                if (data == null || data.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No data found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/user/all/5
        [Logged]
        [HttpGet]
        [Route("all/{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var user = UserService.Get(id);
                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No data found");

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // model for create/register (includes password)
        

        // POST api/user/create
        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(RegisterModel model)
        {
            try
            {
                var dto = new UserDTO
                {
                    UserName = model.UserName,
                    Name = model.Name
                };

                var result = UserService.Create(dto, model.Password);
                if (!result)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be created");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "User created");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/user/update
        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(UserDTO u)
        {
            try
            {
                // this version does NOT change password
                var result = UserService.Update(u);
                if (!result)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Update failed");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Update successful");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/user/delete/5
        [HttpDelete]
        [Route("delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var result = UserService.Delete(id);
                if (!result)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Delete successful");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/user/logout/5
        /*[HttpPost]
        [Route("logout/{id}")]
        public HttpResponseMessage Logout(int id)
        {
            try
            {
                var res = AuthService.Logout(id);
                if (!res)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { msg = "Logout failed / user not found" });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { msg = "Token Expired" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }*/
    }
}
