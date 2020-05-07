using System;
using LOIS.BLL.Factories;
using LOIS.BLL.services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LOIS.TESTS.DAL
{
    [TestClass]
    public class ProlisIntegration
    {
        [TestMethod]
        public void GetTestRequisition()
        {
            var id = 180316001;
            var req = RequisitionFactory.CreateRequisition(id, new ProlisService());
        }
    }
}
