using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services.Tests
{
    [TestClass]
    public class TestUserPivotServices
    {
        private IUserPivotServices _userPivotServices;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestUserPivotServicesConstructor()
        {
            _unitOfWork = new UnitOfWork();
            _userPivotServices = new UserPivotServices(_unitOfWork);
        }

        [TestMethod]
        public void TestAuthenticate()
        {
            TestUserPivotServicesConstructor();
            var result =_userPivotServices.Authenticate("master","admin@421");
            Assert.AreEqual(result, 0);
        }
    }
}