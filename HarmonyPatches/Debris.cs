using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class Debris {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableDebris;

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(NoteDebrisSpawner), nameof(NoteDebrisSpawner.SpawnDebris));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
