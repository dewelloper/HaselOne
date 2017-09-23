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
    public class RequestControllerTests
    {
        private CustomerRequestController controller;

        [TestInitialize]
        public void Init()
        {
           //var userService = Mock.Create<IUserService>();
          // Mock.Create<IHttpSession>
            var uow = new UnitOfWork(new HASELONEEntities());
            controller= new CustomerRequestController(uow, new UserService(uow), new CustomerService(uow), new MachineparkService(uow));
          
            OneMap.Config();
        }

        [TestMethod()]
        public void Get()
        {
         
            //var filter = new BusinessObjects.MachineModelFilter() {CategoryId = 10, MarkId = 25, Name = "n20-132" };
           
            var res = controller.GenerateViewMachinePark(17) as ContentResult;
            Assert.IsNotNull(res.Content);
        }

        [TestMethod]
        public void CopyMp()
        {
           // var mpExample = 
           //todo dinamik yapilmasi gerekiyor. mp nin
            var res = controller.CopyMp(1034561, 10);

        }

        [TestMethod]
        public void GetMachineList()
        {
            var sp = new Stopwatch();
            sp.Start();
            controller.GridList(new CustomerRequestFilter() { CustomerId = 1071720 }, RequestOpenCloseState.Close);
            sp.Stop();
            Assert.IsTrue(sp.Elapsed.Seconds < 1);
        }

        [TestMethod]
        public void SaveValid()
        {
         
        }
    }
}