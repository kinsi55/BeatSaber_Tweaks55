using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(GameplayCoreInstaller), "InstallBindings")]
	class ZenModeActive {
		public static bool isZenMode = false;

		[HarmonyPriority(int.MinValue)]
		static void Postfix(GameplayCoreSceneSetupData ____sceneSetupData) => isZenMode = ____sceneSetupData.gameplayModifiers.zenMode;
	}

	[HarmonyPatch(typeof(BeatmapDataZenModeTransform), nameof(BeatmapDataZenModeTransform.CreateTransformedData))]
	class ZenMode {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(IReadonlyBeatmapData beatmapData, ref IReadonlyBeatmapData __result) {
			if(!Configuration.PluginConfig.Instance.wallsInZenMode)
				return true;

			BeatmapData copyWithoutBeatmapObjects = beatmapData.GetCopyWithoutBeatmapObjects();
			BeatmapData.CopyBeatmapObjectsWaypointsOnly(beatmapData, copyWithoutBeatmapObjects);

			foreach(BeatmapObjectData beatmapObjectData in beatmapData.beatmapObjectsData) {
				if(beatmapObjectData.beatmapObjectType != BeatmapObjectType.Obstacle)
					continue;

				copyWithoutBeatmapObjects.AddBeatmapObjectData(beatmapObjectData.GetCopy());
			}

			__result = copyWithoutBeatmapObjects;

			return false;
		}
	}


	[HarmonyPatch(typeof(PlayerHeadAndObstacleInteraction), nameof(PlayerHeadAndObstacleInteraction.intersectingObstacles), MethodType.Getter)]
	class ZenMode2 {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(List<ObstacleController> ____intersectingObstacles, ref List<ObstacleController> __result) {
			if(!Configuration.PluginConfig.Instance.wallsInZenMode)
				return true;

			if(!ZenModeActive.isZenMode)
				return true;

			if(____intersectingObstacles.Count != 0)
				____intersectingObstacles.Clear();

			__result = ____intersectingObstacles;

			return false;
		}
	}
}
