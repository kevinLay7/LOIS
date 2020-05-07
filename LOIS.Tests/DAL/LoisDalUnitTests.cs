using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LOIS.DAL;
using LOIS.BLL.services;
using LOIS.Core.Interfaces;
using LOIS.DAL.Lois;
using Patient = LOIS.Core.Models.Patient;
using Requisition = LOIS.Core.Models.Requisition;

namespace LOIS.TESTS.DAL
{
    [TestClass]
    public class LoisDalUnitTests
    {

        [TestMethod]
        public void GetRequisition()
        {
            var repo = new RequisitionService();
            //var a = repo.GetRequisitionById(1).Result;
        }

        [TestMethod]
        public void InsertRequistion()
        {
            var req = LOIS.BLL.Factories.RequisitionFactory.CreateRequisition(1, new FakeProlisService());
            var repo = new RequisitionService();
            repo.SaveRequisition(req);
            using (var b = new loisEntities1())
            {
                var r = b.Requisitions.First(x => x.RequisitionNo == 1);

                b.Requisitions.Remove(r);
                b.SaveChanges();
            }
        }

    }

    public class FakeProlisService : ILisService
    {
        public Requisition GetRequisitionById(int id)
        {
             return new Requisition()
             {
                 RequisitionNo = id
             };
        }

        public Patient GetPatientById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
