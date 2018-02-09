using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotPrototype.Services
{
    public interface IUserManager
    {
        bool IsLoggedIn(string login, string password);
    }
}
