using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(ObstacleSaberSparkleEffectManager), "Update")]
	static class WallClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableWallRumbleAndParticles;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch(typeof(ParametricBoxFakeGlowController), nameof(ParametricBoxFakeGlowController.Refresh))]
	static class WallFakeBloom {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(ParametricBoxFakeGlowController __instance) {
			if(!Config.Instance.disableFakeWallBloom)
				return true;

			__instance.enabled = false;
			return false;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
