using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LOIS.BLL.services;

namespace LOIS.Web.Controllers
{
    public class ManageController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(ChangeRequest user)
        {
            try
            {
                var success = new UserService().ChangePassword(user.email, user.newPassword, user.oldPassword);

                if (success)
                    return Ok();

                return InternalServerError();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }
    }

    public class ChangeRequest
    {
        public string email { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
