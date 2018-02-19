using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace InstaBotLibrary.DbCommunication
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
