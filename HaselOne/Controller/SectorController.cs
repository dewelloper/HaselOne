using AutoMapper.QueryableExtensions;
using BusinessObjects;
using BusinessObjects.Base;
using DAL;
using HaselOne.Controler;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaselOne.Controler
{
    public class SectorController : HaselBaseController
    {
        public readonly ICustomerService _cs;

        public SectorController(IUnitOfWork uow, ICustomerService cs) : base(uow)
        {
            _cs = cs;
        }

        [HttpPost]
        public ActionResult Get()
        {
            var res = _cs.GetListGeneric<Gn_Sector>().AsQueryable().ProjectTo<SectorWrapper>(OneMap.GetConfig()).ToList();

            return ResultService(res);
        }
    }
}