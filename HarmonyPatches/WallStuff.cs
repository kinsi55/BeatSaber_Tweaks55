using HarmonyLib;
using IPA.Utilities;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class WallClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableWallRumbleAndParticles;

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(ObstacleSaberSparkleEffectManager), nameof(ObstacleSaberSparkleEffectManager.Update));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch]
	static class WallFakeBloom {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(MonoBehaviour __instance) {
			if(!Config.Instance.disableFakeWallBloom)
				return true;

			__instance.enabled = false;
			return false;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(ParametricBoxFakeGlowController), nameof(ParametricBoxFakeGlowController.Refresh), assemblyName: "HMRendering");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch]
	static class TransparentWall {
		[HarmonyPriority(int.MaxValue)]
		static void Postfix(Transform ____obstacleCore) {
			/*
			 * It deeply pains me that this is the simplest way to disable it
			 * because disabling the gameobject does not work for some reason.
			 * 
			 * I'd need a transpiler to do better
			 */
			if(Config.Instance.transparentWalls && ____obstacleCore != null)
				____obstacleCore.localScale = Vector3.zero;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(StretchableObstacle), nameof(StretchableObstacle.SetSizeAndColor));
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
