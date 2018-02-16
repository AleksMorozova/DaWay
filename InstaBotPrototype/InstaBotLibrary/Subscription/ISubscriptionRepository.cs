using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Subscription
{
    public interface ISubscriptionRepository
    {
        List<SubscriptionModel> getBoundSubscriptions(int boundId);
        void AddSubscription(SubscriptionModel subscription);
        void UpdateSubscription(SubscriptionModel subscription);
        void DeleteSubscription(int subscriptionId);
    }
}
