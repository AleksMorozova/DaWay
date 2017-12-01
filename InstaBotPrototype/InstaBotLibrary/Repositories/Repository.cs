using System;
using System.Collections.Generic;
using System.Linq;
using InstaBotLibrary.Models;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace InstaBotLibrary.Repositories
{
    class Repository : IRepository
    {
        string connectionString = "";


        public void AddBound(BoundModel bound)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Bounds (UserId, TelegramAccount, InstagramAccount, InstagramPassword) VALUES(@UserId, @TelegramAccount, @InstagramAccount, @InstagramPassword); SELECT CAST(SCOPE_IDENTITY() as int)";
                int boundId = db.Query<int>(sqlQuery, bound).FirstOrDefault();
                bound.Id = boundId;
            }
        }

        public void AddFilter(FilterModel filter)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Filters (BoundId, Filter) VALUES(@BoundId, @Filter)";
                db.Execute(sqlQuery, filter);
            }
        }

        public void AddUser(UserModel user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Users (Login, Password, Name, Surname, Birth) VALUES(@Login, @Password, @Name, @Surname, @Birth); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId;
            }
        }

        public void DeleteBound(int boundId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @boundId";
                db.Execute(sqlQuery,new { boundId });
            }
        }

        public void DeleteFilter(FilterModel filter)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE BoundId = @BoundId AND Filter = @Filter";
                db.Execute(sqlQuery, filter);
            }
        }

        public void DeleteUser(int userId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @userId";
                db.Execute(sqlQuery, new { userId });
            }
        }

        public List<FilterModel> getBoundFilters(int boundId)
        {
            List<FilterModel> filters = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                filters = db.Query<FilterModel>("SELECT * FROM Filters WHERE BoundId = @boundId", new { boundId }).ToList();
            }
            return filters;
        }

        public AuthorizationModel getUserAuthorizationInfo(string login)
        {
            AuthorizationModel authorization = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                authorization = db.Query<AuthorizationModel>("SELECT * FROM Users WHERE Login = @login", new { login }).FirstOrDefault();
            }
            return authorization;
        }

        public List<BoundModel> getUserBounds(int userId)
        {
            List<BoundModel> bounds = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                bounds = db.Query<BoundModel>("SELECT * FROM Bounds WHERE UserId = @userId", new { userId }).ToList();
            }
            return bounds;
        }

        public UserModel getUserInfo(int userId)
        {
            UserModel user = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                user = db.Query<UserModel>("SELECT * FROM Users WHERE Id = @id", new { userId }).FirstOrDefault();
            }
            return user;
        }

        public void UpdateBound(BoundModel bound)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Bounds SET TelegramAccount = @TelegramAccount, InstagramAccount = @InstagramAccount, InstagramPassword = @InstagramPassword WHERE Id = @Id";
                db.Execute(sqlQuery, bound);
            }
        }

        public void UpdateUser(UserModel user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Users SET Login = @Login, Password = @Password, Name = @Name, Surname = @Surname, Birth = @Birth WHERE Id = @Id";
                db.Execute(sqlQuery, user);
            }
        }
    }
}
