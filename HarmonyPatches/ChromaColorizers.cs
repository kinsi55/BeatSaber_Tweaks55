using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class DisableObstacleColorizer {
		[HarmonyPriority(int.MaxValue)]
		[HarmonyPrefix]
		static bool Prefix(ObstacleControllerBase obstactleController, Color? color) {
			return color.HasValue || !WallOutline.enabled;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod("Chroma.Colorizer.ObstacleColorizerManager", "Colorize", assemblyName: "Chroma");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch]
	static class DisableBombColorizer {
		[HarmonyPriority(int.MaxValue)]
		[HarmonyPrefix]
		static bool Prefix(NoteControllerBase noteController, Color? color) {
			return color.HasValue || Config.Instance.bombColor == BombColor.defaultColor;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod("Chroma.Colorizer.BombColorizerManager", "Colorize", assemblyName: "Chroma");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
