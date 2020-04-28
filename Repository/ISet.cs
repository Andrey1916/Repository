using Repository.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    public interface ISet<T> : IQueryable<T> where T : class, IEntity
    {
        T AddOrUpdate(T obj);
        void Remove(T obj);
        void Remove(object id);

        IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
    }
}