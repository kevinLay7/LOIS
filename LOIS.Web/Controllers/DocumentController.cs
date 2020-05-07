using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using LOIS.BLL.services;

namespace LOIS.Web.Controllers
{
    [AuthorizationRequired]
    public class DocumentController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var service = new DocumentService();
                var doc = service.GetDocument(id);

                return Ok(doc);
            }
            catch (Exception e)
            {
                return new InternalServerErrorResult(Request);
            }
        }

        [Route("api/document/{id:int}/delete")]
        [HttpPost]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var service = new DocumentService();
                service.DeleteDocument(id);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
