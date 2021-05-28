using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class FakeBeatmapMirror {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Configuration.PluginConfig.Instance.disableFakeMirror;

		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods() {
			foreach(var m in AccessTools.GetDeclaredMethods(typeof(MirroredBeatmapObjectManager))) {
				if(m.Name.EndsWith("WasSpawned") || m.Name.EndsWith("WasDespawned"))
					yield return m;
			}
		}
	}
}
