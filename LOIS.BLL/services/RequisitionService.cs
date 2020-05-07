using AutoMapper;
using LOIS.Core.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LOIS.DAL.Lois;
using Patient = LOIS.Core.Models.Patient;
using Requisition = LOIS.Core.Models.Requisition;
using RequisitionDocument = LOIS.Core.Models.RequisitionDocument;

namespace LOIS.BLL.services
{
    public class RequisitionService
    {
        private IMapper mapper;

        public RequisitionService()
        {
            mapper = mapperConfig.CreateConfig().CreateMapper();
        }
        
        public IEnumerable<Requisition> GetRequisitions()
        {
            using(var db = new loisEntities1())
            {
                var output = new List<Requisition>();
                var reqs = db.Requisitions.ToList();

                foreach (var req in reqs)
                {
                    var mapped = mapper.Map<Requisition>(req);
                    output.Add(mapped);
                }

                return output;
            }
        }

        /// <summary>
        /// Used to get search results to and from the RequisitionSearchService
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<Requisition> GetRequisitions(RequisitionSearch search)
        {
            using (var db = new loisEntities1())
            {
                var output = new List<Core.Models.Requisition>();
                var reqs = RequisitionSearchService.Search(search, db).ToList();
                    
                foreach (var req in reqs)
                {
                    var patient = db.Patients.FirstOrDefault(x => x.PatientId == req.PatientId);
                    var patMap = mapper.Map<Patient>(patient);
                    var mapping = mapper.Map<Requisition>(req);
                    mapping.Patient = patMap;
                    output.Add(mapping);
                }

                return output;
            }
        }

        public Requisition GetRequisitionById(int id, bool loadDocs = true)
        {
            using (var db = new loisEntities1())
            {
                var req = db.Requisitions.FirstOrDefault(x => x.RequisitionNo == id);

                if (req == null)
                    return null;

                var model = mapper.Map<Requisition>(req);
                var patient = db.Patients.FirstOrDefault(x => x.PatientId == model.PatientId);
                if (patient != null)
                    model.Patient = mapper.Map<Patient>(patient);

                if (loadDocs)
                {
                    var docs = new DocumentService(db).GetDocumentsByReqId(id);
                    model.Documents = docs.ToList();
                }

                return model;
            }
        }

        public bool SaveRequisition(Requisition requisition)
        {            
            using (var db = new loisEntities1())
            {
                var mapping = mapper.Map<DAL.Lois.Requisition>(requisition);

                db.Requisitions.AddOrUpdate(mapping);
                db.SaveChanges();

                return true;
            }
        }
        

        public bool SavePatient(Patient pat)
        {
            using (var db = new loisEntities1())
            {
                var p = mapper.Map<DAL.Lois.Patient>(pat);

                db.Patients.AddOrUpdate(p);
                db.SaveChanges();

                return true;
            }
        }
    }
}
