using HaselOne.Controler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaselOne.Domain.UnitOfWork;
using System.Web.Mvc;
using HaselOne.Services.Interfaces;
using BusinessObjects;
using BusinessObjects.Base;
using BusinessObjects.Base.StaticText;
using DAL;
using DAL.Helper;
using HaselOne.Domain.Repository;
using HaselOne.Services.Services;
using HaselOne.Util;

namespace HaselOne.Controler
{
    public class InterviewController : HaselBaseController
    {
        IInterviewService _iss;
        IMachineparkService _ms;
        ICustomerService _cs;
        public InterviewController(IUnitOfWork uow, IInterviewService iss, ICustomerService cs) : base(uow)
        {
            _iss = iss;
            _cs = cs;
        }

        [HttpPost]
        public ActionResult GetInterview()
        {
            return ResultService(objectData: GetListInterview()); ;
        }
        
        [HttpPost]
        public ActionResult GetAuthenticator(int filter)
        {
            return ResultService(data: GetListCustomerAuthenticators(filter));
        }
        
        [HttpPost]
        public ActionResult GetInterviewImportant()
        {
            return ResultService(data: GetListInterviewImportant());
        }
        
        [HttpPost]
        public ActionResult GetInterviewUser()
        {
         
            return ResultService(data: GetSalesmanListForAreaAndOperainTypeForLiad());
        }

        [HttpPost]
        public ActionResult GetList(CustomerInterviewsFilter filter)
        {
            /*
              {
                            dataField: "User",
                            caption: "Görüşen",d
                        },
                       
                        {
                            dataField: "",
                            caption: "Görüşme Tipi",
                        },
                      
             */
            var list = _iss.GetList(filter);
            var view = list.List.Select(m => new {
                m.Id,
                m.InterviewDate,
                m.Interviewed,
                AuthenticatorsName=m.Cm_CustomerAuthenticators?.Name,
                InterviewImportant=m.Gn_InterviewImportant?.Title,
                User = m.Gn_User1?.Name,
                Interview= m.Cm_Interview?.Name,
            });
            var item = ResultService(objectData: view);
            return item;
        }

        [HttpPost]
        public ActionResult Get(int filter)
        {
            var list = _iss.GetList(new CustomerInterviewsFilter() { Id = filter });
            
            return ResultService(objectData: OneMap.mapper.Map<CustomerInterviewsWrapper>(list.List[0]));
        
        }




        [HttpPost]

        public ActionResult Save(CustomerInterviewsWrapper vm, bool? isDelete = false)
        {

            if (isDelete != null && (bool)isDelete)//silmede validasyon bakmaz
            {
                vm.IsDeleted = true;
            }
            else
            {
                if (!vm.Validate())
                {
                    return ResultService(false, true, Text.Warning, validationMessages: vm.ValidationResult, resultType: ResultType.Warning);
                }
            }
            
            Save(vm);

            return ResultService(objectData: vm, message: "İşlem başarılı", resultType: ResultType.Success);

        }

        private void Save(CustomerInterviewsWrapper vm)
        {
           var item = OneMap.mapper.Map<Cm_CustomerInterviews>(vm);
            _iss.Save(item);
        }


        #region Private_Method
        public  dynamic  GetListCustomerAuthenticators(int filter)
        {
            var list = _iss.GetListGeneric<Cm_CustomerAuthenticators>(m => m.IsDeleted != true && m.CustomerId == filter).Select(m => new {
                Value = m.Id,
                Text = m.Name.ToString()
            }).ToList();
            return list;
        }
        public dynamic GetSalesmanListForAreaAndOperainTypeForLiad()
        {
            return _cs.GetSalesmanListForAreaAndOperationTypeForLoad(CurrentUser.CurrentUserId)
                .Select(m=>new { Value= m.Id, Text =m.UserName.ToString()}).ToList();
        }
        public dynamic GetListInterviewImportant()
        {
            var item = _iss.GetListGeneric<Gn_InterviewImportant>().Select(m => new {
                Value = m.Id,
                Text = m.Title.ToString()
            }).ToList();
            return item;
        }
        public dynamic GetListInterview()
        {
            var list = _iss.GetListGeneric<Cm_Interview>(m => m.IsDeleted != true).Select(m=>new {
               Value= m.Id,
                Text=m.Name.ToString()
            }).ToList();
            return list;
        }
        #endregion


        /*
             Util.Utility.LoadCombo(ddInterviewUser, _cs.GetSalesmanListForAreaAndOperationTypeForLoad(uid).Select(m => new { Title = m.UserName, Id = m.Id }).ToList(), "Title", "Id");
               ddInterviewUser.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });

          
            
            */
    }
}