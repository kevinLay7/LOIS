using System;
using System.Linq;
using System.Web.Http;
using LOIS.BLL.services;

namespace LOIS.Web.Controllers
{
    public class AuthenticationController : ApiController
    {

        [Route("login")]
        [Route("authenticate")]
        [Route("api/auth")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] UserLogin user)
        {
            try
            {
                var service = new UserService();
                var userObj = service.Authenticate(user.username, user.password);
                
                if (userObj != null)
                {
                    //Generate token
                    var tokenService = new TokenService();
                    var token = tokenService.GenerateToken(userObj.id);

                    var auth = new authentication();
                    auth.Expiration = token.expireson.Value;
                    auth.Token = token.authtoken;
                    auth.UserGroups = userObj.Groups.ToArray();
                    auth.email = user.username;

                    return Ok(auth);
                }

                return BadRequest("Invalid username or password");
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }

    public class authentication
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string[] UserGroups { get; set; }
        public string email { get; set; }
    }

    public class UserLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
