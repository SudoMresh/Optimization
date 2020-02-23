using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Modules
{
    public class ModulesFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IModuleBase> m_FactoryMap = new Dictionary<string, IModuleBase>();

        /// <summary>
        /// This function registers your module.
        /// </summary>
        /// <param name="modulName">Name of the module.</param>
        /// <param name="extecutionFunction">Interface of the module.</param>
        public void Register(string moduleName, IModuleBase extecutionFunction)
        {
            m_FactoryMap[moduleName] = extecutionFunction;
        }

        /// <summary>
        /// Creates the module of the specified module name.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>Created module. If it returns null - it means that your module isn't created</returns>
        public IModuleBase CreateModule(string moduleName)
        {
            if (m_FactoryMap.TryGetValue(moduleName, out IModuleBase moduleInstance))
                return moduleInstance;
            else
                return null;
        }

        /// <summary>
        /// This is private constructor. You need to register your module here.
        /// Example: You have Dijkstra module. To register this module you need:
        /// execute "Register" function with parameters module name and it's base module.
        /// </summary>
        private ModulesFactory()
        {
        }
    }
}
