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
using HaselOne.Util;

namespace HaselOne.Controler
{
    public class MachineModelController : HaselBaseController
    {
        public readonly IMachineparkService _ms;

        public MachineModelController(IUnitOfWork uow, IMachineparkService ms) : base(uow)
        {
            _ms = ms;
        }

        [HttpPost]
        public ActionResult Get(MachineModelFilter filter)
        {
            if (!filter.CategoryId.HasValue) filter.CategoryId = 0;
            if (!filter.MarkId.HasValue) filter.MarkId = 0;

            var res = _ms.GetMachineModel(filter).ProjectTo<MachineModelWrapper>(OneMap.GetConfig()).ToList();

            return ResultService(res);
        }

        [HttpPost]
        public ActionResult Save(MachineModelWrapper entity)
        {
            var existing = _ms.GetInstance<Pr_MachineModel>(_ => _.Name.ToLower() == entity.Name.ToLower()
                                                            && _.MarkId == entity.MarkId
                                                            && _.CategoryId == entity.CategoryId);
            var warn = "Belirlenen kategori ve marka için aynı isimde bir model veri tabanında mevcut olmakla birlikte aktif olmayabilir. Lütfen sistem yetkilisi ile görüşünüz.";

            if (existing != null)
                Validations.Add(new TextValue(0, warn));

            if (Validations.Count > 0)
                return ResultService(false, true, "", null, Validations, ResultType.Warning);

            var res = _ms.SaveMachineModel(OneMap.mapper.Map<Pr_MachineModel>(entity));
            return ResultService(OneMap.mapper.Map<MachineModelWrapper>(res));
        }
    }
}