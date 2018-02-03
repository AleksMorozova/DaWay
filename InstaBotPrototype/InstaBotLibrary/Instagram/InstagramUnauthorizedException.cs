using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Instagram
{
    class InstagramUnauthorizedException : InvalidOperationException
    {
        public InstagramUnauthorizedException() : base("Instagram wasn't auntificated, please use Auth method before")
        {
            
        }
    }
}
