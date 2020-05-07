using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LOIS.BLL.services;
using LOIS.Core.Models;
using Newtonsoft.Json;

namespace LOIS.Web.Controllers
{
    [AuthorizationRequired]
    public class UserController : ApiController
    {
        private UserService _service;
        

        public UserController()
        {
            _service =  new UserService();
        }

        //Get
        [HttpGet]
        public IHttpActionResult Get(string email)
        {
            try
            {
                var user = _service.GetUser(email);

                var serailized = JsonConvert.SerializeObject(user);
                return Ok(serailized);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        //[HttpPost]
        //public IHttpActionResult Create(string first, string last, string email, string password)
        //{
        //    try
        //    {
        //        var user = _service.CreateUser(first, last, email, password, false);
        //        return Ok(user);
        //    }
        //    catch (Exception e)
        //    {
        //        return InternalServerError();
        //    }
        //}


    }
}
