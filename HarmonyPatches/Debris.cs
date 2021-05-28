using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(NoteDebrisSpawner), nameof(NoteDebrisSpawner.SpawnDebris))]
	static class Debris {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Configuration.PluginConfig.Instance.disableDebris;
	}
}
