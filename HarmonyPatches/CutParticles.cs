using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class CutParticles {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableCutParticles;

		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods() {
			yield return AccessTools.Method(typeof(NoteCutParticlesEffect), nameof(NoteCutParticlesEffect.SpawnParticles));
			yield return AccessTools.Method(typeof(NoteCutParticlesEffect), "Awake");
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
