using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class WallClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Configuration.PluginConfig.Instance.disableWallRumbleAndParticles;

		static IEnumerable<MethodBase> TargetMethods() {
			yield return AccessTools.Method(typeof(ObstacleSaberSparkleEffectManager), "Update");

			var x = AccessTools.TypeByName("SiraUtil.Sabers.SiraObstacleSaberSparkleEffectManager")?.GetMethod("Update");

			if(x != null)
				yield return x;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("WallClash", ex);
	}

	[HarmonyPatch(typeof(BeatmapDataObstaclesMergingTransform), "CanBeMerged")]
	static class WallMerge {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(ref bool __result) => __result = !Configuration.PluginConfig.Instance.disableWallMerge;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("WallMerge", ex);
	}

	[HarmonyPatch(typeof(StretchableObstacle), nameof(StretchableObstacle.SetSizeAndColor))]
	static class WallFakeBloom {
		[HarmonyPriority(int.MaxValue)]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
			if(instructions.ElementAt(84).opcode != OpCodes.Ldarg_0) {
				Plugin.Log.Warn("disableFakeWallBloom: StretchableObstacle.SetSizeAndColor:84 was not what I expected, not patching it");
			} else {
				ILUtil.DynamicPrefix(84, ref instructions, il, AccessTools.Method(typeof(WallFakeBloom), nameof(__DisableFakeBloomProcessing)));
			}
			return instructions;
		}

		static bool __DisableFakeBloomProcessing() => !Configuration.PluginConfig.Instance.disableFakeWallBloom;

		[HarmonyPriority(int.MinValue)]
		static void Postfix(MonoBehaviour ____obstacleFakeGlow) {
			if(____obstacleFakeGlow?.gameObject.activeInHierarchy == true && Configuration.PluginConfig.Instance.disableFakeWallBloom)
				____obstacleFakeGlow.gameObject.SetActive(false);
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("WallFakeBloom", ex);
	}
}
