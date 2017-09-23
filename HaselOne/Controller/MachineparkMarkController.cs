using BusinessObjects;
using BusinessObjects.Base;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using DAL;
using System.Collections.Generic;
using HaselOne.Util;

namespace HaselOne.Controler
{
    public class MachineparkMarkController : HaselBaseController
    {
        private readonly IMachineparkService _ms;

        public MachineparkMarkController(IUnitOfWork uow, IMachineparkService ms) : base(uow)
        {
            _ms = ms;
        }

        [HttpPost]
        public ActionResult Get(MachineparkMarkFilter filter)
        {
            if (!filter.CategoryId.HasValue) filter.CategoryId = 0;

            filter.IsActive = true;
            filter.IsDeleted = false;
            var res = _ms.GetMachineParkMark(filter).ProjectTo<MachineparkMarkWrapper>(OneMap.GetConfig()).ToList();
            return ResultService(res);
        }

        [HttpPost]
        public ActionResult Save(MachineparkMarkWrapper entity)
        {
            var existing = _ms.GetInstance<Cm_MachineparkMark>(_ => _.MarkName.ToLower() == entity.MarkName.ToLower());
            var warn = "Aynı isimde bir marka veri tabanında mevcut olmakla birlikte aktif olmayabilir ya silinmiş olabilir. Lütfen sistem yetkilisi ile görüşünüz.";

            if (existing != null)
                Validations.Add(new TextValue(0, warn));

            if (Validations.Count > 0)
                return ResultService(false, true, "", null, Validations, ResultType.Warning);

            var res = _ms.SaveMachineparkMark(OneMap.mapper.Map<Cm_MachineparkMark>(entity));
            return ResultService(OneMap.mapper.Map<MachineparkMarkWrapper>(res));
        }
    }
}