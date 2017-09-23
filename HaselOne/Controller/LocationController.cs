using BusinessObjects;
using DAL;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper.QueryableExtensions;
using BusinessObjects.Base;

namespace HaselOne.Controler
{
    public class LocationController : HaselBaseController
    {
        public readonly ICustomerService _cs;

        public LocationController(IUnitOfWork uow, ICustomerService cs) : base(uow)
        {
            _cs = cs;
        }

        [HttpPost]
        public ActionResult Get(LocationFilter filter)
        {
           var res= _cs.GetLocationBy(filter).AsQueryable().ProjectTo<LocationWrapper>(OneMap.GetConfig());
            //var res = _cs.GetListGeneric<Cm_CustomerLocations>(m => m.IsDeleted == filter.IsDelete 
            //                                                       && (filter.CustomerId!=0 && m.CustomerId == filter.CustomerId)
            //                                                       && (filter.LocationId != 0 || m.Id == filter.LocationId))
            //                                                       //&& (string.IsNullOrEmpty(filter.Name) || _.Name.ToLower().Contains(filter.Name.ToLower())
            //                                                      .AsQueryable().ProjectTo<LocationWrapper>(OneMap.GetConfig()).ToList();

            return ResultService(res);
        }

        [HttpPost]
        [OutputCache(Duration = 10,Location = OutputCacheLocation.Client)]
        public ActionResult Lookup(LocationFilter filter )
        {
            var res = _cs.GetListGeneric<Cm_CustomerLocations>(_ => _.IsDeleted == false
                                                                    &&
                                                                    (!filter.CustomerId.HasValue ||
                                                                     _.CustomerId == filter.CustomerId))
                .AsQueryable().Select(m => new
                {
                   m.Id,
                   m.Name

                }).ToList();

            return ResultService(res);
        }
    }
}