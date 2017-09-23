using BusinessObjects;
using DAL;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace HaselOne.Services.Services
{
    public class MachineparkService : ServiceBase, IMachineparkService
    {
        private readonly IGRepository<Cm_CustomerMachineparks> _machineParks;
        private readonly IGRepository<Cm_MachineparkMark> _marks;
        private readonly IGRepository<Pr_MachineModel> _models;
        private readonly IGRepository<Cm_MachineparkCategory> _categories;
        private readonly IGRepository<Cm_CustomerSalesmans> _salesmans;
        private readonly IGRepository<Gn_CategoryDetails> _categoryDetails;

        public MachineparkService(IUnitOfWork uow) : base(uow)
        {
            _machineParks = _uow.GetRepository<Cm_CustomerMachineparks>();
            _marks = _uow.GetRepository<Cm_MachineparkMark>();
            _models = _uow.GetRepository<Pr_MachineModel>();
            _categories = _uow.GetRepository<Cm_MachineparkCategory>();
            _salesmans = _uow.GetRepository<Cm_CustomerSalesmans>();
            _categoryDetails = _uow.GetRepository<Gn_CategoryDetails>();
        }

        public Cm_CustomerMachineparks SaveMachinepark(Cm_CustomerMachineparks obj, bool autoCommit = true)
        {
            return _machineParks.Save(obj, autoCommit);
        }

        public Cm_MachineparkMark SaveMachineparkMark(Cm_MachineparkMark obj)
        {
            return _marks.Save(obj);
        }

        public Pr_MachineModel SaveMachineModel(Pr_MachineModel obj)
        {
            return _models.Save(obj);
        }

        public Cm_MachineparkCategory SaveMachineparkCategory(Cm_MachineparkCategory obj)
        {
            return _categories.Save(obj);
        }

        public Cm_CustomerMachineparks GetMachinePark(Expression<Func<Cm_CustomerMachineparks, bool>> exp, string[] includePaths = null)
        {
            var query = _machineParks.GetContext().Where(exp);
            if (includePaths != null)
                for (int i = 0; i < includePaths.Length; i++)
                {
                    query = query.Include(includePaths[i]);
                }
            return query.FirstOrDefault();
        }

        public IQueryable<Cm_CustomerMachineparks> GetMachineParks(Expression<Func<Cm_CustomerMachineparks, bool>> exp, string[] includePaths = null)
        {
            var query = _machineParks.GetContext().Where(exp);
            if (includePaths != null)
                for (int i = 0; i < includePaths.Length; i++)
                {
                    query = query.Include(includePaths[i]);
                }

            return query;
        }

        public IQueryable<Cm_MachineparkCategory> GetMachineParkCategory(MachineparkCategoryFilter filter)
        {
            var res = new List<Cm_MachineparkCategory>();
            var allCategories = _categories.GetContext().ToList();
            var crgIds = _salesmans.GetContext().Where(_ => (!filter.CustomerId.HasValue || _.CustomerId == filter.CustomerId.Value)
                                                    && (!filter.SalesmanId.HasValue || _.SalesmanId == filter.SalesmanId.Value)
                                                    && _.IsDeleted == false).Select(_ => _.CRGID).Distinct();

            var mapCatIds = _categoryDetails.GetContext().Where(_ => _.IsActive == true
                                                    && _.IsDeleted == false
                                                    && crgIds.Contains(_.CRGId)).Select(_ => _.CategoryId).Distinct();

            var allowedCategories = allCategories.Where(_ => mapCatIds.Contains(_.Id)).ToList();

            res.AddRange(allowedCategories);
            foreach (var item in allowedCategories)
            {
                var parent = allCategories.Where(_ => _.Id == item.ParentId).FirstOrDefault();

                if (parent != null && res.Where(_ => _.Id == parent.Id).FirstOrDefault() == null)
                    res.Add(parent);
                SetMachineparkCategoryParent(allCategories, res, item);
            }

            return filter.OnlyRequestVisibleTrue ? res.Where(m => m.IsRequestVisible).AsQueryable() : res.AsQueryable();
        }

        private void SetMachineparkCategoryParent(List<Cm_MachineparkCategory> source, List<Cm_MachineparkCategory> result, Cm_MachineparkCategory main)
        {
            var parent = source.Where(_ => _.Id == main.ParentId).FirstOrDefault();
            if (parent != null)
            {
                if (result.Where(_ => _.Id == parent.Id).FirstOrDefault() == null)
                    result.Add(parent);

                SetMachineparkCategoryParent(source, result, parent);
            }
        }

        public IQueryable<Cm_MachineparkMark> GetMachineParkMark(MachineparkMarkFilter filter)
        {
            int?[] markIds = null;

            if (filter.CategoryId.HasValue)
                markIds = _models.GetContext().Where(_ => _.CategoryId == filter.CategoryId && _.IsActive == true)
                         .Select(_ => _.MarkId).Distinct().ToArray();

            var res = _marks.Where(_ => _.IsActive == filter.IsActive && _.IsDeleted == filter.IsDeleted);

            if (filter.IsOwnerMachine.HasValue)
                res = res.Where(_ => _.IsOwnerMachine == filter.IsOwnerMachine);

            if (filter.CategoryId.HasValue)
                res = res.Where(_ => markIds.Contains(_.Id));

            return res.AsQueryable();
        }

        public IQueryable<Pr_MachineModel> GetMachineModel(MachineModelFilter filter)
        {
            var res = _models.GetContext().Where(_ => _.IsActive
                                                     && (!filter.CategoryId.HasValue || _.CategoryId == filter.CategoryId)
                                                     && (!filter.RequestVisible.HasValue || _.RequestVisible == filter.RequestVisible)
                                                     && (!filter.MarkId.HasValue || _.MarkId == filter.MarkId)
                                                     && (string.IsNullOrEmpty(filter.Name) || _.Name.ToLower().Contains(filter.Name.ToLower())));

            return res;
        }

        public bool IsMpCategoryAllowed(int categoryId, int customerId)
        {
            var res = _uow.SqlQuery<int>(@"DECLARE @MachineparkCategoryId INT = {0}
                                           DECLARE @CustomerId INT = {1}

                                           SELECT CRGId AS CId
                                           FROM Gn_CategoryDetails
                                           WHERE CategoryId = @MachineparkCategoryId

                                           INTERSECT

                                           SELECT GroupId AS CId
                                           FROM Gn_DepartmentRoles
                                           WHERE Id IN (
		                                           SELECT DepartmentRuleId
		                                           FROM Gn_UserRoles
		                                           WHERE UserId IN (
				                                           SELECT SalesmanId
				                                           FROM Cm_CustomerSalesmans
				                                           WHERE CustomerId = @CustomerId
					                                           AND IsDeleted = 0
				                                           )
		                                           )", categoryId, customerId).ToList();
            return res.Count > 0;
        }

        public int GetMachineParkCount(int RequestId)
        {
            var res = _uow.SqlQuery<int>($@"SELECT count(1) FROM Cm_CustomerMachineparks where RequestId = {RequestId}
            and IsDeleted = 0 and IsActive = 1 and (Serialno is  null or serialno ='')");

            if (res.Count() > 0)
                return res.ToList()[0];
            return 0;
        }
    }
}