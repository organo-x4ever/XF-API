using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Organo.Solutions.X4Ever.V1.DAL.API.Controllers;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using Organo.Solutions.X4Ever.V1.DAL.Services;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Tests
{
    [TestClass]
    public class TestUserPivotController
    {
        private UserPivotServices _userPivotServices;
        private UserMetaPivotServices _userMetaPivotServices;
        private UserTrackerPivotServices _userTrackerPivotServices;
        private PasswordHistoryServices _passwordHistoryServices;
        private UserTokensServices _userTokensServices;
        private UserPushTokenServices _userPushTokenServices;
        private UserNotificationServices _notificationServices;
        private EmailContent _emailContent;
        private UnitOfWork _unitOfWork;

        private void TestUserPivotController_Constructor()
        {
            _unitOfWork= new UnitOfWork();
             _userPivotServices = new UserPivotServices(_unitOfWork);
            _passwordHistoryServices = new PasswordHistoryServices(_unitOfWork);
            _userMetaPivotServices = new UserMetaPivotServices(_unitOfWork);
            _userTokensServices = new UserTokensServices(_unitOfWork);
            _userTrackerPivotServices = new UserTrackerPivotServices(_unitOfWork);
            _userPushTokenServices = new UserPushTokenServices(_unitOfWork);
            _emailContent = new EmailContent();
            _notificationServices = new UserNotificationServices(_unitOfWork);
        }

        [TestMethod]
        public async Task TextPostAuthUser_V2_ShouldReturnCorrectValue()
        {
            //this.TestUserPivotController_Constructor();
            //var controller = new UserPivotController(_userPivotServices,_passwordHistoryServices,_userMetaPivotServices,
            //    _userTokensServices,_userTrackerPivotServices,_emailContent,_userPushTokenServices,_notificationServices);

            //var controllerContext = new HttpControllerContext();
            //var request = new HttpRequestMessage();
            //request.Headers.Add("Token", "95601180-f325-4878-a5e9-ff1a983f3f39");

            //// Don't forget these lines, if you do then the request will be null.
            //controllerContext.Request = request;
            //controller.ControllerContext = controllerContext;

            //var result = await controller.PostAuthUser_V2() as HttpResponseMessage;
            //Assert.IsNotNull(result);
            //Assert.AreEqual(result.Content.ReadAsAsync<User>().Result.ID, result.Content.ReadAsAsync<User>().Result.ID);
        }
    }
}