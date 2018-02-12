using InstaBotLibrary.Authorization;
using InstaBotLibrary.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotPrototype.Services
{
    public class UserManager : IUserManager
    {
        UserRepository userRepository = new UserRepository();
        AuthorizationModel authModel;

        public bool IsLoggedIn(string login, string password)
        {
            authModel = userRepository.getUserAuthorizationInfo(login);

            if (authModel == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int SessionId (string login)
        {
            authModel = userRepository.getUserAuthorizationInfo(login);

            return authModel.Id;
        }
    }
}
