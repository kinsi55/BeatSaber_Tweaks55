using HarmonyLib;
using IPA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class WallClash {
		[HarmonyPriority(int.MaxValue)]
		static void Postfix(MonoBehaviour __instance) {
			if(!Config.Instance.disableSaberClash)
				return;

			__instance.enabled = false;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(ObstacleSaberSparkleEffectManager), nameof(ObstacleSaberSparkleEffectManager.Start));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	// I cant just modify the prefab because for whatever reason, enabling the wall border in the prefab still has it off in the spawned wall
	[HarmonyPatch]
	static class DisableFakeWallBloom {
		static FieldAccessor<ConditionalActivation, bool>.Accessor ConditionalActivation_activateOnFalse = 
			FieldAccessor<ConditionalActivation, bool>.GetAccessor("_activateOnFalse");

		[HarmonyPriority(int.MaxValue)]
		static void Prefix(ObstacleController ____obstaclePrefab) {
			var x = ____obstaclePrefab.GetComponentInChildren<ParametricBoxFakeGlowController>()?
				.GetComponent<ConditionalActivation>();

			if(x != null)
				ConditionalActivation_activateOnFalse(ref x) = !Config.Instance.disableFakeWallBloom;


			x = ____obstaclePrefab.GetComponentInChildren<ParametricBoxFrameController>()?
				.GetComponent<ConditionalActivation>();

			if(x != null)
				ConditionalActivation_activateOnFalse(ref x) = Config.Instance.disableFakeWallBloom;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapObjectsInstaller), "InstallBindings");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch]
	static class TransparentWall {
		static FieldAccessor<ObstacleController, GameObject[]>.Accessor ObstacleController_visualWrappers = FieldAccessor<ObstacleController, GameObject[]>.GetAccessor("_visualWrappers");

		static GameObject[] visualWrappersOriginal = null;

		[HarmonyPriority(int.MaxValue)]
		static void Postfix(ObstacleController ____obstaclePrefab) {
			if(visualWrappersOriginal != null) {
				if(Config.Instance.transparentWalls)
					return;

				ObstacleController_visualWrappers(ref ____obstaclePrefab) = visualWrappersOriginal;
				visualWrappersOriginal = null;
				return;
			}

			if(!Config.Instance.transparentWalls)
				return;

			visualWrappersOriginal = ObstacleController_visualWrappers(ref ____obstaclePrefab);

			if(visualWrappersOriginal.Length != 2)
				return;

			visualWrappersOriginal[0].SetActive(false);

			ObstacleController_visualWrappers(ref ____obstaclePrefab) = new[] { visualWrappersOriginal[1] };
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapObjectsInstaller), "InstallBindings");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch]
	static class WallOutline {
		static readonly Color defaultColor = Color.white;

		static byte colorParam = 0;

		static bool Prepare(MethodBase __originalMethod) {
			colorParam = (byte)(1 + Array.FindIndex(TargetMethod().GetParameters(), x => x.Name == "color" && x.ParameterType == typeof(Color)));

			return colorParam != 0;
		}

		internal static Color realBorderColor;
		internal static Color fakeBorderColor;

		static Color GetRealBorderColor(Color originalColor) {
			if(Config.Instance.wallOutlineColor == defaultColor)
				return originalColor;

			return realBorderColor;
		}

		static Color GetFakeBorderColor(Color originalColor) {
			if(Config.Instance.wallOutlineColor == defaultColor)
				return originalColor;

			return fakeBorderColor;
		}

		[HarmonyPriority(int.MaxValue)]
		static IEnumerable Transpiler(IEnumerable<CodeInstruction> instructions) {
			var c = new CodeMatcher(instructions);

			c.MatchForward(true,
				new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(StretchableObstacle), "_obstacleFrame")),
				new CodeMatch(OpCodes.Ldarg_S, colorParam)
			)
			.ThrowIfInvalid("_obstacleFrame color setter not found")
			.Advance(1)
			.InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(WallOutline), nameof(GetRealBorderColor))))

			.MatchForward(true,
				new CodeMatch(OpCodes.Ldfld, operand: AccessTools.Field(typeof(StretchableObstacle), "_obstacleFakeGlow")),
				new CodeMatch(OpCodes.Ldarg_S, colorParam)
			)
			.ThrowIfInvalid("_obstacleFakeGlow color setter not found")
			.Advance(1)
			.InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(WallOutline), nameof(GetFakeBorderColor))));

			return c.InstructionEnumeration();
		}
		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(StretchableObstacle), nameof(StretchableObstacle.SetSizeAndColor));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
