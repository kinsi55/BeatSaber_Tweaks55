using HarmonyLib;
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
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ParametricBoxFrameController __instance) {
			if(Config.Instance.wallOutlineColor != Color.white)
				__instance.color = Config.Instance.wallOutlineColor;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(ParametricBoxFrameController), nameof(ParametricBoxFrameController.Refresh), assemblyName: "HMRendering");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
