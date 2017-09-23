using BusinessObjects;
using BusinessObjects.Base;
using DAL;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using HaselOne.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;
using BusinessObjects.Base.StaticText;

namespace HaselOne.Controler
{
    [Authorize]
    public class MachineparkController : HaselBaseController
    {
        private readonly IMachineparkService _ms;

        public MachineparkController(IUnitOfWork uow, IMachineparkService ms) : base(uow)
        {
            _ms = ms;
        }


        [HttpPost]
        public ActionResult Save(MachineparkWrapper entity)
        {
            var existing = _ms.GetInstance<Cm_CustomerMachineparks>(_ => _.Id == entity.Id);

            if (entity != null && entity.RequestId.HasValue && entity.RequestId > 0)
            {
                Validations.Add(new TextValue(0, "Talep ile ilişkili makine parkını bu modulde düzenleyemezsiniz."));
                return ResultService(false, true, Text.Warning, validationMessages: Validations, resultType: ResultType.Warning);
            }
            if (entity.Id != 0)
                entity.CustomerId = existing.CustomerId;

            if (!entity.Validate())
                Validations.AddRange(entity.ValidationResult);

            if (entity.CategoryId > 0 && !_ms.IsMpCategoryAllowed(entity.CategoryId, entity.CustomerId))
                Validations.Add(new TextValue(0, "Bu kategoride makine parkı girebilmeniz için, aynı kategoride bir satıcı tanımlaması yapmanız gerekmektedir."));

            if (!string.IsNullOrEmpty(entity.SerialNo))
            {
                var existWithSerial = _ms.GetInstance<Cm_CustomerMachineparks>(_ => _.IsActive == true && _.IsDeleted == false && _.CategoryId == entity.CategoryId && _.MarkId == entity.MarkId && _.ModelId == entity.ModelId && _.SerialNo == entity.SerialNo);

                if (existWithSerial != null && existWithSerial.Id != entity.Id)
                    Validations.Add(new TextValue(0, "Belirtilen seri numarasına sahip aynı kategori, marka ve model için farklı bir kayıt mevcuttur."));
            }

            if (Validations.Count > 0)
                return ResultService(false, true, Text.Warning, validationMessages: Validations, resultType: ResultType.Warning);

            var res = _ms.SaveMachinepark(OneMap.mapper.Map<Cm_CustomerMachineparks>(entity));

            //_uow.SaveChanges();

            return ResultService(objectData: OneMap.mapper.Map<MachineparkWrapper>(res), resultType: ResultType.Success);
        }

        [HttpPost]
        public ActionResult Get(MachineparkFilter filter)
        {
            var res = _ms.GetMachineParks(_ => _.CustomerId == filter.CustomerId
                                                 && (!filter.Id.HasValue || _.Id == filter.Id)
                                                 && (_.IsDeleted == false && _.IsActive == true)
                                                 && ((filter.IsReleased && _.ReleaseDate.HasValue) || (!filter.IsReleased && !_.ReleaseDate.HasValue))
                                                 && (!filter.RequestId.HasValue || _.RequestId == filter.RequestId),
                                                //&& ((filter.HasRequest && _.RequestId != null) || _.RequestId == null), Bu seçim ile talepten gelenler gösterilmez.
                                                new string[] { "Cm_CustomerLocations", "Cm_MachineparkMark", "Cm_MachineparkCategory", "Gn_User1" })
                                                .OrderByDescending(_ => _.UpdateDate.HasValue ? _.UpdateDate : _.CreateDate)
                                                .ProjectTo<MachineparkWrapper>(OneMap.GetConfig());

            return ResultService(res.ToList());
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
            {
                Validations.Add(new TextValue(0, "Lütfen kayıt seçiniz."));
                return ResultService(false, true, "", null, Validations, ResultType.Warning);
            }

            var machineparks = _ms.GetMachineParks(_ => ids.Contains(_.Id)).ToList();

            if (machineparks == null)
                Validations.Add(new TextValue(0, "Silinecek kayıt bulunamadı."));
            else
            {
                var allparksChoosen = true;

                var customerIds = machineparks.Select(_ => _.CustomerId).Distinct().ToList();

                if (customerIds.Count > 1)
                    Validations.Add(new TextValue(0, "Cariye ait olmayan makine parkları silinemez."));
                else
                {
                    var allMps = _ms.GetMachineParks(_ => _.CustomerId == customerIds.FirstOrDefault() && _.IsDeleted != true && !_.ReleaseDate.HasValue).Select(_ => _.Id).ToArray();

                    for (int i = 0; i < allMps.Length; i++)
                    {
                        if (!ids.Contains(allMps[i]))
                        {
                            allparksChoosen = false;
                            break;
                        }
                    }

                    if (allparksChoosen && ids.Distinct().Count() == allMps.Length)
                        Validations.Add(new TextValue(0, "Cariye ait makine parklarının tamamı silinemez."));

                    if (machineparks.Where(_ => _.RequestId.HasValue).ToList().Count > 0)
                        Validations.Add(new TextValue(0, "Seçilenler arasında talepten gelen kayıt(lar) mevcut. Bu kayıtların silme işlemi bu modülden yapılamaz."));
                }
            }

            if (Validations.Count > 0)
                return ResultService(false, true, "", null, Validations, ResultType.Warning);
          
            foreach (var mp in machineparks)
            {
                mp.IsDeleted = true;
                _ms.SaveMachinepark(mp, false);
            }

            _uow.SaveChanges();

            return ResultService(true, true, "", null, null, ResultType.Success);
        }

