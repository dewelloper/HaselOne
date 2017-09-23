using BusinessObjects;
using DAL;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using HaselOne.Services.Interfaces;

namespace HaselOne.Services.Services
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _uow;

        public ServiceBase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public  List<TEntity> GetListGeneric<TEntity>(Expression<Func<TEntity, bool>> filter = null, string[] includePaths = null, int? page = null, int? pageSize = null, params SortExpression<TEntity>[] sortExpressions) where TEntity : class,IEntity
        {
            return _uow.GetRepository<TEntity>().GetPaged(filter, includePaths, page, pageSize, sortExpressions).ToList();
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> filter, string[] includePaths = null) where TEntity : class,IEntity
        {
            return _uow.GetRepository<TEntity>().GetPaged(filter, includePaths).FirstOrDefault();
        }

        public TEntity GetInstance<TEntity>(Expression<Func<TEntity, bool>> filter, string[] includePaths = null) where TEntity : class,IEntity
        {
            return new UnitOfWork(new HASELONEEntities()).GetRepository<TEntity>().GetPaged(filter, includePaths).FirstOrDefault();
        }
    
        public ServiceResponse<T> ResponseFactory<T>()
        {
            return new ServiceResponse<T>();
        }

    }

  
}