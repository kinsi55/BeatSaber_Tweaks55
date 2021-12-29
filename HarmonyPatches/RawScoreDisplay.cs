using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(ScoreUIController), nameof(ScoreUIController.UpdateScore))]
	static class RawScoreDisplay {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(TextMeshProUGUI ____scoreText) {
			if(!Config.Instance.disableRawScore)
				return true;

			____scoreText.text = "";
			return false;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
