using System;
using System.Collections.Generic;
using System.Text;
using InstaBotLibrary.Models;

namespace InstaBotLibrary.Repositories
{
    interface IRepository
    {
        //READ



        AuthorizationModel getUserAuthorizationInfo(string login);
        List<BoundModel> getUserBounds(int userId);
        List<FilterModel> getBoundFilters(int boundId);
        UserModel getUserInfo(int userId);

        //ADD
        void AddUser(UserModel user);
        void AddBound(BoundModel bound);
        void AddFilter(FilterModel filter);


        //UPDATE
        void UpdateUser(UserModel user);
        void UpdateBound(BoundModel bound);

        //DELETE
        void DeleteUser(int userId);
        void DeleteBound(int boundId);
        void DeleteFilter(FilterModel filter);

    }
}
