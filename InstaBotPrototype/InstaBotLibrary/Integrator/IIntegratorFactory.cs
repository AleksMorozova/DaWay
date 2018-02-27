using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Integrator
{
    public interface IIntegratorFactory
    {
        IIntegrator Create(Bound.BoundModel model);
    }
}
