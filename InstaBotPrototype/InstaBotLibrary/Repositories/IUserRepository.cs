using InstaBotLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Repositories
{
    interface IUserRepository
    {
        AuthorizationModel getUserAuthorizationInfo(string login);
        UserModel getUserInfo(int userId);
        void AddUser(UserModel user);
        void UpdateUser(UserModel user);
        void DeleteUser(int userId);
    }
}
