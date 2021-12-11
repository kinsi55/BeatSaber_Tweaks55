using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class WallClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableWallRumbleAndParticles;

		static IEnumerable<MethodBase> TargetMethods() {
			yield return AccessTools.Method(typeof(ObstacleSaberSparkleEffectManager), "Update");

			var x = AccessTools.TypeByName("SiraUtil.Sabers.SiraObstacleSaberSparkleEffectManager")?.GetMethod("Update");

			if(x != null)
				yield return x;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("WallClash", ex);
	}

	[HarmonyPatch(typeof(ParametricBoxFakeGlowController), nameof(ParametricBoxFakeGlowController.Refresh))]
	static class WallFakeBloom {
		[HarmonyPriority(int.MinValue)]
		static bool Prefix(ParametricBoxFakeGlowController __instance) {
			if(!Config.Instance.disableFakeWallBloom)
				return true;

			__instance.enabled = false;
			return false;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("WallFakeBloom", ex);
	}
}
