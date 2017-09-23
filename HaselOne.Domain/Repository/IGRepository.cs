using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace HaselOne.Domain.Repository
{
    public interface IGRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity Single(int? id);

        TEntity Single(long? id);

        TEntity Single(string where = null, params object[] parms);

        IEnumerable<TEntity> All();

        IEnumerable<TEntity> All(string ids);

        IEnumerable<TEntity> All(string where = null, string orderBy = null, int top = 0, params object[] parms);

        IEnumerable<TEntity> Paged(out int totalRows, string where = null, string orderBy = null, int page = 0, int pageSize = 20, params object[] parms);

        /// <summary>
        /// Girdiyi kaydeder. Kayıt esnasında kendi içinde transaction açar ve varsayılan olarak her kayıt işleminde transaction kapatılır.
        /// Transaction kapsamını genişletmek için, autoCommit parametresini false değerine çekmeniz gerekir.
        /// </summary>
        /// <param name="t">Girdi</param>
        /// <param name="autoCommit">Değer true ise method içinde transaction sonlanır. False ise transaction'ı manuel tamamlamanız gerekir.</param>
        /// <returns>Kaydedilen değer</returns>
        TEntity Save(TEntity t, bool autoCommit = true);

        Int32 Insert(TEntity t);

        TEntity InsertOrUpdate(TEntity t);

        void Update(TEntity t);

        void Delete(TEntity t);

        object Scalar(string operation, string column, string where = null, params object[] parms);

        int Count(string where = null, params object[] parms);

        object Max(string column = null, string where = null, params object[] parms);

        object Min(string column = null, string where = null, params object[] parms);

        object Sum(string column, string where = null, params object[] parms);

        IEnumerable<TEntity> Query(string sql, params object[] parms);

        int Execute(string sql, params object[] parms);

        DbSet<TEntity> GetContext();

        string GetConnectionString();

        int GetCountByAdoNet(string query);

        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> Filter = null);

        IQueryable<TEntity> WhereQuery(Expression<Func<TEntity, bool>> Filter = null);

        IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter = null, string[] includePaths = null, int? page = null, int? pageSize = null, params SortExpression<TEntity>[] sortExpressions);

        IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes);

        Database GetDatabase();
    }
}