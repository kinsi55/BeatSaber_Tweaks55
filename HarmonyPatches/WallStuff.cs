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

		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods() {
			yield return AccessTools.Method(typeof(ObstacleSaberSparkleEffectManager), "Update");

			var x = AccessTools.TypeByName("SiraUtil.Sabers.SiraObstacleSaberSparkleEffectManager")?.GetMethod("Update");

			if(x != null)
				yield return x;
		}
	}

	[HarmonyPatch(typeof(StretchableObstacle), nameof(StretchableObstacle.SetSizeAndColor))]
	static class WallFakeBloom {
		[HarmonyPriority(int.MaxValue)]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
			if(instructions.ElementAt(79).opcode != OpCodes.Ldarg_0) {
				Plugin.Log.Warn("disableFakeWallBloom: StretchableObstacle.SetSizeAndColor:79 was not what I expected, not patching it");
			} else {
				ILUtil.DynamicPrefix(79, ref instructions, il, AccessTools.Method(typeof(WallFakeBloom), nameof(__DisableFakeBloomProcessing)));
			}
			return instructions;
		}

		static bool __DisableFakeBloomProcessing() => !Configuration.PluginConfig.Instance.disableFakeWallBloom;

		[HarmonyPriority(int.MinValue)]
		static void Postfix(MonoBehaviour ____obstacleFakeGlow) {
			if(____obstacleFakeGlow.gameObject.activeInHierarchy && Configuration.PluginConfig.Instance.disableFakeWallBloom)
				____obstacleFakeGlow.gameObject.SetActive(false);
		}
	}
}
