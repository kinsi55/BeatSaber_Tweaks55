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

		static MethodBase TargetMethod() => 
			AccessTools.GetDeclaredMethods(typeof(ScoreUIController))
			.Where(m => m.Name == nameof(ScoreUIController.UpdateScore))
			.Last();
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
