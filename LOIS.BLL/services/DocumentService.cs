using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using LOIS.DAL.Lois;
using RequisitionDocument = LOIS.Core.Models.RequisitionDocument;

namespace LOIS.BLL.services
{
    public class DocumentService : IDisposable
    {
        private IMapper mapper;
        private loisEntities1 db;

        public DocumentService(loisEntities1 database = null)
        {
            mapper = mapperConfig.CreateConfig().CreateMapper();
            db = database ?? new DAL.Lois.loisEntities1();
            db.Configuration.LazyLoadingEnabled = false;
        }

        public RequisitionDocument GetDocument(int id)
        {
            var doc = db.RequisitionDocuments.FirstOrDefault(x => x.Id == id && x.MarkedDeleted == false);
            return mapper.Map<RequisitionDocument>(doc);
        }

        public IEnumerable<RequisitionDocument> GetDocumentsByReqId(int reqId)
        {
            var docs = db.RequisitionDocuments.Where(x => x.RequisitionId == reqId && x.MarkedDeleted == false);
            return mapper.Map<IEnumerable<RequisitionDocument>>(docs.ToList());
        }

        public void DeleteDocument(int documentId)
        {
            var doc = db.RequisitionDocuments.FirstOrDefault(x => x.Id == documentId);

            if (doc == null)
                return;

            //don't actually delete, but mark as deleted
            doc.MarkedDeleted = true;
            doc.MarkedDeletedDate = DateTime.Now;

            db.SaveChanges();
        }

        public bool SaveDocument(RequisitionDocument doc)
        {
            var docs = db.RequisitionDocuments.Where(x => x.RequisitionId == doc.RequisitionId);

            var docHash = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(doc.Document));

            if (!docs.Any(x => x.DocumentHash == docHash))
            {
                var dbDoc = mapper.Map<DAL.Lois.RequisitionDocument>(doc);
                dbDoc.DocumentHash = docHash;

                db.RequisitionDocuments.AddOrUpdate(dbDoc);
                db.SaveChanges();
            }

            return true;
        }

        #region IDisposable

        public void Dispose()
        {
            db?.Dispose();
        }

        #endregion
    }
}
