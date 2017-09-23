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
using BusinessObjects;
using BusinessObjects.Base;

namespace HaselOne.Controler.Tests
{
    [TestClass()]
    public class SalesmanControllerTests
    {
        [TestInitialize]
        public void Init()
        {
            OneMap.Config();
        }

        [TestMethod()]
        public void GetLocationsTest()
        {
            var uow = new UnitOfWork(new HASELONEEntities());
            var controller = new SalesmanController(uow, new CustomerService(uow));
            var res = controller.GetList(new SalesmanFilter() {CustomerId = 1071720 }) as ContentResult;
            Assert.IsNotNull(res.Content);
        }
    }
}