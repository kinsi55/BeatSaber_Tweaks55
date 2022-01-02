using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {

	[HarmonyPatch]
	static class BasegameSearchFulltext {
		// Minvalue so other mods can still have a list of all the seperate words
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ref string[] searchTexts) {
			if(Config.Instance.basegameFulltextSearch && searchTexts.Length != 1)
				searchTexts = new string[] { string.Join(" ", searchTexts) };
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapLevelFilterModel), "LevelContainsText", BindingFlags.NonPublic | BindingFlags.Static);
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
