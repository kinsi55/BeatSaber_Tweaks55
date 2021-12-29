using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(BeatLineManager), nameof(BeatLineManager.Start))]
	static class BeatLines {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(BeatLineManager __instance) {
			if(!Config.Instance.disableBeatLines)
				return true;

			__instance.enabled = false;
			return false;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
