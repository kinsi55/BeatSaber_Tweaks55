using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class CutParticles {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableCutParticles;

		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods() {
			yield return Resolver.GetMethod(nameof(NoteCutParticlesEffect), nameof(NoteCutParticlesEffect.SpawnParticles));
			yield return Resolver.GetMethod(nameof(NoteCutParticlesEffect), nameof(NoteCutParticlesEffect.Awake));
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
