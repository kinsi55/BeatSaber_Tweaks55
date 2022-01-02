using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BombColor {
		[HarmonyPriority(int.MaxValue)]
		static void Prefix(MonoBehaviour __instance) {
			if(Config.Instance.bombColor == Color.black)
				return;

			var c = __instance.transform.GetChild(0);

			if(c == null)
				return;

			// If CustomNotes "HMD Only" is active, there will be another nested child (The HMD bomb), which we should apply the color to instead
			if(c.childCount != 0)
				c = c.GetChild(0);

			c.GetComponent<Renderer>()?.material?.SetColor("_SimpleColor", Config.Instance.bombColor);
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BombNoteController), nameof(BombNoteController.Init));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
