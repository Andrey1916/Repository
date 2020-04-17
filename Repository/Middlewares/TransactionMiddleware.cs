using System;
using Repository.Transaction;

namespace Repository.Middlewares
{
    public abstract class TransactionMiddleware : ITransaction
    {
        protected readonly ITransaction transaction;

        public TransactionMiddleware(ITransaction transaction)
        {
            this.transaction = transaction ?? throw new NullReferenceException(nameof(transaction));
        }

        public abstract void Commit();

        public abstract void Rollback();


        public virtual void Dispose()
        { }


        public abstract T GetById<T>(object id) where T : class;

        public abstract void Remove<T>(T obj) where T : class;

        public abstract void Remove<T>(object id) where T : class;

        public abstract T AddOrUpdate<T>(T obj) where T : class, IEntity;

        public abstract ISet<T> Set<T>() where T : class, IEntity;
    }
}