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
    public class LocationControllerTests
    {
        private UnitOfWork uow;
        private CustomerService _cs;
        [TestInitialize]
        public void Init()
        {
            uow = new UnitOfWork(new HASELONEEntities());
            _cs = new CustomerService(uow);
            OneMap.Config();
        }

        [TestMethod()]
        public void GetLocationsTest()
        {

            LocationController controller = new LocationController(uow, new CustomerService(uow));
            var res = controller.Get(new BusinessObjects.LocationFilter() { Name = "Merkez" }) as ContentResult;
            Assert.IsNotNull(res.Content);
        }

        [TestMethod()]
        public void GetTest()
        {
            var list = _cs.GetLocationBy(new LocationFilter() {CustomerId = 1071720});
            var list2 = _cs.GetLocationBy(new LocationFilter() {Id = 1019831 });
            var list4 = _cs.GetLocationBy(new LocationFilter() {  });
            Assert.Equals(list.Count , 4);
        }
    }
}