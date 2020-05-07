using LOIS.BLL.services;
using LOIS.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using LOIS.Web.App_Start;

namespace LOIS.Web.Controllers
{
    [AuthorizationRequired]
    public class RequisitionController : ApiController
    {
        private RequisitionService reqService;

        public RequisitionController()
        {
            reqService = new RequisitionService();
        }
        
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var req = await Task.Run(() => reqService.GetRequisitionById(id, loadDocs: false));

                return Ok(req);
            }
            catch (Exception ex)
            {
                return new System.Web.Http.Results.ExceptionResult(ex, this);
            }
        }

        [Route("api/requisition/{id:int}/DocumentUrls")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDocUrls(int id)
        {
            try
            {
                var docService = new DocumentService();

                var urls = new List<string>();
                var docs = await Task.Run(() => docService.GetDocumentsByReqId(id));

                foreach (var doc in docs)
                {
                    var name = $"{ doc.Id }.pdf";
                    //Write doc to disk.
                    var path = HttpContext.Current.Server.MapPath("~/Pdfs") + $"\\{name}";

                    if(!File.Exists(path))
                        System.IO.File.WriteAllBytes(path, doc.Document);

                    urls.Add(name);
                }

                return Ok(urls);
            }
            catch (Exception e)
            {
                return new ExceptionResult(e, this);
            }
        }
        

        [HttpGet]
        public async Task<IHttpActionResult> Get(string data = "")
        {
            try
            {               
                IEnumerable<Requisition> req = new List<Requisition>();

                if(string.IsNullOrEmpty(data))
                {
                    req = await Task.Run(() =>  reqService.GetRequisitions());
                }
                else
                {
                    var searchModel = JsonConvert.DeserializeObject<RequisitionSearch>(data);
                    req = await Task.Run(() =>  reqService.GetRequisitions(searchModel));
                }
                
                var serialized = JsonConvert.SerializeObject(req);

                return Ok(serialized);
            }
            catch (Exception ex)
            {
                return new System.Web.Http.Results.ExceptionResult(ex, this);
            }
        }
    }
}
