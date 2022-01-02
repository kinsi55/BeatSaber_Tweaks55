using HarmonyLib;
using HMUI;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class OverrideMaxScrollSpeed {
		[HarmonyPriority(int.MaxValue)]
		static void Prefix(ref Vector2 deltaPos) {
			deltaPos *= Config.Instance.scrollSpeedMultiplier;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod($"{nameof(HMUI)}.{nameof(ScrollView)}", nameof(ScrollView.HandleJoystickWasNotCenteredThisFrame), assemblyName: "HMUI");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