        [HttpPost]
        public ActionResult Release(List<int> ids, DateTime? date)
        {
            if (ids == null)
            {
                Validations.Add(new TextValue(0, "Lütfen kayıt seçiniz."));
                return ResultService(false, true, "", null, Validations, ResultType.Warning);
            }
            var machineparks = _ms.GetMachineParks(_ => ids.Contains(_.Id)).ToList();
            if (machineparks == null)
                Validations.Add(new TextValue(0, "Güncellenecek kayıt bulunamadı."));

            var allparksChoosen = true;

            var customerIds = machineparks.Select(_ => _.CustomerId).Distinct().ToList();

            if (customerIds.Count > 1)
                Validations.Add(new TextValue(0, "Cariye ait olmayan makine parkları düzenlenemez."));
            else
            {
                var activeMps = _ms.GetMachineParks(_ => _.CustomerId == customerIds.FirstOrDefault() && _.IsDeleted != true && !_.ReleaseDate.HasValue).Select(_ => _.Id).ToArray();

                for (int i = 0; i < activeMps.Length; i++)
                {
                    if (!ids.Contains(activeMps[i]))
                    {
                        allparksChoosen = false;
                        break;
                    }
                }

                if (allparksChoosen && ids.Distinct().Count() == activeMps.Length)
                    Validations.Add(new TextValue(0, "Cariye ait makine parklarının tamamı elden çıkarılamaz."));
            }

            if (machineparks.Where(_ => _.SaleDate.HasValue && date.HasValue && _.SaleDate.Value > date).ToList().Count > 0)
                Validations.Add(new TextValue(0, "Seçilenler arasında satın alma tarihi, elden çıkarma tarihinden daha yeni kayıt(lar) mevcut."));

            if (Validations.Count > 0)
                return ResultService(false, true, "", null, Validations, ResultType.Warning);

            foreach (var machinepark in machineparks)
            {
                machinepark.ReleaseDate = date;
                _ms.SaveMachinepark(machinepark, false);
            }
            _uow.SaveChanges();

            return ResultService(true, true, "", null, null, ResultType.Success);
        }

        [HttpPost]
        public ActionResult Copy(int id, int customerId, int count)
        {
            var existing = _ms.GetInstance<Cm_CustomerMachineparks>(_ => _.Id == id && _.CustomerId == customerId);
            if (existing == null)
                Validations.Add(new TextValue(0, "Kopyalanacak kayıt bulunamadı."));
            else
            {
                if (count < 1)
                    Validations.Add(new TextValue(0, "Kopya adedi 1 den az olamaz."));

                if (count > 50)
                    Validations.Add(new TextValue(0, "Kopya adedi 50 den fazla olamaz."));

                if (existing.RequestId.HasValue)
                    Validations.Add(new TextValue(0, "Talepten gelen makine parkları bu modülden düzenlenemez."));
            }

            if (Validations.Count > 0)
                return ResultService(false, true, Text.Warning, validationMessages: Validations, resultType: ResultType.Warning);

            var wrapper = OneMap.mapper.Map<MachineparkWrapper>(existing);
            wrapper.Id = 0;
            wrapper.UpdateDate = null;
            wrapper.UpdateUserId = null;
            wrapper.SerialNo = "";
            for (int i = 0; i < count; i++)
            {
                var res = _ms.SaveMachinepark(OneMap.mapper.Map<Cm_CustomerMachineparks>(wrapper), false);
            }

            _uow.SaveChanges();

            return ResultService(message: string.Format("{0} adet makine eklenmiştir.", count), resultType: ResultType.Success);
        }

        public ActionResult MakeMachineForRequest()
        {
            return null;
            // return oneContent()
        }
    }
}