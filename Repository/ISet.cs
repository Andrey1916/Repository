using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public interface ISet<T> : IQueryable<T> where T : class, IEntity
    {
        T AddOrUpdate(T obj);
        void Remove(T obj);
        void Remove(object id);
    }
}
