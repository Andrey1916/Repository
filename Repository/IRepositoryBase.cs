using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IRepositoryBase
    {
        T GetById<T>(object id) where T : class;
        T AddOrUpdate<T>(T obj) where T : class, IEntity;
        void Remove<T>(T obj) where T : class;
        void Remove<T>(object id) where T : class;

        ISet<T> Set<T>() where T : class, IEntity;
    }
}
