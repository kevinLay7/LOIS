using System;
using System.Web.Http;
using LOIS.BLL.services;
using LOIS.Core.Models;

namespace LOIS.Web.Controllers
{
    //Todo figure out how to add admin check
    [AuthorizationRequired]
    public class AdminController : ApiController
    {
        private UserService _userService;

        public AdminController()
        {
            _userService = new UserService();
        }

        [HttpGet]
        [Route("api/admin/users")]
        public IHttpActionResult Users()
        {
            try
            {
                var users = _userService.GetUsers();

                return Ok(users);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        [Route("api/admin/CreateUser")]
        public IHttpActionResult CreateUser(UserModel user)
        {
            try
            {
                var userObj = _userService.CreateUser(user.firstName, user.lastName, user.email, user.password,
                    user.isAdmin);
                return Ok(userObj);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }

    public class UserModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
    }
}
