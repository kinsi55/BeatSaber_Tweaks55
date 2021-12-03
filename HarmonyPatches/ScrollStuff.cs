using HarmonyLib;
using HMUI;
using System;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(ScrollView), nameof(ScrollView.HandleJoystickWasNotCenteredThisFrame))]
	static class OverrideMaxScrollSpeed {
		[HarmonyPriority(int.MaxValue)]
		static void Prefix(ref Vector2 deltaPos) {
			deltaPos *= Configuration.PluginConfig.Instance.scrollSpeedMultiplier;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("OverrideMaxScrollSpeed", ex);
	}
}
