using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(FlyingScoreSpawner), nameof(FlyingScoreSpawner.SpawnFlyingScore))]
	static class DisableSlideScore {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableSliceScore;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("DisableSlideScore", ex);
	}
}