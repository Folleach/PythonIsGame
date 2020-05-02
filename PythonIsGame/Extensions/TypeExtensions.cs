using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Extensions
{
    public static class TypeExtensions
    {
        private static readonly Type[] DefaultTypes = new Type[0];
        private static readonly Type[] DefaultParameters = new Type[0];

        public static T CreateInstance<T>(this Type type, Type[] types = null, object[] parameters = null)
            where T : class
        {
            if (types == null)
                types = DefaultTypes;
            if (parameters == null)
                parameters = DefaultParameters;
            var target = type.GetConstructor(types);
            if (target == null)
                return null;
            return (T)target.Invoke(parameters);
        }
    }
}
