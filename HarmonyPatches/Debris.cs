using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(NoteDebrisSpawner), nameof(NoteDebrisSpawner.SpawnDebris))]
	static class Debris {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableDebris;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
