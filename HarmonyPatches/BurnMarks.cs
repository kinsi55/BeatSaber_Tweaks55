using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(SaberBurnMarkArea), nameof(SaberBurnMarkArea.OnEnable))]
	static class BurnMark {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(SaberBurnMarkArea __instance) {
			if(!Configuration.PluginConfig.Instance.disableBurnMarks)
				return true;

			__instance.enabled = false;
			return false;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("BurnMark", ex);
	}
}
