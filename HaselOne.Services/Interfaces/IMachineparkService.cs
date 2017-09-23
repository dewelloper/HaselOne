using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HaselOne.Services.Interfaces
{
    public interface IMachineparkService : IServiceBase
    {
        int GetMachineParkCount(int RequestId);

        Cm_CustomerMachineparks SaveMachinepark(Cm_CustomerMachineparks obj, bool autoCommit = true);

        Cm_MachineparkMark SaveMachineparkMark(Cm_MachineparkMark obj);

        Cm_MachineparkCategory SaveMachineparkCategory(Cm_MachineparkCategory obj);

        Pr_MachineModel SaveMachineModel(Pr_MachineModel obj);

        bool IsMpCategoryAllowed(int categoryId, int customerId);

        Cm_CustomerMachineparks GetMachinePark(Expression<Func<Cm_CustomerMachineparks, bool>> exp, string[] includePaths = null);

        IQueryable<Cm_CustomerMachineparks> GetMachineParks(Expression<Func<Cm_CustomerMachineparks, bool>> exp, string[] includePaths = null);

        IQueryable<Cm_MachineparkCategory> GetMachineParkCategory(MachineparkCategoryFilter filter);

        IQueryable<Cm_MachineparkMark> GetMachineParkMark(MachineparkMarkFilter filter);

        IQueryable<Pr_MachineModel> GetMachineModel(MachineModelFilter filter);
    }
}