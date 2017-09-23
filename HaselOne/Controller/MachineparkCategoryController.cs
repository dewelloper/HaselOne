using BusinessObjects;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using AutoMapper.QueryableExtensions;
using System.Web.Mvc;
using BusinessObjects.Base;
using System.Linq;
using System;
using DAL;
using HaselOne.Util;
using System.Collections.Generic;

namespace HaselOne.Controler
{
    [Authorize]
    public class MachineparkCategoryController : HaselBaseController
    {
        private readonly IMachineparkService _ms;

        public MachineparkCategoryController(IUnitOfWork uow, IMachineparkService ms) : base(uow)
        {
            _ms = ms;
        }

        [HttpPost]
        public ActionResult Get(MachineparkCategoryFilter filter)
        {
            var cats = _ms.GetMachineParkCategory(filter).ProjectTo<MachineparkCategoryWrapper>(OneMap.GetConfig()).ToList();

            var res = new List<MachineparkCategoryWrapper>();
            int level = 0;
            foreach (var item in cats)
            {
                GetSubMpCategories(cats, item, level);
                if (cats.Where(_ => _.Id == item.ParentId).FirstOrDefault() == null)
                {
                    item.TreeLevel = level;
                    res.Add(item);
                }
            }

            return ResultService(res.OrderBy(_ => _.OrderBy).ToList());
        }

        private void GetSubMpCategories(List<MachineparkCategoryWrapper> source, MachineparkCategoryWrapper main, int level)
        {
            level++;
            main.Categories = source.Where(_ => _.ParentId == main.Id).OrderBy(_ => _.OrderBy).ToList();
            foreach (var sub in main.Categories)
            {
                sub.TreeLevel = level;
                GetSubMpCategories(source, sub, level);
            }
        }

        [HttpPost]
        public ActionResult Save(MachineparkCategoryWrapper entity)
        {
            var existing = _ms.GetInstance<Cm_MachineparkCategory>(_ => _.CategoryName.ToLower() == entity.CategoryName.ToLower());
            var warn = "Aynı isimde bir kategori veri tabanında mevcut olmakla birlikte aktif olmayabilir ya silinmiş olabilir. Lütfen sistem yetkilisi ile görüşünüz.";

            if (existing != null)
                Validations.Add(new TextValue(0, warn));

            if (Validations.Count > 0)
                return ResultService(false, true, "", null, Validations, ResultType.Warning);

            var res = _ms.SaveMachineparkCategory(OneMap.mapper.Map<Cm_MachineparkCategory>(entity));
            return ResultService(OneMap.mapper.Map<MachineparkCategoryWrapper>(res));
        }
    }
}