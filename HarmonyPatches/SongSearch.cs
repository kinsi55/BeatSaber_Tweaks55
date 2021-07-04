using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {

	[HarmonyPatch(typeof(BeatmapLevelFilterModel), "LevelContainsText")]
	static class BasegameSearchFulltext {
		// Minvalue so other mods can still have a list of all the seperate words
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ref string[] searchTexts) {
			if(Configuration.PluginConfig.Instance.basegameFulltextSearch)
				searchTexts = new string[] { string.Join(" ", searchTexts) };
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("BasegameSearchFulltext", ex);
	}
}
