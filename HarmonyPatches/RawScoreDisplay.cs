using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using TMPro;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class RawScoreDisplay {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(TextMeshProUGUI ____scoreText) {
			if(!Config.Instance.disableRawScore)
				return true;

			____scoreText.text = "";
			return false;
		}

		static MethodBase TargetMethod() => AccessTools.Method(typeof(ScoreUIController), nameof(ScoreUIController.UpdateScore), new[] { typeof(int) });

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
