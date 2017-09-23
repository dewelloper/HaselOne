using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq.Expressions;
using HaselOne.Domain.UnitOfWork;

namespace HaselOne.Domain.Repository
{
    public class GRepository<TEntity> : IGRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly HASELONEEntities _context;
        private readonly DbSet<TEntity> _dbSet;

        public GRepository(HASELONEEntities context)
        {
            _context = context;
            //db log  _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            _dbSet = _context.Set<TEntity>();
        }

        public GRepository() : this(new HASELONEEntities())
        {
        }

        public TEntity Single(int? id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Single(long? id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Single(string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> All()
        {
            return _dbSet;
        }

        public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
        {
            return _dbSet.IncludeMultiple<TEntity>(includes);
        }

        public IEnumerable<TEntity> All(string ids)
        {
            _dbSet.Where(m => m.Id == Convert.ToInt32(ids));
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> All(string where = null, string orderBy = null, int top = 0, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Paged(out int totalRows, string where = null, string orderBy = null, int page = 0, int pageSize = 20, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public TEntity Save(TEntity t, bool autoCommit = true)
        {
            var tran = _context.Database.CurrentTransaction != null ? _context.Database.CurrentTransaction : _context.Database.BeginTransaction();
            try
            {
                if (t.Id > 0)
                    TCreateLog(t);
                var a = CurrentUser.CurrentUserId;
                var idProperty = t.Id;

                if (t is IBusinessEntity)
                {
                    if (idProperty == 0)
                    {
                        (t as IBusinessEntity).CreateUserId = a;
                        (t as IBusinessEntity).CreateDate = DateTime.Now;
                    }
                    else
                    {
                        (t as IBusinessEntity).UpdateUserId = a;
                        (t as IBusinessEntity).UpdateDate = DateTime.Now;
                       
                    }
                }

                _dbSet.AddOrUpdate(t);
                if (autoCommit)
                {
                    _context.SaveChanges();
                    tran.Commit();
                }
            }
            catch (Exception e)
            {
                if (autoCommit)
                    tran.Rollback();
                throw;
            }

            return t;
        }

        //public void Save()
        //{
        //    try
        //    {
        //        _context.SaveChanges();
        //        _tran.Commit();
        //    }
        //    catch (Exception e)
        //    {
        //        _tran.Rollback();
        //        throw e;
        //    }

        //}
        public TEntity InsertOrUpdate(TEntity t)
        {
            return Save(t);
        }

        public int Insert(TEntity t)
        {
            _dbSet.Add(t);
            _context.SaveChanges();
            var idProperty = t.GetType().GetProperty("Id").GetValue(t, null);//t.GetType().GetProperty("CreateUserId").SetValue(t, a);
            return Convert.ToInt32(idProperty);
        }

        public void Update(TEntity t)
        {
            TCreateLog(t);
            _dbSet.Attach(t);
            _context.Entry<TEntity>(t).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(TEntity t)
        {
            if (_context.Entry<TEntity>(t).State == EntityState.Detached)
                _dbSet.Attach(t);
            _dbSet.Remove(t);
            _context.SaveChanges();
        }

        //public TEntity Clone<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        //{
        //    var keyName = GetKeyName<TEntity>();
        //    var keyValue = context.Entry(entity).Property(keyName).CurrentValue;
        //    var keyType = typeof(TEntity).GetProperty(keyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).PropertyType;

        //    var dbSet = context.Set<TEntity>();
        //    var newEntity = dbSet.Where(keyName + " = @0", keyValue).AsNoTracking().Single();

        //    context.Entry(newEntity).Property(keyName).CurrentValue = keyType.GetDefault();

        //    context.Add(newEntity);

        //    return newEntity;
        //}
        //private string GetKeyName<TEntity>() where TEntity : class
        //{
        //    return ((DataServiceKeyAttribute)typeof(TEntity)
        //       .GetCustomAttributes(typeof(DataServiceKeyAttribute), true).First())
        //       .KeyNames.Single();
        //}

        public object Scalar(string operation, string column, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public int Count(string where = null, params object[] parms)
        {
            return _dbSet.Count();
        }

        public object Max(string column = null, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public object Min(string column = null, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public object Sum(string column, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Query(string sql, params object[] parms)
        {
            return _dbSet.SqlQuery(sql, parms);
        }

        public int Execute(string sql, params object[] parms)
        {
            return _context.Database.ExecuteSqlCommand(sql, parms);
        }

        public DbSet<TEntity> GetContext()
        {
            return _dbSet;
        }

        public string GetConnectionString()
        {
            return _context.Database.Connection.ConnectionString;
        }

        public Database GetDatabase()
        {
            return _context.Database;
        }

        public int GetCountByAdoNet(string query)
        {
            using (SqlConnection connection = new SqlConnection(_context.Database.Connection.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query.Replace("*", " COUNT(*) "), connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return 0;
        }

        public virtual IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> Filter = null)
        {
            if (Filter != null)
            {
                return _dbSet.Where(Filter);
            }
            return _dbSet;
        }

        public virtual IQueryable<TEntity> WhereQuery(Expression<Func<TEntity, bool>> Filter = null)
        {
            if (Filter != null)
            {
                return _dbSet.Where(Filter);
            }
            return _dbSet;
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter = null, string[] includePaths = null, int? page = null, int? pageSize = null, params SortExpression<TEntity>[] sortExpressions)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includePaths != null)
            {
                for (var i = 0; i < includePaths.Count(); i++)
                {
                    query = query.Include(includePaths[i]);
                }
            }

            if (sortExpressions != null)
            {
                IOrderedQueryable<TEntity> orderedQuery = null;
                for (var i = 0; i < sortExpressions.Count(); i++)
                {
                    if (i == 0)
                    {
                        if (sortExpressions[i].SortDirection == ListSortDirection.Ascending)
                        {
                            orderedQuery = query.OrderBy(sortExpressions[i].SortBy);
                        }
                        else
                        {
                            orderedQuery = query.OrderByDescending(sortExpressions[i].SortBy);
                        }
                    }
                    else
                    {
                        if (sortExpressions[i].SortDirection == ListSortDirection.Ascending)
                        {
                            orderedQuery = orderedQuery.ThenBy(sortExpressions[i].SortBy);
                        }
                        else
                        {
                            orderedQuery = orderedQuery.ThenByDescending(sortExpressions[i].SortBy);
                        }
                    }
                }

                if (page != null)
                {
                    query = orderedQuery.Skip(((int)page - 1) * (int)pageSize);
                }
            }

            if (pageSize != null)
            {
                query = query.Take((int)pageSize);
            }

            return query.ToList();
        }

        public void TCreateLog(TEntity t)
        {
            var name = t.GetType().Name;
            if (t.GetType().BaseType != typeof(System.Object))
            {
                name = t.GetType().BaseType.Name;
            }
            _context.Database.ExecuteSqlCommand($"exec createlog {t.Id}, {typeof(TEntity).Name}");
        }

        //public void CreateLog(TEntity t)
        //{
        //    if(t.Id>0)
        //     _context.Database.SqlQuery();

        //    //var idProperty = (int)t.GetType().GetProperty("Id").GetValue(t, null);
        //    //if (idProperty > 0)
        //    //{
        //    //    _context.CreateLog(idProperty, typeof(TEntity).Name);
        //    //}
        //}
    }

    public class SortExpression<TEntity> where TEntity : class
    {
        public SortExpression(Expression<Func<TEntity, int>> sortBy, ListSortDirection sortDirection)
        {
            SortBy = sortBy;
            SortDirection = sortDirection;
        }

        public Expression<Func<TEntity, int>> SortBy { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }

    public static class Extension
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }
    }
}