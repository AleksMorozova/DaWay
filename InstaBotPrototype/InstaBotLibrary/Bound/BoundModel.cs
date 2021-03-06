﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotLibrary.Bound
{
    public class BoundModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string TelegramAccount { get; set; }
        public long? TelegramChatId {get; set;}
        public string TelegramToken {get; set;}
        public string InstagramUsername { get; set; }
        public long? InstagramId { get; set; }
        public string InstagramToken { get; set; }
    }
}
