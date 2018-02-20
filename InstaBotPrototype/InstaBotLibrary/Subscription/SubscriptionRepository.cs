using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using InstaBotLibrary.DbCommunication;

namespace InstaBotLibrary.Subscription
{
    public class SubscriptionRepository : Repository, ISubscriptionRepository
    {
        public SubscriptionRepository(IDbConnectionFactory factory) : base(factory) { }


        public void AddSubscription(SubscriptionModel subscription)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "INSERT INTO Subscriptions (BoundId, UserName, UserId) VALUES(@BoundId, @UserName, @UserId); SELECT CAST(SCOPE_IDENTITY() as int)";
                int subscriptionId = db.Query<int>(sqlQuery, subscription).FirstOrDefault();
                subscription.Id = subscriptionId;
            }
        }

        public void DeleteSubscription(int subscriptionId)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "DELETE FROM Subscriptions WHERE Id = @subscriptionId";
                db.Execute(sqlQuery, new { subscriptionId });
            }
        }

        public List<SubscriptionModel> getBoundSubscriptions(int boundId)
        {
            List<SubscriptionModel> subscriptions = null;
            using (IDbConnection db = GetConnection())
            {
                subscriptions = db.Query<SubscriptionModel>("SELECT * FROM Subscriptions WHERE BoundId = @boundId", new { boundId }).ToList();
            }
            return subscriptions;
        }

        public void UpdateSubscription(SubscriptionModel subscription)
        {
            using (IDbConnection db = GetConnection())
            {
                var sqlQuery = "UPDATE Subscriptions SET BoundId = @BoundId, UserName = @UserName, UserId = @UserId WHERE Id = @Id";
                db.Execute(sqlQuery, subscription);
            }
        }
    }
}
