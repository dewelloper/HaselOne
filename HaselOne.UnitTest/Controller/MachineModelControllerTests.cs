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

namespace HaselOne.Controler.Tests
{
    [TestClass()]
    public class MachineModelTests
    {
        [TestInitialize]
        public void Init()
        {
            OneMap.Config();
        }

        [TestMethod()]
        public void Get()
        {
            var uow = new UnitOfWork(new HASELONEEntities());
            var controller = new MachineModelController(uow, new MachineparkService(uow));
            var filter = new BusinessObjects.MachineModelFilter() { CategoryId = 10, MarkId = 25, Name = "n20-132" };

            var res = controller.Get(filter) as ContentResult;
            Assert.IsNotNull(res.Content);
        }
    }
}