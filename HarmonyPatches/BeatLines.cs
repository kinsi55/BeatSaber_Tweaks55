using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BeatLines {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Configuration.PluginConfig.Instance.disableBeatLines;

		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods() {
			yield return AccessTools.Method(typeof(BeatLineManager), nameof(BeatLineManager.HandleNoteWasSpawned));
			yield return AccessTools.Method(typeof(BeatLineManager), "Update");
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("BeatLines", ex);
	}
}
