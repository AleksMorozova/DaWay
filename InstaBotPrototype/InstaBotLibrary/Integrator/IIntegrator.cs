using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaBotLibrary.Integrator
{
    public interface IIntegrator
    {
        void Start();
        Task Update();
    }
}
