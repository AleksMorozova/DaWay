using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotLibrary.Bound
{
    public class BoundModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TelegramAccount { get; set; }
        public string InstagramAccount { get; set; }
        public string InstagramPassword { get; set; }
    }
}
