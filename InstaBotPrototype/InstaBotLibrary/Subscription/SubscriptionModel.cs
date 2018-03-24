using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Subscription
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        public int BoundId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
