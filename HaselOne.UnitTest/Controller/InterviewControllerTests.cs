using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaselOne.Controler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HaselOne.Domain.UnitOfWork;
using DAL;
using HaselOne.Services.Services;
using System.Web.Mvc;
using BusinessObjects;
using BusinessObjects.Base;
using DAL.Helper;
using HaselOne.Services.Interfaces;
using Telerik.JustMock;

namespace HaselOne.Controler.Tests
{
    [TestClass()]
    public class InterviewControllerTests
    {
        private InterviewController controller;
        IUnitOfWork uow;
        [TestInitialize]
        public void Init()
        {
            //var userService = Mock.Create<IUserService>();
            // Mock.Create<IHttpSession>
            uow = new UnitOfWork(new HASELONEEntities());

            OneMap.Config();
        }

        [TestMethod]
        public void InterviewController_PrivateMethod_Test()
        {
            //var controller = Mock.Create<InterviewController>(m=>m.SetBehavior)


            var instance = new PrivateAccessor(new InterviewController(uow, new InterviewService(uow), new CustomerService(new UnitOfWork(new HASELONEEntities()))));
            var getListInterview = instance.CallMethod("GetListInterview");
            Assert.IsTrue(((List<Cm_Interview>)getListInterview).Count > 0);


            var GetInterviewImportant = instance.CallMethod("GetListInterviewImportant");
            Assert.IsTrue(((List<Gn_InterviewImportant>)GetInterviewImportant).Count > 0);



            var GetSalesmanListForAreaAndOperainTypeForLiad = instance.CallMethod("GetSalesmanListForAreaAndOperainTypeForLiad");
            Assert.IsTrue(((List<Cm_Interview>)GetSalesmanListForAreaAndOperainTypeForLiad).Count > 0);

        }

        [TestMethod]
        public void InterviewGetList()
        {
            var instansce = Mock.Create<IInterviewService>();
            var list = instansce.GetList(new CustomerInterviewsFilter()
            {
                CustomerId = 1071720
            });

            Assert.IsTrue(list.List.Count > 0);
        }
    }
}