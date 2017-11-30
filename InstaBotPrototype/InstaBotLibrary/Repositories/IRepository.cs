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
        List<BoundModel> getAllBounds(int userId);
        List<FilterModel> getAllFilters(int filterId);
        UserModel getUserInfo(int userId);

        //ADD
        void AddUser(UserModel user);
        void AddBound(BoundModel bound);
        void AddFilter(FilterModel filter);


        //UPDATE
        void UpdateUser(UserModel user);
        void UpdateBound(BoundModel bound);
        void UpdateFilter(FilterModel filter);

        //DELETE
        void DeleteUser(UserModel user);
        void DeleteBound(BoundModel bound);
        void DeleteFilter(FilterModel filter);

    }
}
