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
        void Auth(BoundModel boundModel);
        Task Update();
    }
}
