using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using Organo.Solutions.X4Ever.V1.DAL.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IDefaultServices<T>
    {
        IEnumerable<T> Get();
        Task<IEnumerable<T>> GetAsync();

        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        T Get(string ID);
        Task<T> GetAsync(string ID);

        bool Insert(ref ValidationErrors validationErrors, T entity);

        bool Delete(ref ValidationErrors validationErrors, T entity);

        bool Update(ref ValidationErrors validationErrors, T entity);
    }
}