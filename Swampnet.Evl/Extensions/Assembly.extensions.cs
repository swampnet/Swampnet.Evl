using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    static class AssemblyExtensions
    {
        public static IEnumerable<Type> AllOfType<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(s => s.GetTypes()).Where(p => typeof(T).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
        }
    }
}
