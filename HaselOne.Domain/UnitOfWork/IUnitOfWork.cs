using HaselOne.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace HaselOne.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;

        /// <summary>
        /// Context te otomatik olarak açılan transaction'ı ve eğer varsa onu kapsayan transaction'ı sonlandırır.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Yeni bir transaction başlatır.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Var olan transaction için commit işlemi uygular.
        /// </summary>
        void Commit();

        /// <summary>
        /// Var olan transaction için rollback işlemi uygular.
        /// </summary>
        void Rollback();

        new void Dispose();

        object ExecuteSqlCommand(string storeProcedure, params object[] parameters);

        object ExecuteSqlCommand(string storeProcedure);

        IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parms);
    }
}