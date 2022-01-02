using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class SaberClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(MonoBehaviour __instance) {
			if(!Config.Instance.disableSaberClash)
				return true;

			__instance.enabled = false;
			return false;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(SaberClashEffect), nameof(SaberClashEffect.Start));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
