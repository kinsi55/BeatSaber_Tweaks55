using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BeatLines {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(MonoBehaviour __instance) {
			if(!Config.Instance.disableBeatLines)
				return true;

			__instance.enabled = false;
			return false;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatLineManager), nameof(BeatLineManager.Start));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
