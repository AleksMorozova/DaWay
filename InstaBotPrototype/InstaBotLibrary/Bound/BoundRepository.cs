using Dapper;
using InstaBotLibrary.Bound;
using InstaBotLibrary.DbCommunication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace InstaBotLibrary.Bound
{
    public class BoundRepository : Repository, IBoundRepository
    {
        public BoundRepository() { }
        public BoundRepository(string str) : base(str) { }



        public void AddBound(BoundModel bound)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "INSERT INTO Bounds (UserId, TelegramAccount, InstagramAccount, InstagramPassword) VALUES(@UserId, @TelegramAccount, @InstagramAccount, @InstagramPassword); SELECT CAST(SCOPE_IDENTITY() as int)";
                int boundId = db.Query<int>(sqlQuery, bound).FirstOrDefault();
                bound.Id = boundId;
            }
        }

        public void DeleteBound(int boundId)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @boundId";
                db.Execute(sqlQuery, new { boundId });
            }
        }
        

        public List<BoundModel> getUserBounds(int userId)
        {
            List<BoundModel> bounds = null;
            using (IDbConnection db = GetConnection())
            {
                bounds = db.Query<BoundModel>("SELECT * FROM Bounds WHERE UserId = @userId", new { userId }).ToList();
            }
            return bounds;
        }

        public void UpdateBound(BoundModel bound)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "UPDATE Bounds SET TelegramAccount = @TelegramAccount, InstagramAccount = @InstagramAccount, InstagramPassword = @InstagramPassword WHERE Id = @Id";
                db.Execute(sqlQuery, bound);
            }
        }

    }
}
