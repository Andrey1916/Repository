using System;
using System.Collections.Generic;
using System.Text;
using Repository.Transaction;

namespace Repository.Middlewares
{
    public abstract class TransactionalMiddleware : MiddlewareBase, IRepository
    {
        protected readonly new IRepository repository;

        public TransactionalMiddleware(IRepository repository) : base(repository)
        {
            repository = (IRepository)base.repository;
        }

        public abstract ITransaction BeginTransaction();
    }
}