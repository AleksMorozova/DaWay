using InstaBotLibrary.Bound;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaBotLibrary.Integrator
{
    public interface IIntegrator
    {
        void Start();
        event BotNotification SendPost;
        void Auth(BoundModel boundModel);
        void Update();
    }
}
