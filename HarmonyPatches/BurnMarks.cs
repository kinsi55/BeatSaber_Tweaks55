using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BurnMark {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(MonoBehaviour __instance) {
			if(!Config.Instance.disableBurnMarks)
				return true;

			__instance.enabled = false;
			return false;
		}

		static IEnumerable<MethodBase> TargetMethods() {
			yield return AccessTools.Method(typeof(SaberBurnMarkArea), nameof(SaberBurnMarkArea.OnEnable));

			var x = AccessTools.TypeByName("SiraUtil.Sabers.SiraSaberBurnMarkArea")?.GetMethod("OnEnable");

			if(x != null)
				yield return x;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("BurnMark", ex);
	}
}
