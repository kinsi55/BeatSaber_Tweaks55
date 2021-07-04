using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(SaberClashEffect), nameof(SaberClashEffect.LateUpdate))]
	static class SaberClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Configuration.PluginConfig.Instance.disableSaberClash;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("SaberClash", ex);
	}
}
