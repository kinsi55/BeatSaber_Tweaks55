using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(DefaultScenesTransitionsFromInit), nameof(DefaultScenesTransitionsFromInit.TransitionToNextScene))]
	static class PatchHealthWarning {
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ref bool goStraightToMenu) {
			if(Config.Instance.disableHealthWarning)
				goStraightToMenu = true;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
