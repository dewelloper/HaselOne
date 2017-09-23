using BusinessObjects;
using BusinessObjects.Base;
using DAL;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using HaselOne.Util;

namespace HaselOne.Controler
{
    public class CustomerController : HaselBaseController
    {
        public readonly ICustomerService _cs;

        public CustomerController(IUnitOfWork uow, ICustomerService cs) : base(uow)
        {
            _cs = cs;
        }

        [HttpPost]
        public ActionResult Get(CustomerFilter filter)
        {
            var res = _cs.GetCustomers(filter).ToList();

            return ResultService(res);
        }

        [HttpPost]
        public ActionResult GetCustomerOptions(int comboId = 0)
        {
            try
            {
                var res = (from k in _cs.GetCombos(1) select new TextValue { Value = k.Key, Text = k.Value }).ToList();
                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }
    }
}