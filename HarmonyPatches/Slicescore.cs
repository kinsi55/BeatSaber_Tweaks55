using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class DisableSlideScore {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableSliceScore;

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(FlyingScoreSpawner), nameof(FlyingScoreSpawner.SpawnFlyingScore));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}