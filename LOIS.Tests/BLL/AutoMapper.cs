using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using LOIS.BLL.Imaging;

namespace LOIS.TESTS.BLL
{
    [TestClass]
    public class AutoMapper
    {
        [TestMethod]
        public void TestMapperMappings()
        {
            var config = LOIS.BLL.services.mapperConfig.CreateConfig();
            config.AssertConfigurationIsValid();
        }
        
    }
}
