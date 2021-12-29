using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {

	[HarmonyPatch(typeof(BeatmapLevelFilterModel), "LevelContainsText")]
	static class BasegameSearchFulltext {
		// Minvalue so other mods can still have a list of all the seperate words
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ref string[] searchTexts) {
			if(Config.Instance.basegameFulltextSearch)
				searchTexts = new string[] { string.Join(" ", searchTexts) };
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
