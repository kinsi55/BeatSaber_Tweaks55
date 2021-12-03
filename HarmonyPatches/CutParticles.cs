using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(NoteCutParticlesEffect), nameof(NoteCutParticlesEffect.SpawnParticles))]
	static class CutParticles {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Configuration.PluginConfig.Instance.disableCutParticles;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("CutParticles", ex);
	}
}
