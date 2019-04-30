
using System.CodeDom;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Organo.Solutions.X4Ever.V1.DAL.Helper;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private static string TAG = typeof(GenericRepository<TEntity>).FullName;
        private X4EverContext _context;
        private DbSet<TEntity> _dbSet => _context.Set<TEntity>();
        private readonly IHelper _helper;

        public GenericRepository(X4EverContext context)
        {
            _context = context;
            _helper = new Helper();
        }

        public GenericRepository()
        {
            _context = new X4EverContext();
            _helper = new Helper();
        }

        /// <summary>
        /// generic Get method for Entities
        /// </summary>
        /// <param name="filter">
        /// </param>
        /// <param name="orderBy">
        /// </param>
        /// <param name="includeProperties">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<TEntity> GetMany(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;
            try
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetMany(filter,orderBy,includeProperties");
            }

            return query.ToList();
        }

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        public TEntity GetByID(object id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetByID?=id" + id);
            }

            return null;
        }

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entity">
        /// </param>
        public void Insert(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Insert(entity)");
            }
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="id">
        /// </param>
        public void Delete(object id)
        {
            try
            {
                TEntity entityToDelete = _dbSet.Find(id);
                Delete(entityToDelete);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Delete?id=" + id);
            }
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entityToDelete">
        /// </param>
        public void Delete(TEntity entityToDelete)
        {
            try
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }

                _dbSet.Remove(entityToDelete);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Delete(entityToDelete)");
            }
        }

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate">
        /// </param>
        public void Update(TEntity entityToUpdate)
        {
            try
            {
                _dbSet.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Update(entityToUpdate)");
            }
        }

        //Additional Functions

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            IEnumerable<TEntity> result = null;
            try
            {
                result = _dbSet.Where(where).ToList();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetMany(where)");
            }

            return result;
        }

        /// <summary>
        /// generic method to get many record but query able.
        /// </summary>
        /// <returns>
        /// </returns>
        public IQueryable<TEntity> GetManyQueryable()
        {
            IQueryable<TEntity> result = null;
            try
            {
                result = _dbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetManyQueryable()");
            }

            return result;
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        public IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            try
            {
                return _dbSet.Where(where).AsQueryable();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetManyQueryable(where)");
            }

            return _dbSet.Where(where).AsQueryable();
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        public TEntity Get(Func<TEntity, Boolean> where)
        {
            try
            {
                return _dbSet.Where(where).FirstOrDefault<TEntity>();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Get(where)");
            }

            return null;
        }

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        public void Delete(Func<TEntity, Boolean> where)
        {
            try
            {
                IQueryable<TEntity> objects = _dbSet.Where<TEntity>(where).AsQueryable();
                foreach (TEntity obj in objects)
                    _dbSet.Remove(obj);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Delete(where)");
            }
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetAll()");
            }

            return new List<TEntity>();
        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate">
        /// </param>
        /// <param name="include">
        /// </param>
        /// <returns>
        /// </returns>
        public IQueryable<TEntity> GetWithInclude(
            System.Linq.Expressions.Expression<Func<TEntity,
                bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this._dbSet;
            try
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
                return query.Where(predicate);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetWithInclude(predicate,include)");
            }

            return query.Where(predicate);
        }

        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Exists(object primaryKey)
        {
            try
            {
                return _dbSet.Find(primaryKey) != null;
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Exists(primaryKey)");
            }

            return false;
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record that matches the specified criteria
        /// </returns>
        public TEntity GetSingle(Func<TEntity, bool> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return _dbSet.Single<TEntity>();
                return _dbSet.Single<TEntity>(predicate);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetSingle(predicate)");
            }

            return null;
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record containing the first record matching the specified criteria
        /// </returns>
        public TEntity GetFirst(Func<TEntity, bool> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return _dbSet.First<TEntity>();
                return _dbSet.FirstOrDefault<TEntity>(predicate);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetFirst(predicate)");
            }

            return null;
        }

        /// <summary>
        /// The last record matching the specified criteria
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record containing the last record matching the specified criteria
        /// </returns>
        public TEntity GetLast(Func<TEntity, bool> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return _dbSet.First<TEntity>();
                return _dbSet.LastOrDefault<TEntity>(predicate);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetLast(predicate)");
            }

            return null;
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<TEntity> FindAll(Func<TEntity, bool> where)
        {
            try
            {
                return _dbSet.Where(where).AsQueryable();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "FindAll(where)");
            }

            return new List<TEntity>();
        }

        /// <summary>
        /// Execute Store Procedure for a Model
        /// </summary>
        /// <param name="procedureName">Procedure Name</param>
        /// <param name="parameters">Param Object Array</param>
        /// <returns></returns>
        public IEnumerable<TEntity> SqlQuery(string procedureName, params object[] parameters)
        {
            try
            {
                return this._context.Database.SqlQuery<TEntity>(procedureName, parameters);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "SqlQuery(procedureName, parameters)");
            }

            return new List<TEntity>();
        }

        public IEnumerable<TEntity> SqlQuery(string procedureName)
        {
            try
            {
                return this._context.Database.SqlQuery<TEntity>(procedureName);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "SqlQuery(procedureName)");
            }

            return new List<TEntity>();
        }

        public void Reload(TEntity entity)
        {
            try
            {
                _context.Entry<TEntity>(entity).Reload();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Reload(entity)");
            }
        }

        // ASYNC METHODS

        public async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;
            try
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }
                else
                {
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetManyAsync(filter,orderBy,includeProperties");
            }

            return query.ToList();
        }

        public async Task<TEntity> GetByIDAsync(object id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetByID?=id" + id);
            }

            return null;
        }

        public async Task<IEnumerable<TEntity>> GetManyAsync(Func<TEntity, bool> @where)
        {
            try
            {
                return (await _dbSet.ToListAsync()).Where(where);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetMany(where)");
            }

            return new List<TEntity>();
        }

        public async Task<TEntity> GetAsync(Func<TEntity, bool> @where)
        {
            try
            {
                return await Task.Factory.StartNew(() => 
                { 
                    return _dbSet.Where(where).FirstOrDefault<TEntity>();
                });
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Get(where)");
            }

            return null;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetAll()");
            }

            return new List<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(Expression<Func<TEntity, bool>> predicate,
            params string[] include)
        {
            IQueryable<TEntity> query = this._dbSet;
            try
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
                return await query.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetWithInclude(predicate,include)");
            }

            return new List<TEntity>();
        }

        public async Task<bool> ExistsAsync(object primaryKey)
        {
            try
            {
                return await _dbSet.FindAsync(primaryKey) != null;
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Exists(primaryKey)");
            }

            return false;
        }

        public async Task<TEntity> GetSingleAsync(Func<TEntity, bool> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return await _dbSet.SingleAsync<TEntity>();
                return _dbSet.Single<TEntity>(predicate);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetSingle(predicate)");
            }

            return null;
        }

        public async Task<TEntity> GetFirstAsync(Func<TEntity, bool> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return await _dbSet.FirstOrDefaultAsync<TEntity>();
                return _dbSet.FirstOrDefault<TEntity>(predicate);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetFirst(predicate)");
            }

            return null;
        }

        public async Task<TEntity> GetLastAsync(Func<TEntity, bool> predicate = null)
        {
            try
            {
                return await Task.Factory.StartNew(()=>{ 
                    if (predicate == null)
                        return _dbSet.LastOrDefault<TEntity>();
                    return _dbSet.LastOrDefault<TEntity>(predicate);
                });
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "GetLast(predicate)");
            }

            return null;
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Func<TEntity, bool> @where)
        {
            try
            {
                return (await _dbSet.ToListAsync()).Where(where);
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "FindAll(where)");
            }

            return new List<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> SqlQueryAsync(string procedureName, params object[] parameters)
        {
            try
            {
                return await this._context.Database.SqlQuery<TEntity>(procedureName, parameters).ToListAsync();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "SqlQuery(procedureName, parameters)");
            }

            return new List<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> SqlQueryAsync(string procedureName)
        {
            try
            {
                return await this._context.Database.SqlQuery<TEntity>(procedureName).ToListAsync();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "SqlQuery(procedureName)");
            }

            return new List<TEntity>();
        }

        public async Task ReloadAsync(TEntity entity)
        {
            try
            {
                await _context.Entry<TEntity>(entity).ReloadAsync();
            }
            catch (Exception ex)
            {
                SaveLog(ex,  "Reload(entity)");
            }
        }

        private void SaveLog(Exception ex, string methodDetail)
        {
            try
            {
                _helper.SaveLog(ex, TAG, methodDetail);
                var exceptionString = ex?.ToString() ?? "";
                if (exceptionString.Contains("password") && exceptionString.Contains("expired"))
                {
                    var message = "";
                    new Message().SendMail(ref message,
                        "gurpreet.deol@organogold.com",
                        "taha.ismail@organogold.com",
                        "iman.bayazi@organogold.com",
                        ex.ToString(),
                        GetExceptionDetail(ex),
                        true);
                }
            }
            catch
            {
                //
            }
        }

        private static string GetExceptionDetail(Exception exception)
        {
            var stringBuilder = new StringBuilder();
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);
                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}