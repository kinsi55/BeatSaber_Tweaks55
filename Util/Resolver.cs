using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tweaks55.Util {
	public static class Resolver {
		public static Dictionary<string, Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(x => x.GetName().Name, x => x);

		public static Assembly GetAssembly(string name) {
			if(assemblies.TryGetValue(name, out Assembly x))
				return x;

			return null;
		}

		public static MethodBase GetMethod(string className, string methodName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance, string assemblyName = "Main") {
			if(!assemblies.TryGetValue(assemblyName, out Assembly assembly))
				return null;

			var t = assembly.GetType(className);

			return t?.GetMethod(methodName, bindingFlags);
		}
	}
}
