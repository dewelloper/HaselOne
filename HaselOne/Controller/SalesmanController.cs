using System;
using System.Linq;
using BusinessObjects;
using BusinessObjects.Base;
using DAL;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System.Web.Mvc;
using HaselOne.Domain.Repository;

namespace HaselOne.Controler
{
    [Authorize]
    public class SalesmanController : HaselBaseController
    {
        private readonly ICustomerService _csService;

        public SalesmanController(IUnitOfWork uow, ICustomerService ms) : base(uow)
        {
            _csService = ms;
        }

        [HttpPost]
        public ActionResult GetList(SalesmanFilter filter)
        {
            var res = _csService.GetListGeneric<Cm_CustomerSalesmans>(filter: m => m.CustomerId == filter.CustomerId  && m.IsDeleted==false, includePaths:new []{nameof(Cm_CustomerSalesmans.Gn_User), nameof(Cm_CustomerSalesmans.Gn_User1)});
            var list = res.Select(m => new TextValue
            {
                Text  = m.Gn_User1.Name,
                Value   = Convert.ToInt32( m.SalesmanId),
            });
            return ResultService(objectData:list);
        }

      

   
    }
}