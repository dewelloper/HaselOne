using DAL;
using HaselOne.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HaselOne.Domain.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HASELONEEntities _context;
        private bool disposed = false;
        private System.Data.Common.DbTransaction _tran;

        public UnitOfWork(HASELONEEntities context)
        {
            Database.SetInitializer<HASELONEEntities>(null);
            if (context == null)
                throw new ArgumentException("context is null");
            _context = context;
        }

        /// <summary>
        /// Yeni bir transaction başlatır.
        /// </summary>
        public void BeginTransaction()
        {
            _tran = _context.Database.Connection.BeginTransaction();
        }

        /// <summary>
        /// Var olan transaction için commit işlemi uygular.
        /// </summary>
        public void Commit()
        {
            if (_context.Database.CurrentTransaction != null)
                _context.Database.CurrentTransaction.Commit();
        }

        /// <summary>
        /// Var olan transaction için rollback işlemi uygular.
        /// </summary>
        public void Rollback()
        {
            if (_context.Database.CurrentTransaction != null)
                _context.Database.CurrentTransaction.Rollback();
        }

        /// <summary>
        /// Context te otomatik olarak açılan transaction'ı ve eğer varsa onu kapsayan transaction'ı sonlandırır.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            try
            {
                var res = _context.SaveChanges();

                if (_context.Database.CurrentTransaction != null)
                    _context.Database.CurrentTransaction.Commit();

                return res;
            }
            catch (Exception)
            {
                if (_context.Database.CurrentTransaction != null)
                    _context.Database.CurrentTransaction.Rollback();
                throw;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return new GRepository<TEntity>(_context);
        }

        public object ExecuteSqlCommand(string storeProcedure, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(storeProcedure, parameters);
        }

        public object ExecuteSqlCommand(string strQuery)
        {
            return _context.Database.ExecuteSqlCommand(strQuery);
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parms)
        {
            return _context.Database.SqlQuery<TEntity>(sql, parms);
        }
    }

    public class CurrentUser
    {
        public static int CurrentUserId
        {
            get
            {
                //  var a = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var a = HttpContext.Current.User.Identity as ClaimsIdentity;
                    int userId = Convert.ToInt32(a.FindFirst("Id").Value);

                    return userId;
                }

                return 0;
            }
        }
    }
}