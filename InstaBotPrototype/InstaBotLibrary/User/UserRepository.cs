using Dapper;
using InstaBotLibrary.User;
using InstaBotLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace InstaBotLibrary.User
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository() { }
        public UserRepository(string str) : base(str) { }


        public void AddUser(UserModel user)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "INSERT INTO Users (Login, Password, Name, Surname, Birth) VALUES(@Login, @Password, @Name, @Surname, @Birth); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId;
            }
        }

        public void DeleteUser(int userId)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @userId";
                db.Execute(sqlQuery, new { userId });
            }
        }
        public AuthorizationModel getUserAuthorizationInfo(string login)
        {
            AuthorizationModel authorization = null;
            using (IDbConnection db = GetConnection())
            {
                authorization = db.Query<AuthorizationModel>("SELECT * FROM Users WHERE Login = @login", new { login }).FirstOrDefault();
            }
            return authorization;
        }

        public UserModel getUserInfo(int userId)
        {
            UserModel user = null;
            using (IDbConnection db = GetConnection())
            {
                user = db.Query<UserModel>("SELECT * FROM Users WHERE Id = @userId", new { userId }).FirstOrDefault();
            }
            return user;
        }

        public void UpdateUser(UserModel user)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "UPDATE Users SET Login = @Login, Password = @Password, Name = @Name, Surname = @Surname, Birth = @Birth WHERE Id = @Id";
                db.Execute(sqlQuery, user);
            }
        }
    }
}
