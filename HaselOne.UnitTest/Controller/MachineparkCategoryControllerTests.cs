﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaselOne.Controler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Base;
using HaselOne.Domain.UnitOfWork;
using DAL;
using HaselOne.Services.Services;
using System.Web.Mvc;

namespace HaselOne.Controler.Tests
{
    [TestClass()]
    public class MachineparkCategoryControllerTests
    {
        [TestInitialize]
        public void Init()
        {
            OneMap.Config();
        }

        [TestMethod()]
        public void GetTest()
        {
            var uow = new UnitOfWork(new HASELONEEntities());
            var controller = new MachineparkCategoryController(uow, new MachineparkService(uow));
            var res = controller.Get(new BusinessObjects.MachineparkCategoryFilter() { CustomerId = 1060005, SalesmanId = 1001057 }) as ContentResult;
            //var res = controller.Get(new BusinessObjects.MachineparkCategoryFilter() { CustomerId = null, SalesmanId = null }) as ContentResult;
            Assert.IsNotNull(res.Content);
        }
    }
}