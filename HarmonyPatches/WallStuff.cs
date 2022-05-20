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
		static FieldAccessor<ObstacleController, StretchableObstacle>.Accessor ObstacleController_StretchableObstacle;
		static FieldAccessor<StretchableObstacle, ParametricBoxFrameController>.Accessor StretchableObstacle_obstacleFrame;
		static FieldAccessor<StretchableObstacle, ParametricBoxFakeGlowController>.Accessor StretchableObstacle_obstacleFakeGlow;

		static bool CreateAccessors() {
			ObstacleController_StretchableObstacle = FieldAccessor<ObstacleController, StretchableObstacle>.GetAccessor("_stretchableObstacle");
			StretchableObstacle_obstacleFrame = FieldAccessor<StretchableObstacle, ParametricBoxFrameController>.GetAccessor("_obstacleFrame");
			StretchableObstacle_obstacleFakeGlow = FieldAccessor<StretchableObstacle, ParametricBoxFakeGlowController>.GetAccessor("_obstacleFakeGlow");
			return true;
		}

		static Color defaultColor = Color.white;

		static bool Prepare() => UnityGame.GameVersion > new AlmostVersion("1.19.1") && CreateAccessors();
		[HarmonyPriority(int.MaxValue)]
		static void Prefix(ObstacleController obstacleController) {
			/*
			 * It also deeply pains me that I have to do it this way, it really does.
			 * This is for compatability with Noodle, the double-refresh could only be saved with a transpiler
			 */
			if(Config.Instance.wallOutlineColor != defaultColor) {
				var a = ObstacleController_StretchableObstacle(ref obstacleController);
				var b = StretchableObstacle_obstacleFrame(ref a);

				b.color = Config.Instance.wallOutlineColor;
				b.Refresh();

				var c = StretchableObstacle_obstacleFakeGlow(ref a);

				if(c != null) {
					c.color = Config.Instance.wallOutlineColor;
					c.Refresh();
				}
			}
		}
		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapObjectManager), "AddSpawnedObstacleController", BindingFlags.NonPublic | BindingFlags.Instance);
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);

		[HarmonyPatch]
		static class WallOutline_1_19 {
			static bool Prepare() => UnityGame.GameVersion <= new AlmostVersion("1.19.1") && CreateAccessors();
			static void Postfix(ObstacleController __result) => Prefix(__result);
			static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapObjectManager), "SpawnObstacle");
			static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
		}
	}
}
