using System;
using LOIS.BLL.services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LOIS.TESTS.BLL
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void CreateUser()
        {
            var serv = new UserService();
            var user = serv.CreateUser("kevin", "lay", "kevin@kevinlay.net", "6Eku082790", true);
        }

        [TestMethod]
        public void GetUser()
        {
            var serv = new UserService();
            var user = serv.GetUser("kevin@kevinlay.net");
            Assert.IsNotNull(user);
        }
    }
}
