using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IGenericRepository<TEntity> : IDisposable
    {
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
        IEnumerable<TEntity> GetMany(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        TEntity GetByID(object id);

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entity">
        /// </param>
        void Insert(TEntity entity);

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="id">
        /// </param>
        void Delete(object id);

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entityToDelete">
        /// </param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate">
        /// </param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        IEnumerable<TEntity> GetMany(Func<TEntity, bool> where);

        /// <summary>
        /// generic method to get many record but query able.
        /// </summary>
        /// <returns>
        /// </returns>
        IQueryable<TEntity> GetManyQueryable();

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where);

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        TEntity Get(Func<TEntity, Boolean> where);

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        void Delete(Func<TEntity, Boolean> where);

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate">
        /// </param>
        /// <param name="include">
        /// </param>
        /// <returns>
        /// </returns>
        IQueryable<TEntity> GetWithInclude(
            System.Linq.Expressions.Expression<Func<TEntity,
                bool>> predicate, params string[] include);

        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey">
        /// </param>
        /// <returns>
        /// </returns>
        bool Exists(object primaryKey);

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record that matches the specified criteria
        /// </returns>
        TEntity GetSingle(Func<TEntity, bool> predicate = null);

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record containing the first record matching the specified criteria
        /// </returns>
        TEntity GetFirst(Func<TEntity, bool> predicate = null);

        /// <summary>
        /// The last record matching the specified criteria
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record containing the last record matching the specified criteria
        /// </returns>
        TEntity GetLast(Func<TEntity, bool> predicate = null);

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<TEntity> FindAll(Func<TEntity, bool> where);

        /// <summary>
        /// Execute Store Procedure for a Model
        /// </summary>
        /// <param name="procedureName">Procedure Name</param>
        /// <param name="parameters">Param Object Array</param>
        /// <returns></returns>
        IEnumerable<TEntity> SqlQuery(string procedureName, params object[] parameters);

        /// <summary>
        /// Execute Store Procedure for a Model
        /// </summary>
        /// <param name="procedureName">Procedure Name</param>
        /// <returns></returns>
        IEnumerable<TEntity> SqlQuery(string procedureName);

        void Reload(TEntity entity);



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
        Task<IEnumerable<TEntity>> GetManyAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        Task<TEntity> GetByIDAsync(object id);

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        Task<IEnumerable<TEntity>> GetManyAsync(Func<TEntity, bool> where);

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where">
        /// </param>
        /// <returns>
        /// </returns>
        Task<TEntity> GetAsync(Func<TEntity, Boolean> where);

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns>
        /// </returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate">
        /// </param>
        /// <param name="include">
        /// </param>
        /// <returns>
        /// </returns>
        Task<IEnumerable<TEntity>> GetWithIncludeAsync(
            System.Linq.Expressions.Expression<Func<TEntity,
                bool>> predicate, params string[] include);

        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey">
        /// </param>
        /// <returns>
        /// </returns>
        Task<bool> ExistsAsync(object primaryKey);

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record that matches the specified criteria
        /// </returns>
        Task<TEntity> GetSingleAsync(Func<TEntity, bool> predicate = null);

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record containing the first record matching the specified criteria
        /// </returns>
        Task<TEntity> GetFirstAsync(Func<TEntity, bool> predicate = null);

        /// <summary>
        /// The last record matching the specified criteria
        /// </summary>
        /// <param name="predicate">
        /// Criteria to match on
        /// </param>
        /// <returns>
        /// A single record containing the last record matching the specified criteria
        /// </returns>
        Task<TEntity> GetLastAsync(Func<TEntity, bool> predicate = null);

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns>
        /// </returns>
        Task<IEnumerable<TEntity>> FindAllAsync(Func<TEntity, bool> where);

        /// <summary>
        /// Execute Store Procedure for a Model
        /// </summary>
        /// <param name="procedureName">Procedure Name</param>
        /// <param name="parameters">Param Object Array</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> SqlQueryAsync(string procedureName, params object[] parameters);

        /// <summary>
        /// Execute Store Procedure for a Model
        /// </summary>
        /// <param name="procedureName">Procedure Name</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> SqlQueryAsync(string procedureName);

        Task ReloadAsync(TEntity entity);
    }
}