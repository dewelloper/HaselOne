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

namespace HaselOne.UnitTest.Controller
{
    [TestClass()]
    public class CustomerControllerTest
    {

        [TestMethod()]
        public void GetCustomersTest()
        {
            var uow = new UnitOfWork(new HASELONEEntities());
            var controller = new CustomerController(uow, new CustomerService(uow));
            var res = controller.Get(new CustomerFilter() { UserId = 1001327, Id=4 }) as ContentResult;
            Assert.IsNotNull(res.Content);
        }
    }
}
