using HarmonyLib;
using System;
using System.Reflection;
using TMPro;
using Tweaks55.Util;

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

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(ScoreUIController), nameof(ScoreUIController.UpdateScore));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
