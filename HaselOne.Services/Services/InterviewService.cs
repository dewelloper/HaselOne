using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using HaselOne.Domain.Repository;
using BusinessObjects;
using AutoMapper.QueryableExtensions;
using BusinessObjects.Base;

namespace HaselOne.Services.Services
{
    public class InterviewService :ServiceBase, IInterviewService
    {
        private readonly IGRepository<Cm_CustomerInterviews> _repInterview;
       
        public InterviewService(IUnitOfWork uow) : base(uow)
        {
            _repInterview = _uow.GetRepository<Cm_CustomerInterviews>();
           
        }

        public ServiceResponse<Cm_CustomerInterviews> Save(Cm_CustomerInterviews item)
        {
            var res = ResponseFactory<Cm_CustomerInterviews>();
            var item3 =_repInterview.Save(item);
            res.Entity=item3;
            return res;

        }
        
        public ServiceResponse<Cm_CustomerInterviews> GetList(CustomerInterviewsFilter filter)
        {
            var response = ResponseFactory<Cm_CustomerInterviews>();
            bool state = false;
            var query = _repInterview.Where(m => m.IsDeleted == filter.IsDelete);

            if (filter != null)
            {
                if (filter.Id !=null && filter.Id != 0)
                {
                    state = true;
                    query = query.Where(m => m.Id == filter.Id);
                }

                if (filter.AuthenticatorId != 0)
                {
                    state = true;
                    query = query.Where(m => m.AuthenticatorId == filter.AuthenticatorId);
                }
                if (filter.CustomerId != 0)
                {
                    state = true;
                    query = query.Where(m => m.CustomerId == filter.CustomerId);
                }
            }

           
            if(state==true)
            {
               // var a = query.AsQueryable().ProjectTo<CustomerInterviewsWrapper>(OneMap.GetConfig());
                response.List = query.ToList();
            }
            return response;
        }

     




    }
}
