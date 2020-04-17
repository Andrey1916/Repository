using System;

namespace Repository.Middlewares
{
    public abstract class MiddlewareBase : IRepositoryBase
    {
        protected readonly IRepositoryBase repository;

        public MiddlewareBase(IRepositoryBase repository)
        {
            this.repository = repository ?? throw new NullReferenceException(nameof(repository));
        }

        public abstract T GetById<T>(object id) where T : class;

        public abstract T AddOrUpdate<T>(T obj) where T : class, IEntity;

        public abstract void Remove<T>(object id) where T : class;

        public abstract void Remove<T>(T obj) where T : class;

        public abstract ISet<T> Set<T>() where T : class, IEntity;
    }
}