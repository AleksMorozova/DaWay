using Dapper;
using InstaBotLibrary.DbCommunication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace InstaBotLibrary.Bound
{
    public class BoundRepository : Repository, IBoundRepository
    {
        public BoundRepository(IDbConnectionFactory factory) :base(factory){ }


        public void AddBound(BoundModel bound)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "INSERT INTO Bounds (UserId, TelegramAccount, TelegramChatId, TelegramToken, InstagramToken, InstagramId, InstagramUsername) VALUES(@UserId, @TelegramAccount, @TelegramChatId, @TelegramToken, @InstagramToken, @InstagramId, @InstagramUsername); SELECT CAST(SCOPE_IDENTITY() as int)";
                int boundId = db.Query<int>(sqlQuery, bound).FirstOrDefault();
                bound.Id = boundId;
            }
        }

        public BoundModel GetBoundInfo(int boundId)
        {
            BoundModel bound = null;
            using (IDbConnection db = GetConnection())
            {
                bound = db.Query<BoundModel>("SELECT * FROM Bounds WHERE Id = @boundId", new { boundId }).FirstOrDefault();
            }
            return bound;
        }

        public BoundModel GetBoundByTelegramChatId(long chatId)
        {
            BoundModel bound = null;
            using (IDbConnection db = GetConnection())
            {
                bound = db.Query<BoundModel>("SELECT * FROM Bounds WHERE TelegramChatId = @chatId", new { chatId }).FirstOrDefault();
            }
            return bound;
        }

        public void SetInstagramToken(BoundModel bound)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "UPDATE Bounds SET InstagramToken = @InstagramToken WHERE Id = @Id";
                db.Execute(sqlQuery, bound);
            }
        }

        public void SetInstagramInfo(BoundModel bound)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "UPDATE Bounds SET InstagramToken = @InstagramToken, InstagramId = @InstagramId, InstagramUsername = @InstagramUsername WHERE Id = @Id";
                db.Execute(sqlQuery, bound);
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
                var sqlQuery = "UPDATE Bounds SET TelegramAccount = @TelegramAccount, TelegramChatId = @TelegramChatId, TelegramToken = @TelegramToken, InstagramToken = @InstagramToken, InstagramId = @InstagramId, InstagramUsername = @InstagramUsername WHERE Id = @Id";
                db.Execute(sqlQuery, bound);
            }
        }

    }
}
