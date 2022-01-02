using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BombExplosion {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableBombExplosion;

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BombExplosionEffect), nameof(BombExplosionEffect.SpawnExplosion));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
