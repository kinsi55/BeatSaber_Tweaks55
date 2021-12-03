using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(SaberClashEffect), nameof(SaberClashEffect.Start))]
	static class SaberClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(SaberClashEffect __instance) {
			if(!Configuration.PluginConfig.Instance.disableSaberClash)
				return true;

			__instance.enabled = false;
			return false;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("SaberClash", ex);
	}
}
