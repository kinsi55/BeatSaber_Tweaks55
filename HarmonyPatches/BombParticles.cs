using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(BombExplosionEffect), nameof(BombExplosionEffect.SpawnExplosion))]
	static class BombExplosion {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableBombExplosion;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
