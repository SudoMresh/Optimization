using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Modules
{
    public interface IModuleBase
    {
        void RunTask(params object[] objects);
    }
}
