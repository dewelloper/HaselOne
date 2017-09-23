using BusinessObjects;
using DAL;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using BusinessObjects.Base;

namespace HaselOne.Controler
{
    public class MachineparkYearController : HaselBaseController
    {
        private readonly ICustomerService _cs;

        public MachineparkYearController(IUnitOfWork uow, ICustomerService cs) : base(uow)
        {
            _cs = cs;
        }

        [HttpPost]
        public ActionResult Get(YearFilter filter)
        {
            var res = _cs.GetListGeneric<Cm_MachineparkYear>(_ => (!filter.Id.HasValue || _.Id == filter.Id)
                                                               && (!filter.Year.HasValue || _.Year == filter.Year)).ToList();

            return ResultService(res);
        }
    }
}