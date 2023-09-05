using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace Tweaks55.Util {
	public static class Resolver {
		public static Dictionary<string, Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
			.Select(assembly => (assembly.GetName().Name, assembly))
			.GroupBy(x => x.Name)
			.Select(x => x.First())
			.ToDictionary(x => x.Name, x => x.assembly);

		public static Assembly GetAssembly(string name) {
			if(assemblies.TryGetValue(name, out Assembly x))
				return x;

			return null;
		}

		public static MethodBase GetMethod(string className, string methodName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, string assemblyName = "Main") {
			if(!assemblies.TryGetValue(assemblyName, out Assembly assembly))
				return null;

			var t = assembly.GetType(className);

			return t.GetMethod(methodName, bindingFlags);
		}
	}
}
