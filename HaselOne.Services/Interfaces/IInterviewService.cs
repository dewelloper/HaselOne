using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HaselOne.Services.Interfaces
{
    public interface IInterviewService : IServiceBase
    {
        ServiceResponse<Cm_CustomerInterviews >Save(Cm_CustomerInterviews item);
        ServiceResponse<Cm_CustomerInterviews> GetList(CustomerInterviewsFilter filter);

       
    }
}