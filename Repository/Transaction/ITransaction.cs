using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Transaction
{
    public interface ITransaction : IRepositoryBase, IDisposable
    {
        void Commit();
        void Rollback();
    }
}
