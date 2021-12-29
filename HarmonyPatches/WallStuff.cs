using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(ObstacleSaberSparkleEffectManager), "Update")]
	static class WallClash {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableWallRumbleAndParticles;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch(typeof(ParametricBoxFakeGlowController), nameof(ParametricBoxFakeGlowController.Refresh))]
	static class WallFakeBloom {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(ParametricBoxFakeGlowController __instance) {
			if(!Config.Instance.disableFakeWallBloom)
				return true;

			__instance.enabled = false;
			return false;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch(typeof(StretchableObstacle), nameof(StretchableObstacle.SetSizeAndColor))]
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

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch(typeof(ParametricBoxFrameController), nameof(ParametricBoxFrameController.Refresh))]
	static class WallOutline {
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ParametricBoxFrameController __instance) {
			if(Config.Instance.wallOutlineColor != Color.white)
				__instance.color = Config.Instance.wallOutlineColor;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
