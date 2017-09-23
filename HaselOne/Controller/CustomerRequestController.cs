using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using BusinessObjects;
using BusinessObjects.Base;
using BusinessObjects.Base.StaticText;
using DAL;
using DAL.Helper;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using HaselOne.Services.Services;
using HaselOne.Util;

namespace HaselOne.Controler
{
    public class CustomerRequestController : HaselBaseController
    {
        public readonly IUserService _us;
        public readonly ICustomerService _cs;
        public readonly IMachineparkService _ms;

        public CustomerRequestController(IUnitOfWork uow, IUserService us, ICustomerService cs, IMachineparkService ms) : base(uow)
        {
            _us = new UserService(new UnitOfWork(new HASELONEEntities()));
            _cs = new CustomerService(new UnitOfWork(new HASELONEEntities()));
            _ms = new MachineparkService(new UnitOfWork(new HASELONEEntities()));
        }

        [HttpPost]
        public ActionResult GetListConnectionChannel()
        {
            var res = _us.ListConnectionChannel().ToList();
            return ResultService(objectData: res);

        }

        [HttpPost]
        public ActionResult ChangeResultType(int requestId, int resultType)
        {
            var item = this.GetRequestItem(new CustomerRequestFilter() { Id = requestId });
            int lastResult = item.ResultType;
            if (lastResult !=(int)eResultType.Bekliyor)
            {
                return ResultService(isValid: false, message: "Satış veya Kayıp satış degiştirilemez. Sadece bekleyen talepler değiştirilebilir", resultType: ResultType.Warning);
            }

            if (resultType == (int)eResultType.Satis ||resultType == (int)eResultType.KayipSatis)
            {
                if (item.ConditionType == (int)eConditionType.Hepsi ||  item.ConditionType.IsNullOrZero())
                {
                    return ResultService(false,false,"Kondisyon tipi boş veya Hepsi olamaz", resultType:ResultType.Warning);
                }

                if (item.SalesType == (int)eSalesType.Hepsi || item.SalesType.IsNullOrZero())
                {
                    return ResultService(false, false, "Satış tipi boş veya Hepsi olamaz", resultType: ResultType.Warning);
                }

                if (item.SalesType == (int)eSalesType.Kiralik)
                    {
                    return ResultService(false, false, "Satış tipi kiralik ise satışı sonlandıramazsınız.", resultType: ResultType.Warning);

                    }
                
            }
            int q = Convert.ToInt32(item.Quantity);
            item.ResultType = resultType;
            var vm = OneMap.mapper.Map<CustomerRequestWrapper>(item);
            SaveRequest(vm);
            if (resultType == (int)eResultType.Satis || resultType == (int)eResultType.KayipSatis)
            {
                this.GenerateViewMachinePark(requestId);
            }
            return ResultService(message: $@"Durum değiştirilmiştir. Eski Durum: {dicStaticList.dicResultType[lastResult]} 
                                          Yeni Durum: {dicStaticList.dicResultType[item.ResultType]} 
                                          {q} adet makine parkı olusturulmuştur", resultType: ResultType.Success);
        }
        [HttpPost]
        public ActionResult Save(CustomerRequestWrapper vm, bool? isDelete = false)
        {
            vm.InsertPre();
            Cm_CustomerRequest item = null;
           

            if (vm.Id > 0)
            {
                item = GetRequestItem(new CustomerRequestFilter() { Id = vm.Id });
            }
           

            if (isDelete != null && (bool)isDelete)//silmede validasyon bakmaz
            {
                vm.IsDeleted = true;
            }
            else
            {
                if (!vm.Validate() || !vm.DbValidate(item, getCategoryName(item)))
                {
                    return ResultService(false, true, Text.Warning, validationMessages: vm.ValidationResult, resultType: ResultType.Warning);
                }
            }


            SaveRequest(vm);

            if (isDelete != null && (vm.Id > 0 || isDelete.Value))
            {
                var count = UpdateAllMachineParkCategorySale(vm);
                string str = "Talep güncellenmiştir. ";
                if (count>0)
                {
                    str += " Talebin tüm makinelerinde Kategori ve Satış tarihi değişmiştir";
                }
                return ResultService(objectData: vm, message: str, resultType: ResultType.Success);

            }
         

            return ResultService(objectData: vm, message: "Talep kaydedilmiştir.", resultType: ResultType.Success);

        }

        [HttpPost]
        public ActionResult CopyMp(int MachineParkId, int Count)
        {
            try
            {
                if (Count > 50)
                {
                    ResultService(message: "50 den fazla olamaz", resultType: ResultType.Warning);
                }
                var mp = _ms.GetMachinePark(m => m.Id == MachineParkId);

                for (int i = 0; i < Count; i++)
                {
                    var item = OneMap.mapper.Map<MachineparkWrapper>(mp);
                    item.Id = 0;
                    item.UpdateDate = null;
                    item.UpdateUserId = null;
                    SaveMachinePark(OneMap.mapper.Map<MachineparkWrapper>(item), RequestMachineParkSave.CopyMp);

                }

                RequestQuantityUpdate(Convert.ToInt32(mp.RequestId), MachineParkList(Convert.ToInt32(mp.RequestId)).Count());
            }
            catch (Exception e)
            {
                return ResultService(isSuccess: false, message: "hata " + e.Message, resultType: ResultType.Error);
            }

            return ResultService(message: $"{Count} adet makine eklenmiştir", resultType: ResultType.Success);

        }


        private string getCategoryName(Cm_CustomerRequest entity)
        {
            if (entity != null)
            {
                var item = _ms.GetMachineParkCategory(new MachineparkCategoryFilter() { Id = entity.CategoryId }).FirstOrDefault();
                if (item != null)
                {
                    return item.CategoryName;
                }
               
            }
            return string.Empty;
            

        }
        private void SaveRequest(CustomerRequestWrapper vm)
        {
            var request = _cs.CustomerRequestInsertOrUpdate(vm);

            //  UpdateAllMachineParkCategorySale(vm);
        }

        private int UpdateAllMachineParkCategorySale(CustomerRequestWrapper vm)
        {
            var list = _ms.GetMachineParks(m => m.RequestId == vm.Id).ToList();
            foreach (var item in list)
            {
                Cm_CustomerMachineparks item2 = _ms.GetMachineParks(m => m.Id == item.Id).FirstOrDefault();
                item2.CategoryId = Convert.ToInt32(vm.CategoryId);
                item2.SaleDate = vm.EstimatedBuyDate;
                _ms.SaveMachinepark(item);
            }

            return list.Count;
        }

        [HttpPost]
        public ActionResult GridList(CustomerRequestFilter customerRequestFilter, RequestOpenCloseState openCloseMode)
        {
          
            customerRequestFilter.OpenClose = openCloseMode;

            var list = _cs.GetListCustomerRequest(customerRequestFilter);

            var resultList = new List<RequestVm>();
            foreach (var m in list)
            {
                var e = new RequestVm
                {
                    Id = m.Id,
                    IdForCommand = m.Id,
                    CustomerId = m.CustomerId,
                    ModelName = m.Pr_MachineModel?.Name,
                    Quantity = m.Quantity,
                    EstimatedBuyDate = m.EstimatedBuyDate,
                    Channel = m.Gn_ConnectionChannel?.Name,
                    SalesType = NullControl(dicStaticList.dicSalesType, m.SalesType),
                    ConditionType = NullControl(dicStaticList.dicConditionType, m.ConditionType),
                    CategoryName = m.Cm_MachineparkCategory?.CategoryName,
                    CategoryId = m.CategoryId,
                    MarkName = m.Cm_MachineparkMark?.MarkName,
                    MarkId = m.MarkId,
                    Owner = m.Gn_User2?.UserName,
                    RequestDate = m.RequestDate,
                    Salesman = m.Gn_User3?.UserName,
                    MonthlyWorkingHours = m.MonthlyWorkingHours,
                    ResultType = m.ResultType,
                    UseDurationFull =
                        m.UseDuration + " " + NullControl(dicStaticList.dicUseDurationUnitList, m.UseDurationUnit),
                    UseDuration = m.UseDuration,
                    ResultText = NullControl(dicStaticList.dicResultType, m.ResultType),
                    UpdateDate = m.UpdateDate,
                    CreateDate = m.CreateDate,
                    //order alani 
                };

                if (customerRequestFilter.OpenClose == RequestOpenCloseState.Close)
                    e.SerialNoHasntMacCount = _ms.GetMachineParkCount(e.Id);//m.Cm_CustomerMachineparks.Count(l => l.IsDeleted == false && String.IsNullOrEmpty(l.SerialNo));
               
                resultList.Add(e);
            }
           
            /*
             Açık taleplerde güncel olan en üstte sıralanır.
            Kapalı taleplerde satınalma tarihi en yeni olan en üstte olmalı.
             */
            if (customerRequestFilter.OpenClose == RequestOpenCloseState.Open)
            {
                resultList = resultList.OrderByDescending(m => m.UpdateDate.HasValue ? m.UpdateDate : m.CreateDate).ToList();
                   
            }
             

            if (customerRequestFilter.OpenClose == RequestOpenCloseState.Close)
                resultList = resultList.OrderByDescending(m => m.EstimatedBuyDate).ToList();

            return ResultService(objectData: resultList); //Json(new {Data= list, IsSuccess=true});
          
             
        }

      

        public List<Cm_CustomerLocations> GetCustomerLocation(int customerId)
        {
            //_cs.ge
            return null;
        }

        [HttpPost]
        public ActionResult Get(CustomerRequestFilter customerRequestFilter)
        {
            var item = GetRequestItem(customerRequestFilter);
            var a = OneMap.mapper.Map<CustomerRequestWrapper>(item);
            return Content(Result.Get(a));
        }

        private Cm_CustomerRequest GetRequestItem(CustomerRequestFilter customerRequestFilter)
        {
            Cm_CustomerRequest item = _cs.GetListCustomerRequest(customerRequestFilter).Select(m => m).ToList()[0];
            return item;
        }



        private string NullControl(Dictionary<int, string> dic, int? key)
        {
            if (key != null)
            {
                if (dic.ContainsKey(Convert.ToInt32(key)))
                    return dic[Convert.ToInt32(key)];
            }
            return string.Empty;
        }

        [HttpPost]
        public ActionResult GenerateViewMachinePark(int requestId, bool dontExitsCreate = true)
        {

            if (dontExitsCreate)
            {
                var createStatus = false;
                int customerId = 0;
                int reqQuantity = 0;

                var listMachine = MachineParkList(requestId);
                if (listMachine.Count == 0)
                {
                    var req = _cs.GetListGeneric(m => m.Id == requestId, new[] { "Pr_MachineModel", "Cm_MachineparkCategory" }, sortExpressions: new SortExpression<Cm_CustomerRequest>(m => m.Id, ListSortDirection.Ascending))[0];

                    for (int i = 0; i < req.Quantity; i++)
                    {
                        createStatus = true;
                        var mp = new MachineparkWrapper();

                        if (req.CategoryId != null) mp.CategoryId = req.CategoryId.Value;
                        //if(req.ResultType == )
                        //mp.MarkId = req.MarkId;

                        mp.Quantity = 1;
                        mp.SaleDate = req.EstimatedBuyDate;
                        mp.CustomerId = req.CustomerId;
                        mp.IsActive = true;
                        mp.IsDeleted = false;
                        //mp.ModelId = req.ModelId;
                        mp.RequestId = req.Id;

                        var a = OneMap.mapper.Map<Cm_CustomerMachineparks>(mp);
                        _ms.SaveMachinepark(a);
                    }
                    _uow.SaveChanges();
                }

                var list = MachineParkList(requestId);
                if (createStatus)
                    return ResultService(objectData: list, resultType: ResultType.Success, message: $"{list.Count} adet makine olusturuldu");

                return ResultService(objectData: list);
            }
            else
            {
                var list = MachineParkList(requestId);
                return ResultService(objectData: list);
            }
        }



        private List<MachineparkWrapper> MachineParkList(int requestId)
        {
            var list = _cs.GetListGeneric(m => m.RequestId == requestId && m.IsDeleted == false,
                sortExpressions: new SortExpression<Cm_CustomerMachineparks>(m => m.Id, ListSortDirection.Descending))
                //.AsQueryable()
                //.ProjectTo<MachineparkWrapper>(OneMap.GetConfig())
                .ToList();
            
            List<MachineparkWrapper> listDest = OneMap.mapper.Map<List <Cm_CustomerMachineparks> ,List <MachineparkWrapper>>(list);
            return listDest;
        }

        [HttpPost]
        public ActionResult SaveMachinePark(MachineparkWrapper parameter, RequestMachineParkSave mode)
        { 
              string strMessage = "İşlemi tamamlandı";

            var req = GetRequestItem(new CustomerRequestFilter() { Id = Convert.ToInt32(parameter.RequestId) });
             
            if (mode == RequestMachineParkSave.CopyMp)
            {
                parameter.CategoryId = Convert.ToInt32(req.CategoryId);
                parameter.SaleDate = req.EstimatedBuyDate;
                _ms.SaveMachinepark(OneMap.mapper.Map<Cm_CustomerMachineparks>(parameter));
                return ResultService(resultType: ResultType.Success, message: "Makine kopyalama işlemi tamamlandı.");
            }

            if (mode == RequestMachineParkSave.MpGridUpdateInsert)
            {
              

                _ms.SaveMachinepark(OneMap.mapper.Map<Cm_CustomerMachineparks>(parameter));
                RequestQuantityUpdate(Convert.ToInt32(parameter.RequestId), MachineParkList(Convert.ToInt32(parameter.RequestId)).Count());

                if (parameter.Id != 0)
                {
                    var item = SyncOtherMachinePark(parameter);
                    if (item.Item1)
                    {
                        strMessage += item.Item2;
                    }
                   
                }
                return ResultService(resultType: ResultType.Success, message: strMessage);
            }

            if (mode == RequestMachineParkSave.Delete)
            {
                parameter.IsDeleted = true;
                _ms.SaveMachinepark(OneMap.mapper.Map<Cm_CustomerMachineparks>(parameter));
                RequestQuantityUpdate(Convert.ToInt32(parameter.RequestId), MachineParkList(Convert.ToInt32(parameter.RequestId)).Count());
                return ResultService(resultType: ResultType.Success, message: "Silme işlemi tamamlandı.");
            }
            return ResultService(resultType: ResultType.Error, message: "Hata");
        }

        private Tuple<bool, string> SyncOtherMachinePark(MachineparkWrapper p)
        {
            //var mp = new MachineparkController(this._uow, new MachineparkService(new UnitOfWork(new HASELONEEntities())));

            try
            {

                var list = this.MachineParkList(Convert.ToInt32(p.RequestId));
                foreach (var entity in list)
                {
                    var last = _ms.GetMachinePark(m => m.Id == entity.Id);
                    last.MarkId = p.MarkId;
                    last.ModelId = p.ModelId;
                    var item = OneMap.mapper.Map<Cm_CustomerMachineparks>(last);
                    _ms.SaveMachinepark(item);
                }

                return new Tuple<bool, string>(true, $"{list.Count} adet makine güncellenmiştir");
            }
            catch (Exception ex)
            {

                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult MachineParkYear()
        {
            var list = _cs.GetListGeneric<Cm_MachineparkYear>();
            return ResultService(data: list);
        }

        private void RequestQuantityUpdate(int RequestId, int Count)
        {
            var item = GetRequestItem(new CustomerRequestFilter() { Id = RequestId });
            item.Quantity = Count;
            SaveRequest(OneMap.mapper.Map<CustomerRequestWrapper>(item));
        }
    }

   

    public enum RequestMachineParkSave
    {
        CopyMp = 1,
        MpGridUpdateInsert=2,
        Delete=3
    }
}