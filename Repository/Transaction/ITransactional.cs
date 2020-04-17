using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Transaction
{
    public interface ITransactional
    {
        ITransaction BeginTransaction();
    }
}
