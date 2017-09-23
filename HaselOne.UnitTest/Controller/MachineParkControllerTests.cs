using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaselOne.Controler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaselOne.Domain.UnitOfWork;
using DAL;
using HaselOne.Services.Services;
using System.Web.Mvc;
using BusinessObjects.Base;
using BusinessObjects;
using Telerik.JustMock;
using HaselOne.Services.Interfaces;
using System.Diagnostics;

namespace HaselOne.Controler.Tests
{
    [TestClass()]
    public class MachineParkControllerTests
    {
            Stopwatch sp = new Stopwatch();
        IMachineparkService machineparkService; ICustomerService customerService;
        IUserService userService; IUnitOfWork uow;
        [TestInitialize]
        public void Init()
        {
            IMachineparkService machineparkService = Mock.Create<IMachineparkService>();
            ICustomerService customerService = Mock.Create<ICustomerService>();
            IUserService userService = Mock.Create<IUserService>();
            IUnitOfWork uow = Mock.Create<IUnitOfWork>();
            OneMap.Config();
        }

        [TestMethod()]
        public void Get()
        {
            var uow = new UnitOfWork(new HASELONEEntities());
            var controller = new MachineparkController(uow, new MachineparkService(uow));
            var filter = new MachineparkFilter();
            filter.CustomerId = 1057462;
            filter.Id = 1034524;
            filter.IsReleased = true;
            var res = controller.Get(filter) as ContentResult;
            Assert.IsNotNull(res.Content);
        }

        [TestMethod()]
        public void GetMachineParkCount()
        {
            sp.Start();

            //int ReqId = 1;
            //Mock.Arrange(() => machineparkService.GetMachineParkCount(ReqId)).Returns(2);

            var ms = new MachineparkService(uow);
           // ms.SaveMachinepark
            ms.GetMachineParkCount(77);
            sp.Stop();
            Assert.IsTrue(sp.Elapsed.Seconds > 2);
        }
    }
}