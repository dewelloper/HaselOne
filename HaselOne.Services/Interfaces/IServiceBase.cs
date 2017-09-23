using HaselOne.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL;

namespace HaselOne.Services.Interfaces
{
    public interface IServiceBase
    {
        List<TEntity> GetListGeneric<TEntity>(Expression<Func<TEntity, bool>> filter = null, string[] includePaths = null, int? page = null, int? pageSize = null, params SortExpression<TEntity>[] sortExpressions) where TEntity : class, IEntity;

        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, string[] includePaths = null) where TEntity : class,IEntity;

        TEntity GetInstance<TEntity>(Expression<Func<TEntity, bool>> filter = null, string[] includePaths = null) where TEntity : class,IEntity;
    }
}